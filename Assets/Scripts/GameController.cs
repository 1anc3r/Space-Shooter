using System;
using System.IO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// 游戏控制器
public class GameController : MonoBehaviour
{

    public GameObject player;           // 玩家预制体
    public GameObject enemy;            // 敌人预制体
    public GameObject asteroid1;        // 小型小行星预制体
    public GameObject asteroid2;        // 中型小行星预制体
    public GameObject asteroid3;        // 大型小行星预制体
    public GameObject scoreText;        // 游戏分数展示
    public GameObject playButton;       // 开始游戏按钮
    public GameObject exitButton;       // 结束游戏按钮
    public GameObject chooseButton;     // 选择飞船按钮

    private GameObject showPlayer;      // 飞船模型
    private int score;                  // 游戏分数
    private bool isLoadGallery = false; // 是否加载场景

    void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(OnGamePlayClick);
        exitButton.GetComponent<Button>().onClick.AddListener(OnGameExitClick);
        chooseButton.GetComponent<Button>().onClick.AddListener(OnPlayerChooseClick);
    }

    void Update()
    {
        if (showPlayer)
        {
            RotateObjectToAngle(showPlayer, 1);
        }
    }

    private void OnGamePlayClick()
    {
        // 加载飞船展示场景
        if (!isLoadGallery)
        {
            isLoadGallery = true;
            SceneController.Instance.LoadGalleryScene(() =>
            {
                // 在回调中展示飞船
                ShowPlayer(player);
            });
        }
    }

    private void OnPlayerChooseClick()
    {
        // 卸载飞船展示场景
        if (isLoadGallery)
        {
            isLoadGallery = false;
            SceneController.Instance.UnloadGalleryScene(() =>
            {
                // 在回调中隐藏飞船并开始游戏
                HidePlayer();
                GamePlay();
            });
        }
    }

    private void OnGameExitClick()
    {
        Application.Quit();
    }

    // 展示飞船模型
    public void ShowPlayer(GameObject player)
    {
        this.player = player;
        scoreText.SetActive(false);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        chooseButton.SetActive(true);
        // 如果飞船模型不存在， 则以初始旋转角度生成飞船模型，否则继承前一个飞船模型的旋转角度并销毁前一个飞船模型
        Quaternion rotation = Quaternion.Euler(-45, 180, 0);
        if (showPlayer)
        {
            rotation = showPlayer.transform.rotation;
            HidePlayer();
        }
        showPlayer = Instantiate(player, new Vector3(0f, 0f, 0f), rotation);
        showPlayer.transform.localScale = 0.25f * Vector3.one;
        // 飞船模型不需要控制器和特效
        Destroy(showPlayer.GetComponent<PlayerController>());
        Destroy(GameObject.Find("engines_player"));
    }

    // 隐藏飞船模型
    public void HidePlayer()
    {
        if (showPlayer)
        {
            Destroy(showPlayer);
        }
    }

    // 游戏开始
    public void GamePlay()
    {
        scoreText.SetActive(true);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        chooseButton.SetActive(false);
        Instantiate(player, new Vector3(0f, 0f, -4f), Quaternion.identity);
        StartCoroutine("SpawnWaves");
    }

    // 游戏结束
    public void GameOver()
    {
        scoreText.SetActive(false);
        playButton.SetActive(true);
        exitButton.SetActive(true);
        chooseButton.SetActive(false);
        StopCoroutine("SpawnWaves");
    }

    // 得分
    public void AddScore(int score)
    {
        this.score += score;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score:" + this.score;
    }

    // 以指定角度旋转指定物体
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

    // 生成小行星或敌人波次
    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            // 每波生成10个
            for (int i = 0; i < 10; i++)
            {
                GameObject asteroid = asteroid1;
                float range = Mathf.Abs(UnityEngine.Random.Range(0f, 4f));
                // 掷骰子决定是敌人还是小行星
                if (range > 3f)
                {
                    asteroid = enemy;
                    Instantiate(enemy, new Vector3(UnityEngine.Random.Range(-2.3f, 2.3f), 0f, 5.5f), Quaternion.identity);
                }
                else
                {
                    // 掷骰子决定小行星大小
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