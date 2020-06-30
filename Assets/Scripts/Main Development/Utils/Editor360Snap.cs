using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

public class Editor360Snap : MonoBehaviour
{
    //Singleton
    public static Editor360Snap instance;

    [Header("360 Capture")]
    [SerializeField] private int imageWidth = 1024;
    [SerializeField] private bool saveAsJPEG = false;
    [SerializeField] private Texture2D screenshot;
	//[SerializeField] private CanvasGroup ScreenshotCapture_CanvasGroup;
	//[SerializeField] private RawImage Screenshot_rawImage;

    /// <summary>
	/// This is not required on Production Build. Only for Editor
	/// </summary>
	private void Start() {
        if (!Application.isEditor)
            Destroy(this.gameObject);
		else {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
			} else {
                Destroy(this.gameObject);
			}
        }
	}

#if UNITY_EDITOR
    [Button("Clear Resource and GC")]
    void ClearRam() {
        DestroyImmediate(screenshot);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        Debug.Log("GC Called, System Cleared as far possible");
	}

    [Button("Show Screenshot Path")]
    void ShowSavePath() {
        UnityEditor.EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    /// <summary>
    /// Will take screenshot on LeftShift + S
    /// </summary>
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
            Capture360Image();
        }
    }
#endif

    public void Capture360Image() {
        //Tab_gameObject.SetActive(false);
        StartCoroutine(CaptureImage());
    }

    private IEnumerator CaptureImage() {
        yield return new WaitForEndOfFrame();
        //Captures Rendered Image into Data, and also refs out the Texture
        byte[] ScreenshotBytes = I360Render.Capture(ref screenshot, imageWidth, saveAsJPEG);
        if (ScreenshotBytes != null) {

            //Shows the Captured Screenshot to UI
            screenshot.Apply();
            //Screenshot_rawImage.texture = screenshot;

            print("Saving " + ScreenshotBytes.Length);

            string screenShotName = System.DateTime.Now.ToString("dd MMMM,yyyy HH.mm.ss");
            string myPath;

            myPath = "/storage/emulated/0/Defender_360shots/";

#if UNITY_EDITOR
            myPath = Application.persistentDataPath + "/";
#endif

            if (!Directory.Exists(myPath))
                Directory.CreateDirectory(myPath);

            //Save to File
            if (saveAsJPEG) {
                File.WriteAllBytes(myPath + screenShotName + ".jpg", ScreenshotBytes);

                print("Saved " + File.Exists(myPath + screenShotName + ".jpg"));
            } else {
                File.WriteAllBytes(myPath + screenShotName + ".png", ScreenshotBytes);

                print("Saved " + File.Exists(myPath + screenShotName + ".png"));
            }

            //Opens the Screenshot UI on garage
            //OpenCanvasGroup(ScreenshotCapture_CanvasGroup);

#if UNITY_EDITOR
            print("saved 360 image");
#endif
        }
    }
}
