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
    private GameObject mainRoot;

    // Start is called before the first frame update
    void Start()
    {
        mainRoot = GameObject.Find("MainWrapper");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
        {
            mainCtrl = mainRoot.GetComponent<MainCtrl>();
            asyncSceneLoader = mainRoot.GetComponent<AysnLoader>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitTilSceneLoaded()
    {
        yield return null;
    }

    public void switchScene(string nextSceneName)
    {
        //bool isLoaded = mainCtrl.IsSceneLoaded(nextSceneName);

        if (!mainCtrl.IsSceneLoaded(nextSceneName))
        {
            //asyncSceneLoader.SetLoadScene(WaitTilSceneLoaded());
            asyncSceneLoader.StartLoading("Loading", WaitTilSceneLoaded());

            while (!mainCtrl.IsSceneLoaded(nextSceneName))
            {
                StartCoroutine(WaitTilSceneLoaded());
                //isLoaded = mainCtrl.IsSceneLoaded(nextSceneName);
            }

            asyncSceneLoader.StartUnloading("Loading");
        }

        mainRoot.SetActive(false);
        GameObject[]gameObjs = SceneManager.GetSceneByName(nextSceneName).GetRootGameObjects();

        for (int i = 0; i < gameObjs.Length; i++)
        {
            gameObjs[i].SetActive(true);
        }
    }

    public void PlayButton()
    {
        switchScene("StageMapScene");
    }
}
