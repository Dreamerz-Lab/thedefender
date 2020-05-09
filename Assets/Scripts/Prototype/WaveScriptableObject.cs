using UnityEngine;
using Sirenix.OdinInspector;

namespace Defender.Prototype
{
    //v1.0.1
    [CreateAssetMenu(fileName = "New Wave", menuName = "Create Wave")]
    public class WaveScriptableObject : ScriptableObject
    {
        [System.Serializable]
        public struct WaveCondition
        {
            public enum Conditions { missileCount, timer };

            public Conditions ConditionType;
            public float Value;
        }

        [System.Serializable]
        public struct MissileOrigin
        {
            public enum OriginDirections { Forward, Right, Backward, Left };

            [Tooltip("Missile Origin Direction, From this direction it's angle will be calculated")]
            public OriginDirections OriginDirection;
            [Tooltip("Max and Min Angle from the Origin Direction")]
            public float SpawnAngle;
        }

        public enum Incomings { LeftMissile, RightMissile, HeavyMissile, BigBlock };

        [Header("Missile Types")]
        [Indent(1)] public Incomings[] incomings;

        [Header("Missile Origin Directions")]
        [Indent(1)] public MissileOrigin[] missileOrigin;

        [Space(6)]
        [Tooltip("The probability of Missile to match each hand color. " +
            "The more the Value, The more it will be randomized")]
        [Indent(1)] [Range(0, 1)] public float MissileHandMatchProbability;

        [Tooltip("The time frequency of Missile Spawn")]
        [Indent(1)] public float MissileSpawnFrequency;

        [Tooltip("Number of Missiles to be spawned ")]
        [Indent(1)] [Range(1, 3)] public int AtATimeSpawn;

        [Space(6)]
        [Header("Missile Spawn Y Position")]
        [Indent(1)] public float MinY;
        [Indent(1)] public float MaxY;

        [Space(6)]
        [Header("Missile Speed")]
        [Indent(1)] public float MinSpeed;
        [Indent(1)] public float MaxSpeed;
        [Indent(1)] public float StaticSpeed;

        [Space(6)]
        [Indent(1)] public WaveCondition[] WaveEndCondition;
    }
}