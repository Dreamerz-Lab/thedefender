using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defender.Data;

namespace Defender.Core {
	/// <summary>
	/// Manager Class that Handles all the Incoming Generations
	/// Missiles, Fireball, Blocks are Pooled, Spawned and Updated
	/// </summary>
	public class WaveManager : MonoBehaviour {
	#region Variables
		[Header("Wave Config")] public WaveConfigData CurrentWave;

		//The Condition to End This Wave
		private WaveConfigData.WaveCondition m_WaveEndCondition;

		[Header("Incoming Prefabs")]
		//An Array of All the Incomings
		[SerializeField] private Transform[] AllIncomings;
		
		//Holds all the Left Missile Type1
		[SerializeField] private List<Transform> m_LeftMissile;

		//Holds all the Right Missile Type1
		[SerializeField] private List<Transform> m_RightMissile;

		//Holds the Spawned incoming Missiles
		[SerializeField] private List<Transform> m_SpawnedMissile;

		[Header("Wave Data")]
		//The count of total spawned missiles in the current wave
		[SerializeField]
		private int TotalSpawnCount;

		//The timer till the wave started
		[SerializeField] private float WaveElapsedTime;

		//The Frequency of Incoming Spawning
		private WaitForSeconds m_SpawnFrequency;
		private float m_SpawnFrequencyf;

		//Number of Spawn each spawn time
		private int m_AtATimeSpawn;

		private bool isWaveStart = false;

		private Coroutine[] SpawningRoutines;

	#endregion //Variables

	#region UNITYCALLBACK
		/// <summary>
		/// 
		/// </summary>
		private void Awake() {
			//Initialize Spawning Sub Routine holder
			SpawningRoutines = new Coroutine[2];
		}
		
		/// <summary>
		/// 
		/// </summary>
		private void Start() {
			isWaveStart = false;
			lastSpawnTime = 0f;
		}

		private float lastSpawnTime = 0f;

		private void Updates() {
			if(!isWaveStart)
				return;
			
			float _currentTime = Time.time;

			//Spawning
			if ((_currentTime - lastSpawnTime) >= m_SpawnFrequencyf) {
				lastSpawnTime = _currentTime;
				//Spawn Missiles
				if (TotalSpawnCount < m_WaveEndCondition.Value) {
#if UNITY_EDITOR
					print("Spawned Missile");
#endif
					if (m_LeftMissile.Count <= 0) {
						UnityEditor.EditorApplication.isPaused = true;
					}
					else {
						m_SpawnedMissile.Add(m_LeftMissile[m_LeftMissile.Count - 1]);
						m_LeftMissile.Remove(m_LeftMissile[m_LeftMissile.Count - 1]);
					}

					TotalSpawnCount++;
				}
			}
			
			for (int i = 0; i < m_SpawnedMissile.Count; i++) {
				m_SpawnedMissile[i].transform.position += m_SpawnedMissile[i].transform.forward;
			}

			//When a Wave is Finished, Start the Next Wave
			if (TotalSpawnCount >= m_WaveEndCondition.Value) {
				isWaveStart = false;
				GameManager.instance.GetNextWave();
			}
		}

	#endregion //Unitycallback Region

	#region PublicFunctions

		/// <summary>
		/// Starts a new Wave
		/// </summary>
		/// <param name="_newWave"></param>
		public void StartWave(WaveConfigData _newWave) {
			//Stop all the Spawning Sub Routines if there exists any
			for (byte i = 0; i < SpawningRoutines.Length; i++) {
				if(SpawningRoutines[i] != null)
					StopCoroutine(SpawningRoutines[i]);
			}

			//Sets the Current Wave as the New Starting Wave
			CurrentWave = _newWave;
			//Sets the Wave End Condition for Current Wave
			m_WaveEndCondition = CurrentWave.WaveEndCondition;
			//Sets the Spawning Frequency
			m_SpawnFrequency = new WaitForSeconds(CurrentWave.MissileSpawnFrequency);
			m_SpawnFrequencyf = CurrentWave.MissileSpawnFrequency;
			
			//Total Spawn Count for Current Wave
			m_AtATimeSpawn = CurrentWave.AtATimeSpawn;
			//init Total Spawn Count Data
			TotalSpawnCount = 0;
			
			isWaveStart = true;
			if (m_WaveEndCondition.ConditionType == WaveConfigData.WaveCondition.Conditions.timer) {
				// SpawningRoutines[0] = StartCoroutine(UpdateWaveCountBased());
			}
			else {
				SpawningRoutines[0] = StartCoroutine(UpdateWaveCountBased());
			}
			
			SpawningRoutines[1] = StartCoroutine(UpdateMissiles());
		}

	#endregion //Public Functions Region

	#region PrivateFunction

		IEnumerator UpdateWaveCountBased() {
			while (TotalSpawnCount < m_WaveEndCondition.Value) {
#if UNITY_EDITOR
				print("Spawned Missile");
#endif
				if (m_LeftMissile.Count <= 0) {
					// UnityEditor.EditorApplication.isPaused = true;
					m_LeftMissile.Add(Instantiate(AllIncomings[0],Vector3.zero,Quaternion.identity));
				}
				else {
					m_SpawnedMissile.Add(m_LeftMissile[m_LeftMissile.Count - 1]);
					m_LeftMissile.Remove(m_LeftMissile[m_LeftMissile.Count - 1]);
				}

				TotalSpawnCount++;
				yield return m_SpawnFrequency;
			}
		}

		IEnumerator UpdateWaveTimerBased() {
			while (true) {
				yield return m_SpawnFrequency;
			}
		}

		IEnumerator UpdateMissiles() {
			while (true) {
				int _spawned = m_SpawnedMissile.Count;
				for (int i = 0; i < _spawned; i++) {
					m_SpawnedMissile[i].transform.position += m_SpawnedMissile[i].transform.forward;
				}
				
				//If Wave has Ended and All Missiles are Destroyed, Go To Next Wave
				if(_spawned <= 0 && TotalSpawnCount >= m_WaveEndCondition.Value)
					GameManager.instance.GetNextWave();
				
				yield return null;
			}
		}

		private void SpawnMissile() {

		}

	#endregion //Private Functions Region
	}
}