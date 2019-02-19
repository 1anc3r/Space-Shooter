using System;
using System.IO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public GameObject asteroid1;
    public GameObject asteroid2;
    public GameObject asteroid3;
    public GameObject scoreText;
    public GameObject playButton;
    public GameObject exitButton;
    public GameObject chooseButton;

    private GameObject showPlayer;
    private int score;
    private bool isLoadGallery = false;

    // Use this for initialization
    void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(OnGamePlayClick);
        exitButton.GetComponent<Button>().onClick.AddListener(OnGameExitClick);
        chooseButton.GetComponent<Button>().onClick.AddListener(OnPlayerChooseClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (showPlayer)
        {
            RotateObjectToAngle(showPlayer, 1);
        }
    }

    private void OnGamePlayClick()
    {
        if (!isLoadGallery)
        {
            isLoadGallery = true;
            SceneController.Instance.LoadGalleryScene(() =>
            {
                ShowPlayer(player);
            });
        }
    }

    private void OnPlayerChooseClick()
    {
        if (isLoadGallery)
        {
            isLoadGallery = false;
            SceneController.Instance.UnloadGalleryScene(() =>
            {
                HidePlayer();
                GamePlay();
            });
        }
    }

    private void OnGameExitClick()
    {
        GameExit();
    }

    public void ShowPlayer(GameObject player)
    {
        this.player = player;
        scoreText.SetActive(false);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        chooseButton.SetActive(true);
        Quaternion rotation = Quaternion.Euler(-45, 180, 0);
        if (showPlayer)
        {
            rotation = showPlayer.transform.rotation;
            Destroy(showPlayer);
        }
        showPlayer = Instantiate(player, new Vector3(0f, 0f, 0f), rotation);
        showPlayer.transform.localScale = 0.25f * Vector3.one;
        Destroy(showPlayer.GetComponent<PlayerController>());
        Destroy(GameObject.Find("engines_player"));
    }

    public void HidePlayer()
    {
        if (showPlayer)
        {
            Destroy(showPlayer);
        }
    }

    public void GamePlay()
    {
        scoreText.SetActive(true);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        chooseButton.SetActive(false);
        Instantiate(player, new Vector3(0f, 0f, -4f), Quaternion.identity);
        StartCoroutine("SpawnWaves");
    }

    public void GameOver()
    {
        scoreText.SetActive(false);
        playButton.SetActive(true);
        exitButton.SetActive(true);
        chooseButton.SetActive(false);
        StopCoroutine("SpawnWaves");
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

    private bool RotateObjectToAngle(GameObject gameObject, float angle)
    {
        if (gameObject != null)
        {
            gameObject.transform.Rotate(Vector3.forward, angle, Space.World);
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject asteroid = asteroid1;
                float range = Mathf.Abs(UnityEngine.Random.Range(0f, 4f));
                if (range > 3f)
                {
                    asteroid = enemy;
                    Instantiate(enemy, new Vector3(UnityEngine.Random.Range(-2.3f, 2.3f), 0f, 5.5f), Quaternion.identity);
                }
                else
                {
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
                    Instantiate(asteroid, new Vector3(UnityEngine.Random.Range(-2.3f, 2.3f), 0f, 5.5f), Quaternion.identity);
                }
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }
    }
}