using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool LoadOnStart;
    [SerializeField] private int SceneIndex;
    [SerializeField] private float waitTimeBeforeLoad = 3f;

    // Start is called before the first frame update
    void Start()
    {
        if(LoadOnStart)
            StartCoroutine(LoadScene(SceneIndex));
    }

	public void OnLoadRequest() {
        StartCoroutine(LoadScene(SceneIndex));
    }

	private IEnumerator LoadScene(int index) {
        if(waitTimeBeforeLoad > 0)
            yield return new WaitForSeconds(waitTimeBeforeLoad);

        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
        yield return async;
	}
}
