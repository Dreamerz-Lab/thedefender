#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SceneViewCameraFollower : MonoBehaviour {
	public Defender.Core.WaveManager m_WaveManager;
	public int MissileIndex;
	public bool on = true;
	public bool onlyInPlayMode = false;
	public SceneViewFollower[] sceneViewFollowers;
	private ArrayList sceneViews;

	void LateUpdate() {
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			MissileIndex = (MissileIndex + 1) % m_WaveManager.MissilesIncoming.Count;
		} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			MissileIndex = (MissileIndex - 1) % m_WaveManager.MissilesIncoming.Count;
		}

		if (sceneViewFollowers[0].targetTransform == null && m_WaveManager.MissilesIncoming.Count > 0) {
			MissileIndex = m_WaveManager.MissilesIncoming.Count - 1;
			sceneViewFollowers[0].targetTransform = m_WaveManager.MissilesIncoming[MissileIndex];
		}

		if (sceneViewFollowers != null && sceneViews != null) {
			foreach (SceneViewFollower svf in sceneViewFollowers) {
				if (svf.targetTransform == null) svf.targetTransform = transform;
				svf.size = Mathf.Clamp(svf.size, .01f, float.PositiveInfinity);
				svf.sceneViewIndex = Mathf.Clamp(svf.sceneViewIndex, 0, sceneViews.Count - 1);
			}
		}

		if (Application.isPlaying)
			Follow();
	}

	public void OnDrawGizmos() {
		if (!Application.isPlaying)
			Follow();
	}

	void Follow() {
		sceneViews = UnityEditor.SceneView.sceneViews;
		if (sceneViewFollowers == null || !on || sceneViews.Count == 0) return;

		foreach (SceneViewFollower svf in sceneViewFollowers) {
			UnityEditor.SceneView sceneView = (UnityEditor.SceneView)sceneViews[svf.sceneViewIndex];
			if (sceneView != null) {
				if ((Application.isPlaying && onlyInPlayMode) || !onlyInPlayMode) {
					sceneView.orthographic = svf.orthographic;
					//sceneView.look
					sceneView.pivot = svf.targetTransform.position + svf.TargetOffset;

					if (svf.enableFixedRotation)
						sceneView.rotation = Quaternion.AngleAxis(svf.Angle + svf.targetTransform.localEulerAngles.y, Vector3.up);

				}
			}
		}
	}

	[System.Serializable]
	public class SceneViewFollower {
		[Header("Target Properties")]
		[Space(6)]
		public Transform targetTransform;
		public Vector3 TargetOffset;

		[Space(2)]
		[Header("Rotation Properties")]
		[Space(2)]
		public bool enableFixedRotation;
		public float Angle;

		[Space(2)]
		[Header("Camera Setup")]
		public bool orthographic;
		[HideInInspector] public float size;
		[HideInInspector] public int sceneViewIndex;

		SceneViewFollower() {
			TargetOffset = Vector3.zero;
			enableFixedRotation = false;
			//fixedRotation = Vector3.zero;
			size = 5;
			orthographic = true;
			sceneViewIndex = 0;
		}
	}

}
#endif