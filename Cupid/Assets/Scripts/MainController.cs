using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using AsynLoader = AsyncLoader;

public class MainController : MonoBehaviour
{
    private AsynLoader asyncSceneLoader;
    private Dictionary<string, bool> isScenesLoaded;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("main controller!");

        asyncSceneLoader = gameObject.GetComponent<AsynLoader>();
        isScenesLoaded = new Dictionary<string, bool>();
        isScenesLoaded.Add("StageMapScene", false);
        isScenesLoaded.Add("StageScene", false);

        List<string> preloadScenes = new List<string>();
        preloadScenes.Add("StageMapScene");
        preloadScenes.Add("StageScene");

        List<IEnumerator> inactiveScenes = new List<IEnumerator>();
        inactiveScenes.Add(SetSceneInactive("StageMapScene"));
        inactiveScenes.Add(SetSceneInactive("StageScene"));

        //asyncSceneLoader.SetBatchLoadScenes(inactiveScenes);
        asyncSceneLoader.StartBatchLoading(preloadScenes, inactiveScenes);

        Debug.Log("Preload all scenes.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SetSceneInactive(string name)
    {
        isScenesLoaded[name] = true;
        GameObject[] gameObjs = SceneManager.GetSceneByName(name).GetRootGameObjects();

        for (int i = 0; i < gameObjs.Length; i++)
        {
            gameObjs[i].SetActive(false);
        }

        yield return null;
    }

    public bool IsSceneLoaded(string name)
    {
        return isScenesLoaded[name];
    }
}
