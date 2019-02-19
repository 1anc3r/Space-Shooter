using System;
using System.IO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneController : MonoBehaviour
{
    static SceneController _instance;

    // 场景管理器单例
    static public SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindObjectOfType(typeof(SceneController)) as SceneController;

                if (_instance == null)
                {
                    GameObject go = new GameObject("_SceneController");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<SceneController>();
                }
            }
            return _instance;
        }
    }

    public void LoadGalleryScene(Action OnComplete = null)
    {
        SceneManager.sceneLoaded += LoadedSceneEve;
        Resources.UnloadUnusedAssets();
        StartCoroutine(LoadSceneAsync(1, 1, OnComplete));
    }

    public void UnloadGalleryScene(Action OnComplete = null)
    {
        SceneManager.sceneUnloaded += UnloadedSceneEve;
        Resources.UnloadUnusedAssets();
        StartCoroutine(UnloadSceneAsync(1, OnComplete));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, int mode = 0, Action OnComplete = null)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, mode == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (OnComplete != null)
        {
            // 完成回调
            OnComplete();
        }
    }

    private IEnumerator UnloadSceneAsync(int sceneIndex, Action OnComplete = null)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (OnComplete != null)
        {
            // 完成回调
            OnComplete();
        }
    }

    private void LoadedSceneEve(Scene scene, LoadSceneMode level)
    {
        SceneManager.sceneLoaded -= LoadedSceneEve;
    }

    private void UnloadedSceneEve(Scene scene)
    {
        SceneManager.sceneUnloaded -= UnloadedSceneEve;
    }
}