using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Defender.UI;
using Defender.Data;
using System;
using System.Collections;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Defender.Core {
	/// <summary>
	/// Main GameManager class that manages all game data
	/// Acts like a bridge for every other system
	/// </summary>
	public class GameManager : MonoBehaviour {
		#region EditorDebugging
#if UNITY_EDITOR
		[Button("Increase Heavy Shield Count")]
		private void IncreaseHeavyShield() {
			HeavyShieldCount++;
		}
#endif
		#endregion

		#region Variables
		//GameManager Singleton
		public static GameManager instance;

		public bool isGameStart = false;
		private float deltaTime = 0f;

		//Player Body Collider Position
		private Vector3 BodyColliderPos;
		[SerializeField] private Transform BodyCollider_TRNS;

		[Space(6)]
		[SerializeField] private AudioSource GamePlayMusic;
		private Coroutine StopMusicCoroutine;
		private WaitForSeconds _wait = new WaitForSeconds(0.1f);

		#region WAVE
		[Header("Wave Data")]
		//The Wave Manager to handle all Wave Logics
		[SerializeField] private WaveManager m_WaveManager;
		#endregion

		#region PlayerHealth
		[Space(6)]
		[Header("")]
		[SerializeField] private int m_playerHealthCount = 4;

		public int PlayerHealth {
			get {
				return m_playerHealthCount;
			}
			set {
				if(value <= 0) {
					value = 0;
					GamePlayUI.instance.PlayerHealthCount_Img.fillAmount = 0;
					GameEnd();
				} else {
					//Clams the value to 4
					value = value > 4 ? 4 : value;
					GamePlayUI.instance.PlayerHealthCount_Img.fillAmount = value * 0.25f;
				}

				m_playerHealthCount = value;
			}
		}

		[SerializeField] private float HealthUpdateFrequency = 2f;
		private float healthTimeCounter = 0f;
		#endregion

		#region Heavy Shield
		[Space(6)]
		[Header("Heavy Shield Data")]
		//The Heavy Shield Count, No. of Times Shield can be activated
		[SerializeField] private int m_heavyShieldCount = 4;

		public int HeavyShieldCount {
			get { return m_heavyShieldCount; }
			set {
				//If Shield value is 0 or less, adjusts it to 0
				//Deactivates the Fusion Colliders to avoid Bug
				if (value <= 0) {
					value = 0;
					GamePlayUI.instance.HeavyShieldCount_Img.fillAmount = 0;
					FusionCollidersEnableState(false);
				} else { //Else, Activates the Fusion Colliders
					FusionCollidersEnableState(true);
					//Clams the value to 4
					value = value > 4 ? 4 : value;
					//Updates UI accordingly
					GamePlayUI.instance.HeavyShieldCount_Img.fillAmount = value * 0.25f;
				}

				m_heavyShieldCount = value;
			}
		}

		//The Component that handles Heavy Shield Activation
		[SerializeField] private BoxCollider[] FusionColliders;
		//The Active Status of the Heavy Shield
		public bool isHeavyShieldActive = false;
		//The Timer for the Heavy Shield
		//After that the Shield will be disabled
		public float HeavyShieldTimer = 5f;

		[SerializeField] private float HeavyShieldUpdateFrequency = 4f;
		private float shieldTimeCounter = 0f;
		#endregion //HeavyShield Region

		#region SCORE_VARIABLES
#if UNITY_EDITOR
		[ReadOnly]
#endif
		[SerializeField] private float Score = 0f;
		[SerializeField] private TMP_Text Score_TXT;
		#endregion //Score_variables Region

		#region UI_POSITIONING
		[Space(6)]
		[Header("UI Positioning")]
		[SerializeField] private Transform Player;
		[SerializeField] private Transform MainGameplay_Canvas;
		[SerializeField] private float UILerpSpeed = 6f;
		private Vector3 PlayerEulerAngle;
		#endregion

		#endregion //Variables Region

		#region UNITYCALLBACKS

		/// <summary>
		/// Singleton setup
		/// </summary>
		private void Awake() {
			instance = this;
		}

		/// <summary>
		/// Initialize Game Data
		/// </summary>
		private void Start() {
			//Updating the Heavy Shield Count
			HeavyShieldCount = m_heavyShieldCount;
			PlayerHealth = m_playerHealthCount;

			isGameStart = false;

			//Initialize Score
			Score = 0f;
			Score_TXT.text = System.Math.Round(Score, 2).ToString();

			PlayerEulerAngle = new Vector3(0f,0f,0f);
			BodyColliderPos = m_WaveManager.TargetOffset;
			_wait = new WaitForSeconds(0.1f);

			//Start the Game
			Invoke(nameof(GameStart), 2f);
		}

		/// <summary>
		/// Called once each frame
		/// </summary>
		private void Update() {
			if (!isGameStart) {
				return;
			}

			deltaTime = Time.deltaTime;

			//update Score on Each Frame
			Score += deltaTime;
			Score_TXT.text = System.Math.Round(Score, 2).ToString();

			//PlayerEulerAngle.y = Mathf.Lerp(PlayerEulerAngle.y, Player.eulerAngles.y, UILerpSpeed * deltaTime);
			PlayerEulerAngle.y = Player.eulerAngles.y;
			MainGameplay_Canvas.eulerAngles = PlayerEulerAngle;

			BodyCollider_TRNS.position = BodyColliderPos + Player.position;

			//Player health Updater
			healthTimeCounter += deltaTime;
			//Shield Delta updater
			shieldTimeCounter += deltaTime;
			GamePlayUI.instance.PlayerHealthCount_Img.fillAmount += (deltaTime / HealthUpdateFrequency) * 0.25f;
			GamePlayUI.instance.HeavyShieldCount_Img.fillAmount += (deltaTime / HeavyShieldUpdateFrequency) * 0.25f;

			if (healthTimeCounter > HealthUpdateFrequency) {
				healthTimeCounter = 0f;
				PlayerHealth++;
			}

			if (shieldTimeCounter > HeavyShieldUpdateFrequency) {
				shieldTimeCounter = 0f;
				HeavyShieldCount++;
			}
		}
		#endregion

		#region PublicFunctions
		/// <summary>
		/// Function that hanldes Game start logic
		/// </summary>
		public void GameStart() {
#if UNITY_EDITOR
			print("GAME STARTED");
#endif
			isGameStart = true;

			//Start Wave Update Logic
			m_WaveManager.isGameStart = true;

			//The Wave is setup
			m_WaveManager.SetupNextWave();
			//Invoke(nameof(GameEnd), 5f);

			PlayMainMusic();
		}

		/// <summary>
		/// The Function that handles Game Ending logic
		/// </summary>
		public void GameEnd() {
			m_WaveManager.StopWaveManager();
			isGameStart = false;

			if (StopMusicCoroutine != null)
				StopCoroutine(StopMusicCoroutine);
			StopMusicCoroutine = StartCoroutine(StopMusic());

			UI.GamePlayUI.instance.gameOverMenu.UpdateScoreBoard(Score);

			GamePlayUI.instance.gameOverMenu.OpenMenu();
			GamePlayUI.instance.ToggleLaserUI(true);
		}

		public void ToggleGamePause() {
			if (!isGameStart) {
				Invoke(nameof(AwaitedToggleCallback), 1.5f);
				PlayMainMusic();
			} else {
				AwaitedToggleCallback();
				if (StopMusicCoroutine != null)
					StopCoroutine(StopMusicCoroutine);
				StopMusicCoroutine = StartCoroutine(StopMusic());
			}

			GamePlayUI.instance.pauseMenu.TogglePauseMenu();
			GamePlayUI.instance.ToggleLaserUI();
		}

		private void AwaitedToggleCallback() {
			isGameStart = !isGameStart;
			m_WaveManager.isGameStart = isGameStart;
		}

		public void PlayMainMusic() {
			if (!GameSettingsData.instance.isMusicOn)
				return;

			if (StopMusicCoroutine != null)
				StopCoroutine(StopMusicCoroutine);

			GamePlayMusic.pitch = 1;
			GamePlayMusic.volume = 1;
			GamePlayMusic.Play();
		}

		public IEnumerator StopMusic() {
			if (!GameSettingsData.instance.isMusicOn)
				yield break;

			while (GamePlayMusic.volume > 0) {
				GamePlayMusic.volume -= 0.05f;
				GamePlayMusic.pitch -= 0.05f;
				yield return _wait;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="damage"></param>
		public void TakeDamage(float damage = 1) {
			PlayerHealth--;
			healthTimeCounter = 0f;
		}

		#endregion

		#region PrivateFunctions

		/// <summary>
		/// Handles the Fusion Collider active State
		/// Disables when player runs out of Fusion Counts
		/// Enables if the player gains the ability
		/// </summary>
		/// <param name="state"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void FusionCollidersEnableState(bool state) {
			for (byte i = 0; i < FusionColliders.Length; i++)
				FusionColliders[i].enabled = state;
		}

		#endregion
	}
}