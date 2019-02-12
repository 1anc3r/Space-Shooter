using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public GameObject asteroid1;
    public GameObject asteroid2;
    public GameObject asteroid3;
    public GameObject scoreText;
    public GameObject playText;
    public GameObject exitText;

    public GameObject temp;
    private RawImage background;
    private Texture2D texture;
    private AspectRatioFitter fitter;
    private int score;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Background Image"))
        {
            background = GameObject.Find("Background Image").GetComponent<RawImage>();
            fitter = GameObject.Find("Background Image").GetComponent<AspectRatioFitter>();
        }
        SetBackgroundByUrl(Path.Combine(Application.streamingAssetsPath, "Background1.jpg"));
        GameObject.Find("Play Button").GetComponent<Button>().onClick.AddListener(OnGamePlayClick);
        GameObject.Find("Exit Button").GetComponent<Button>().onClick.AddListener(OnGameExitClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBackgroundByUrl(string path)
    {
        StartCoroutine(SetBackgroundByUrlAsync(path));
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

    private void OnGamePlayClick()
    {
        GamePlay();
    }

    private void OnGameExitClick()
    {
        GameExit();
    }

    public void GamePlay()
    {
        Instantiate(player, new Vector3(0f, -4f, 0f), Quaternion.Euler(-90f, 0f, 0f));
        playText.SetActive(false);
        exitText.SetActive(false);
        StartCoroutine("SpawnAsteroids");
    }

    public void GameOver()
    {
        playText.SetActive(true);
        exitText.SetActive(true);
        StopCoroutine("SpawnAsteroids");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score:" + this.score;
    }

    private IEnumerator SpawnAsteroids()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject asteroid = asteroid1;
                float range = Mathf.Abs(UnityEngine.Random.Range(0f, 3f));
                if (range > 0 && range <= 1f)
                {
                    asteroid = asteroid1;
                }
                if (range > 1 && range <= 2f)
                {
                    asteroid = asteroid2;
                }
                if (range > 2 && range <= 3f)
                {
                    asteroid = asteroid3;
                }
                Instantiate(asteroid, new Vector3(UnityEngine.Random.Range(-2.3f, 2.3f), 5.5f, 0f), Quaternion.identity);
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator SpawnEnemys()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(enemy, new Vector3(UnityEngine.Random.Range(-2.3f, 2.3f), 5.5f, 0f), Quaternion.Euler(90f, -90f, 90f));
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }
    }
}