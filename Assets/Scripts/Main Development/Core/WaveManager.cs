using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defender.Data;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Defender.Core {
	/// <summary>
	/// Manager Class that Handles all the Incoming Generations
	/// Missiles, Fireball, Blocks are Pooled, Spawned and Updated
	/// </summary>
	public class WaveManager : MonoBehaviour {

#if UNITY_EDITOR
		[ReadOnly]
#endif
		public bool isGameStart = false;

		[Header("All Waves")]
		//Contains all the wave config data
		[SerializeField] private WaveConfigData[] Waves;
		//Current Wave Index
		[SerializeField] private int m_CurrentIndex = 0;

		//Counts The Number of Waves Passed
#if UNITY_EDITOR
		[ReadOnly]
#endif
		[Space(6)]
		[SerializeField] private int WaveCounter = 1;
		[SerializeField] private TMPro.TMP_Text WaveCounter_TXT;
		[SerializeField] private UnityEngine.UI.Image WaveBar_IMG;

		[Header("Current Wave Config")]
		//The Current wave of the Game
		public WaveConfigData CurrentWave;

		//The Ending Logic of Current Wave
		[SerializeField] private WaveConfigData.WaveCondition WaveEndCondition;

		//Array of All the Incomings
		[SerializeField] private Transform[] IncomingPrefabs;

		//List of all the Spawned Incomings, moving towards the player
		public List<Transform> MissilesIncoming;
		//List of all the Spawned Obstalces, moving towards it's direction
		public List<Transform> ObstaclesIncoming;
		//List of Target Directions of the Spawned Obstacles
		public List<Vector3> Obstacles_Direction;

		[Space(6)]
		[Header("Block Spawn Points")]
		//The Spawn points Transforms only from which Blocks can be spawned
		[SerializeField] private Transform[] BlockSpawnTransforms;
		//The Spawn points only from which Blocks can be spawned
		[SerializeField] private Vector3[] BlockSpawnPoints;

		//The Speed of Spawned Missiles
		private float MissileSpeed;
		//The Speed of the Spawned Fireballs
		private float FireBallSpeed;
		//The Speed of the Spawned Blocks
		private float BlockSpeed;

		//Time Since Current Wave Start
#if UNITY_EDITOR
		[ReadOnly]
#endif
		[SerializeField] private float WaveTime = 0f;

		//Total Missile Spawned in Current Wave
#if UNITY_EDITOR
		[ReadOnly]
#endif
		[SerializeField] private int TotalMissileCount = 0;

//		//Holds the WaveEnd status
//#if UNITY_EDITOR
//		[ReadOnly]
//#endif
//		[SerializeField] private bool isWaveEnd = false;

		//Spawn Frequency
		private WaitForSeconds SpawnFrequency;
		//The SubRoutine that handles spawning Loop
		private Coroutine SpawningRoutine;

#if UNITY_EDITOR
		[ReadOnly]
#endif
		//The spawn angle from the Last Missile
		[SerializeField] private float LastSpawnAngle;

#if UNITY_EDITOR
		[ReadOnly]
#endif
		//The Change in Angle from last Spawned Missile
		[SerializeField] private float DeltaAngle;

		[Header("Spawn Radius")]
		//Spawn Radius of the Missile from Center
		[SerializeField] private float MissileSpawnRadius;
#if UNITY_EDITOR
		//Color for the Spawn Radius Gizmos on the Editor
		[SerializeField] private Color SpawnRadiusColor;
#endif

		[Space(6)]
		[Header("Player Data")]
		//The Player Transform Reference for Spawning
		[SerializeField] private Transform Player;
		//Holds The Updated Player Position
		[SerializeField] private Vector3 PlayerPosition;
		public Vector3 TargetOffset;

		//Random Object, for better Randomization
		private System.Random random;

		#region UNITYCALLBACKS
		/// <summary>
		/// Called on Awake. Initializes Random
		/// </summary>
		private void Awake() {
			random = new System.Random(29453);
		}

		/// <summary>
		/// Unity Start Function logic
		/// </summary>
		private void Start() {
			//Create new array based on the length of Transforms
			BlockSpawnPoints = new Vector3[BlockSpawnTransforms.Length];
			//Pass the Transform Positions to the Spawn Point Vectors
			for (int i = 0; i < BlockSpawnTransforms.Length; i++) {
				BlockSpawnPoints[i] = BlockSpawnTransforms[i].position;
			}

			isGameStart = false;

			//The default Starting Index for Wave
			m_CurrentIndex = 0;

			//Init Wave Counter Data
			WaveCounter = 1;
			WaveCounter_TXT.text = WaveCounter.ToString();

			//Therefore, First wave will Always start from Forward
			//Ignoring Wave Missile Direction at the Beginning
			LastSpawnAngle = 0f;
		}

		/// <summary>
		/// Missiles Update Logic
		/// </summary>
		private void Update() {
			if (!isGameStart)
				return;

			//Gets the Updated Player Position
			PlayerPosition = Player.position;
			//Add Offset to the Position
			PlayerPosition.x += TargetOffset.x;
			PlayerPosition.y += TargetOffset.y;
			PlayerPosition.z += TargetOffset.z;

			//Updates the Missiles
			MoveIncomings();
		}


#if UNITY_EDITOR
		/// <summary>
		/// Called to show Spawn Radius Gizmos on the Editor
		/// </summary>
		private void OnDrawGizmos() {
			Gizmos.color = SpawnRadiusColor;
			Gizmos.DrawWireSphere(Vector3.zero, MissileSpawnRadius);
		}
#endif
		#endregion

		#region PUBLIC_FUNCTIONS
		public void StopWaveManager() {
			isGameStart = false;
			UI.GamePlayUI.instance.gameOverMenu.UpdateWaveBoard(WaveCounter);

			//The default Starting Index for Wave
			m_CurrentIndex = 0;

			//Init Wave Counter Data
			WaveCounter = 1;
			WaveCounter_TXT.text = WaveCounter.ToString();

			//Resets the Current Wave counting data
			WaveTime = 0f;
			TotalMissileCount = 0;

			LastSpawnAngle = 0f;

			//If an older routine is still running, it will be stopped
			if (SpawningRoutine != null)
				StopCoroutine(SpawningRoutine);
		}

		/// <summary>
		/// Cache next wave data
		/// </summary>
		public void SetupNextWave() {
			//Updates the Current Wave to the New Wave
			CurrentWave = Waves[m_CurrentIndex];

			//Get Speed Data
			MissileSpeed = CurrentWave.Speed;
			FireBallSpeed = CurrentWave.FireBallSpeed;
			BlockSpeed = CurrentWave.BlockSpeed;

			//Get Spawn Frequency
			SpawnFrequency = new WaitForSeconds(CurrentWave.MissileSpawnFrequency);

			//Get Wave End Condition
			WaveEndCondition = CurrentWave.WaveEndCondition;

			//Resets the Current Wave counting data
			WaveTime = 0f;
			TotalMissileCount = 0;

			//Gets the Delta angle from Current Wave Data
			DeltaAngle = CurrentWave.DeltaAngle;

			//If an older routine is still running, it will be stopped
			if (SpawningRoutine != null)
				StopCoroutine(SpawningRoutine);

			//holds the Routine of Current New Spawning
			SpawningRoutine = StartCoroutine("SpawnIncomingsTimeBased");
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		/// <summary>
		/// Move all the Incomings toward the Player
		/// </summary>
		private void MoveIncomings() {
			Vector3 _direction;
			float _deltaTime = Time.deltaTime;
			float _updatedMissileSpeed = MissileSpeed * _deltaTime;

			//Iterates through all the Missiles
			for (int i = 0; i < MissilesIncoming.Count; i++) {
				if (MissilesIncoming[i] == null) {
					MissilesIncoming.RemoveAt(i);
					continue;
				}

				//get the normalized Direction of each missile from Player
				_direction = (PlayerPosition - MissilesIncoming[i].position).normalized;

				//Rotation towards the Player
				Quaternion _rotation = Quaternion.LookRotation(_direction);

				//Update Direction to Speed Value
				_direction.x *= _updatedMissileSpeed;
				_direction.y *= _updatedMissileSpeed;
				_direction.z *= _updatedMissileSpeed;

				//Move and Rotate missile towards the player
				MissilesIncoming[i].SetPositionAndRotation(MissilesIncoming[i].position + _direction, _rotation);
			}

			for(int i = 0; i < ObstaclesIncoming.Count; i++) {
				if(ObstaclesIncoming[i] == null) {
					ObstaclesIncoming.RemoveAt(i);
					Obstacles_Direction.RemoveAt(i);
					continue;
				}

				//Move Forward
				ObstaclesIncoming[i].position += (Obstacles_Direction[i] * _updatedMissileSpeed);
			}
		}

		/// <summary>
		/// Spawn incomings based on the Spawn Frequency
		/// </summary>
		private IEnumerator SpawnIncomingsTimeBased() {
			while (WaveTime < WaveEndCondition.Value) {
				//When Game Paused, Wait until unpause
				if (!isGameStart)
					yield return new WaitUntil(() => { return isGameStart; });

				//The Incoming Index to Spawn at Player
				//Decide what to Spawn
				int _index = (int)CurrentWave.incomings[Random.Range(2424, 9340) % CurrentWave.incomings.Length];

				//If Block
				if(_index >= 5) {
					//The position on SpawnRadius based on the Last Spawn Angle
					Vector3 _spawnPos = Player.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * LastSpawnAngle) * MissileSpawnRadius, 0f, Mathf.Cos(Mathf.Deg2Rad * LastSpawnAngle) * MissileSpawnRadius);

					//Choose the nearest Spawn Point from the last Spawned Angle
					int _spawnIndex = 0;

					//Take the distance from First Spawn Point to the Spawn pos as Minimum
					float _minDistance = Vector3.Distance(_spawnPos, BlockSpawnPoints[0]);
					//Iterates to find the index of the Spawn point that is nearest the last spawn angle
					for(int i =1; i< BlockSpawnPoints.Length; i++) {
						float _distance = Vector3.Distance(_spawnPos, BlockSpawnPoints[i]);
						if (_distance < _minDistance) {
							_minDistance = _distance;
							_spawnIndex = i;
						}
					}

					//Set Spawn Pos as the nearest position from the angle
					_spawnPos = BlockSpawnPoints[_spawnIndex];
					
					Vector3 _targetDirection = (PlayerPosition - _spawnPos).normalized;

					Transform _obstacle = Instantiate(IncomingPrefabs[_index], _spawnPos, Quaternion.LookRotation(_targetDirection, Vector3.up));
					//Add the Instantiated obstacle at Spawn Pos, into the List
					ObstaclesIncoming.Add(_obstacle);
					Obstacles_Direction.Add(_targetDirection);
					//Destroy after 10s
					Destroy(_obstacle.gameObject, 10f);

				} else { //If anything else than blocks
					//Current Wave Spawn Angle Range MinMax
					float _angleRange = CurrentWave.SpawnAngle;
					//The actual Spawn Angle Considering, AngleRange and Origin Direction
					float _spawnAngle = (90f * (int)CurrentWave.OriginDirection) +
						RandomRange(Mathf.Max(LastSpawnAngle - DeltaAngle, -_angleRange),
						Mathf.Min(LastSpawnAngle + DeltaAngle, _angleRange));

					//Spawn Angle Stored as Last Spawn Angle
					LastSpawnAngle = _spawnAngle;

					//The Direction of Spawn for the missile
					Vector3 lDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * _spawnAngle) * MissileSpawnRadius, RandomRange(CurrentWave.MinY, CurrentWave.MaxY), Mathf.Cos(Mathf.Deg2Rad * _spawnAngle) * MissileSpawnRadius);
					//The Final Spawn Position of the Missile
					Vector3 _spawnPos = Player.position + lDirection;

					//Add the Instantiated Missile at Spawn Pos, into the List
					MissilesIncoming.Add(Instantiate(IncomingPrefabs[_index], _spawnPos, Quaternion.LookRotation(-1 * lDirection)));
				}
#if UNITY_EDITOR
				print("Spawned " + TotalMissileCount + " : " + Time.time);
#endif

				WaveTime += CurrentWave.MissileSpawnFrequency;
				TotalMissileCount++;

				//Update Wave Progress Bar
				WaveBar_IMG.fillAmount = 1 - (WaveTime/WaveEndCondition.Value);

				//Destroy(_missile.gameObject, 10f);
				yield return SpawnFrequency;
			}

			//Update wave as finished
			//Check and Setup Next Wave
			NextWaveCheck();
		}

		/// <summary>
		/// Check if Next Wave Should Start
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void NextWaveCheck() {
			//New Wave will be started
			//WaveEnd status is reset
			//isWaveEnd = false;

			WaveCounter++;
			WaveCounter_TXT.text = WaveCounter.ToString();

			//Update Wave Index
			//Circular Index ++
			m_CurrentIndex = (m_CurrentIndex + 1) % Waves.Length;

			//Start New Wave after NextWaveTimeOffset
			Invoke(nameof(SetupNextWave), CurrentWave.NextWaveTimeOffset);
		}

		/// <summary>
		/// A Random number from min [Inclusive] to max [Non-Inclusive] is Generated and Returned
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float RandomRange(float min, float max) {
			return ((float)random.NextDouble() * (max - min)) + min;
		}

		#endregion
	}
}