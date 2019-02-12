using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private RawImage background;
    private Texture2D texture;
    private AspectRatioFitter fitter;

	// Use this for initialization
	void Start () {
        if (GameObject.Find("Background Image")) {
            background = GameObject.Find("Background Image").GetComponent<RawImage>();
            fitter = GameObject.Find("Background Image").GetComponent<AspectRatioFitter>();
        }
        SetBackgroundByUrl(Path.Combine (Application.streamingAssetsPath, "Background1.jpg"));
	}

	// Update is called once per frame
	void Update () {

	}

	// 切换背景图片，异步接口
    public void SetBackgroundByUrl (string path) {
        StartCoroutine (SetBackgroundByUrlAsync (path));
    }

    private IEnumerator SetBackgroundByUrlAsync(string path)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(path, true))
        {
            yield return request.SendWebRequest();
            if (request.responseCode == 200)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                fitter.aspectRatio = texture.width * 1f / texture.height;
                background.texture = texture;
            }
            else
            {
                Debug.Log("Set " + path + " to background failed. Error: " + request.error);
            }
        }
    }
}