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

		#region WaveData

		[Header("Wave Config Data")]
		//Holds the Component of Wave Manager
		[SerializeField] private WaveManager m_WaveManager;
		//Holds all the Waves of the Game
		[SerializeField] private WaveConfigData[] m_AllWaves;
#if UNITY_EDITOR
		[ReadOnly]
#endif
		//Current Wave
		[SerializeField] private WaveConfigData m_CurrentWave;
		//The Ending condition of the Current Wave
		private WaveConfigData.WaveCondition m_WaveEndCondition;
		[SerializeField] private int m_CurrentIndex = 0;
		#endregion //WaveData

		#region Heavy Shield

		[Space(6)]
		[Header("Heavy Shield Data")]
		//The Heavy Shield Count, No. of Times Shield can be activated
		[SerializeField]
		private int m_heavyShieldCount = 4;

		public int HeavyShieldCount {
			get { return m_heavyShieldCount; }
			set {
				//If Shield value is 0 or less, adjusts it to 0
				//Deactivates the Fusion Colliders to avoid Bug
				if (value <= 0) {
					value = 0;
					UIManager.instace.HeavyShieldCount_Img.fillAmount = 0;
					FusionCollidersEnableState(false);
				}
				else { //Else, Activates the Fusion Colliders
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

			m_CurrentIndex = 0;
			
			// Invoke(nameof(GameStart), 1f);
		}

		#endregion

		#region PublicFunctions

		/// <summary>
		/// The Function Responsible to start the Game
		/// </summary>
		public void GameStart() {
			//Initialize the First wave as CurrentWave
			m_CurrentWave = m_AllWaves[m_CurrentIndex];
			//Get the Wave End Condition Data
			m_WaveEndCondition = m_CurrentWave.WaveEndCondition;
			
			//Setup the First Wave and Start Spawner
			m_WaveManager.StartWave(m_CurrentWave);
		}

		/// <summary>
		/// The Function Responsible for Game Ending
		/// </summary>
		public void GameEnd() {

		}

		public void GetNextWave() {
			#if UNITY_EDITOR
			print("Next Wave Get");
			#endif
			//The Current index will be cyclic when it reaches the all wave count
			m_CurrentIndex = (m_CurrentIndex + 1)%m_AllWaves.Length;
			
			//Initialize the First wave as CurrentWave
			m_CurrentWave = m_AllWaves[m_CurrentIndex];
			//Get the Wave End Condition Data
			m_WaveEndCondition = m_CurrentWave.WaveEndCondition;
			
			//Setup the First Wave and Start Spawner
			m_WaveManager.StartWave(m_CurrentWave);
		}

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