using UnityEngine;
using Sirenix.OdinInspector;

namespace Defender.Data {
	/// <summary>
	/// Wave Data Holder.
	/// </summary>
    [CreateAssetMenu(fileName = "Wave Data", menuName = "Create Wave Data")]
    public class WaveConfigData : ScriptableObject {
		//Missile Origin Directions
		public enum OriginDirections { Forward, Right, Backward, Left };
		//Incomings
        public enum Incomings { LeftMissile, LeftMissileBall, RightMissile, RightMissileBall, FireBall, Blocks };

		/// <summary>
		/// Data for Wave Ending Condition
		/// </summary>
		[System.Serializable]
        public struct WaveCondition {
            public enum Conditions { missileCount, timer };
            public Conditions ConditionType;
            public float Value;
        }

        //Types of incomings towards the Player in this wave
        [Header("Missile Types")]
        [Indent(1)] public Incomings[] incomings;

		/// <summary>
		/// Missile Angle Towards the Player in this wave
		/// </summary>
        [Header("Missile Origin Directions")]
		//The Main Direction From which Missile Will come
        [Tooltip("Missile Origin Direction, From this direction it's angle will be calculated")]
        [Indent(1)] public OriginDirections OriginDirection;
		//The Min Max Spawning angle from the Origin Direction
        [Tooltip("Max and Min Angle from the Origin Direction")]
        [Indent(1)] public float SpawnAngle;
		//The Max Change in Angle from last missile angle
        [Tooltip("Max Change in Angle from last Missile Spawned Angle")]
        [Indent(1)] public float DeltaAngle;

		//The Missile Rate
        [Tooltip("The time frequency of Missile Spawn")]
        [Indent(1)] public float MissileSpawnFrequency;

		//Numbers of missiles at a time may come to the player
        [Tooltip("Number of Missiles to be spawned ")]
        [Indent(1)] [Range(1, 2)] public int AtATimeSpawn;

		//The Y position
        [Space(6)]
        [Header("Missile Spawn Y Position")]
        [Indent(1)] public float MinY;
        [Indent(1)] public float MaxY;

		//Different types of speeds for the Incomings
        [Space(6)]
        [Header("Missile Speed")]
        [Indent(1)] public float Speed;
        [Indent(1)] public float FireBallSpeed;
        [Indent(1)] public float BlockSpeed;

		//How this Wave will end
        [Space(6)]
        [Indent(1)] public WaveCondition WaveEndCondition;
        //Time after which Next Wave will start
        [Indent(1)] public int NextWaveTimeOffset;
    }
}