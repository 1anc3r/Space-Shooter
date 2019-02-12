using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject asteroid;
    public GameObject player;

    private RawImage background;
    private Texture2D texture;
    private AspectRatioFitter fitter;
    private int score;

    // Use this for initialization
    void Start () {
        if (GameObject.Find ("Background Image")) {
            background = GameObject.Find ("Background Image").GetComponent<RawImage> ();
            fitter = GameObject.Find ("Background Image").GetComponent<AspectRatioFitter> ();
        }
        SetBackgroundByUrl (Path.Combine (Application.streamingAssetsPath, "Background1.jpg"));
        GamePlay ();
        StartCoroutine (SpawnWaves ());
    }

    // Update is called once per frame
    void Update () {
        
    }

    public void SetBackgroundByUrl (string path) {
        StartCoroutine (SetBackgroundByUrlAsync (path));
    }

    private IEnumerator SetBackgroundByUrlAsync (string path) {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture (path, true)) {
            yield return request.SendWebRequest ();
            if (request.responseCode == 200) {
                Texture2D texture = DownloadHandlerTexture.GetContent (request);
                fitter.aspectRatio = texture.width * 1f / texture.height;
                background.texture = texture;
            } else {
                Debug.Log ("Set " + path + " to background failed. Error: " + request.error);
            }
        }
    }

    public void GamePlay () {
        Instantiate (player, new Vector3 (0f, -4f, 0f), Quaternion.Euler (-90f, 0f, 0f));
    }

    public void GameOver () {
        GamePlay ();
    }

    public void AddScore (int score) {
        this.score += score;
    }

    private IEnumerator SpawnWaves () {
        yield return new WaitForSeconds (1);
        while (true) {
            for (int i = 0; i < 10; i++) {
                Instantiate (asteroid, new Vector3 (UnityEngine.Random.Range (-2.3f, 2.3f), 5.5f, 0f), Quaternion.identity);
                yield return new WaitForSeconds (1);
            }
            yield return new WaitForSeconds (1);
        }
    }
}