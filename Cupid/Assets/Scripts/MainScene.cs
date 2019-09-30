using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MainCtrl = MainController;
using AysnLoader = AsyncLoader;

public class MainScene : MonoBehaviour
{
    private MainCtrl mainCtrl;
    private AysnLoader asyncSceneLoader;
    private GameObject root;

    // Start is called before the first frame update
    void Start()
    {
        root = GameObject.Find("MainWrapper");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
        {
            mainCtrl = root.GetComponent<MainCtrl>();
            asyncSceneLoader = root.GetComponent<AysnLoader>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        switchScene("StageMapScene");
    }

    IEnumerator WaitTilSceneLoaded()
    {
        yield return null;
    }

    public void switchScene(string nextSceneName)
    {
        Debug.Log(mainCtrl.IsSceneLoaded(nextSceneName));

        if (!mainCtrl.IsSceneLoaded(nextSceneName))
        {
            asyncSceneLoader.StartLoading("Loading", WaitTilSceneLoaded());

            while (!mainCtrl.IsSceneLoaded(nextSceneName))
            {
                StartCoroutine(WaitTilSceneLoaded());
            }

            asyncSceneLoader.StartUnloading("Loading");
        }

        root.SetActive(false);
        GameObject[] gameObjs = SceneManager.GetSceneByName(nextSceneName).GetRootGameObjects();

        for (int i = 0; i < gameObjs.Length; i++)
        {
            gameObjs[i].SetActive(true);
        }
    }
}
