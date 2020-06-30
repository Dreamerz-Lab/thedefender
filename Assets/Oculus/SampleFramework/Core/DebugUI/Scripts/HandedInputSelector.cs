/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided ?AS IS? WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class HandedInputSelector : MonoBehaviour
{
    public static HandedInputSelector instance;

    [HideInInspector] public OVRInput.Controller ActiveController;

    [SerializeField] private OVRInput.Button LeftHand;
    [SerializeField] private OVRInput.Button RightHand;

    OVRCameraRig m_CameraRig;
    OVRInputModule m_InputModule;

	private void Awake() {
        instance = this;
	}

	void Start()
    {
        m_CameraRig = FindObjectOfType<OVRCameraRig>();
        m_InputModule = FindObjectOfType<OVRInputModule>();

        SetActiveController(OVRInput.Controller.RTouch);
    }

    void Update()
    {
        if (OVRInput.GetDown(LeftHand)) {
            SetActiveController(OVRInput.Controller.LTouch);
        } else if (OVRInput.GetDown(RightHand)) {
            SetActiveController(OVRInput.Controller.RTouch);
        }
    }

    void SetActiveController(OVRInput.Controller c)
    {
        Transform t;
        ActiveController = c;

        if (c == OVRInput.Controller.LTouch)
        {
            t = m_CameraRig.leftHandAnchor;
        }
        else
        {
            t = m_CameraRig.rightHandAnchor;
        }
        m_InputModule.rayTransform = t;
    }
}
