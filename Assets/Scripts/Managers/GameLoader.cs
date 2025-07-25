using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer m_VideoPlayer;
    private bool Temp_val;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMainSceneCo());
    }

    IEnumerator LoadMainSceneCo()
    {
        yield return new WaitForSeconds(Constants.INT_ONE);
        m_VideoPlayer.Play();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(Constants.MAINGAMESCENE);
        asyncOperation.allowSceneActivation = false;
        //load Scene
        while (!asyncOperation.isDone)
        {
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(Constants.GAMELOAD_DELAY);
                asyncOperation.allowSceneActivation = true;
            }
        }
    }
}
