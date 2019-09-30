using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MainCtrl = MainController;

public class AsyncLoader : MonoBehaviour
{
    private LoadSceneMode mode = LoadSceneMode.Single;
    private IEnumerator coroutine;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLoading(string sceneName, IEnumerator scene)
    {
        coroutine = asynchronousLoadScene(sceneName, scene);
        StartCoroutine(coroutine);
    }

    IEnumerator asynchronousLoadScene(string sceneName, IEnumerator scene)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (Mathf.Approximately(operation.progress, 0.9f))
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
        yield return StartCoroutine(scene);
    }

    public void StartBatchLoading(List<string> sceneNames, List<IEnumerator> scenes)
    {
        coroutine = BatchLoadingScenes(sceneNames, scenes);
        StartCoroutine(coroutine);
    }

    IEnumerator BatchLoadingScenes(List<string> sceneNames, List<IEnumerator> scenes)
    {
        List<AsyncOperation> BatchAsynOperation = new List<AsyncOperation>();

        for (int i = 0; i < sceneNames.Count; i++)
        {
            AsyncOperation SceneLoading = SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);
            SceneLoading.allowSceneActivation = false;
            BatchAsynOperation.Add(SceneLoading);

            while (BatchAsynOperation[i].progress < 0.9f)
            {
                yield return null;
            }
        }

        for (int i = 0; i < BatchAsynOperation.Count; i++)
        {
            BatchAsynOperation[i].allowSceneActivation = true;
            while (!BatchAsynOperation[i].isDone)
            {
                yield return null;
            }

            yield return StartCoroutine(scenes[i]);
        }
    }

    public void StartUnloading(string name)
    {
        StartCoroutine(UnloadScene(name));
    }

    IEnumerator UnloadScene(string name)
    {
        AsyncOperation unloading = SceneManager.UnloadSceneAsync(name);

        while (!unloading.isDone)
        {
            yield return null;
        }
    }
}
