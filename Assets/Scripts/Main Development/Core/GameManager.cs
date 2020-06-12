using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using Defender.UI;
using Defender.Data;
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

		#region WAVE
		[Header("Wave Data")]
		//The Wave Manager to handle all Wave Logics
		[SerializeField] private WaveManager m_WaveManager;
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
					UIManager.instace.HeavyShieldCount_Img.fillAmount = 0;
					FusionCollidersEnableState(false);
				} else { //Else, Activates the Fusion Colliders
					FusionCollidersEnableState(true);
					//Clams the value to 4
					value = value > 4 ? 4 : value;
					//Updates UI accordingly
					UIManager.instace.HeavyShieldCount_Img.fillAmount = value * 0.25f;
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
		#endregion //HeavyShield Region

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

			//Start the Game
			Invoke(nameof(GameStart), 2f);
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
			//Start Wave Update Logic
			m_WaveManager.isGameStart = true;

			//The Wave is setup
			m_WaveManager.SetupNextWave();
			//Invoke(nameof(GameEnd), 5f);
		}

		/// <summary>
		/// The Function that handles Game Ending logic
		/// </summary>
		public void GameEnd() {
			m_WaveManager.isGameStart = false;
		}

		public void GetNextWave() {

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="damage"></param>
		public void TakeDamage(float damage) {

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