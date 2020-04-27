using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    [HideInInspector] public float speed = 5f;
    [HideInInspector] public Transform ThisTransform;

    [HideInInspector] public Vector3 thisPosition;
    [HideInInspector] public Vector3 targetDirection;
    private Quaternion rotation;

    public void MissileStart() {
        ThisTransform = transform;
    }

    public void MissileUpdate(Vector3 TargetPos, float deltaTime) {
        //targetDirection = (TargetPos - thisPosition).normalized;

        thisPosition.x += targetDirection.x * (speed * deltaTime);
        thisPosition.y += targetDirection.y * (speed * deltaTime);
        thisPosition.z += targetDirection.z * (speed * deltaTime);

        rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        //#if UNITY_EDITOR
        //        print(thisPosition);
        //#endif

        ThisTransform.SetPositionAndRotation(thisPosition, rotation);
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(this.gameObject);
    }
}
