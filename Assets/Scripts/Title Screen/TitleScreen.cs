using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    [SerializeField]
    int sceneToLoad;

    [SerializeField]
    int sceneToUnload;

    [SerializeField]
    bool shouldUnload;

    [SerializeField]
    GameObject virtualCam;

    Camera[] cameras;

    [SerializeField]
    bool shouldQuit;

    bool gameHasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        cameras = FindObjectsOfType<Camera>();
        Debug.Log(cameras.Length);
        if(cameras.Length > 1)
        {
            for(int i = 1; i < cameras.Length; i++)
            {
                Destroy(cameras[i]);
            }
        }

        virtualCam.SetActive(true);

        if (shouldUnload)
        {
            StartCoroutine(UnloadLevel());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shouldQuit)
            {
                Application.Quit();
            }

            if (!gameHasStarted && !shouldQuit)
            {
                gameHasStarted = true;
                LoadNextLevel();
            }
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    IEnumerator UnloadLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}
