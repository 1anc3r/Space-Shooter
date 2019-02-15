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
    public GameObject playText;
    public GameObject exitText;
    public GameObject chooseText;
    
    private RawImage background;
    private GameObject showPlayer;
    private Texture2D texture;
    private AspectRatioFitter fitter;
    private int score;
    private bool isLoadGallery = false;
    private readonly float rotateFactory = 100f;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Background Image"))
        {
            background = GameObject.Find("Background Image").GetComponent<RawImage>();
            fitter = GameObject.Find("Background Image").GetComponent<AspectRatioFitter>();
            SetBackgroundByUrl(Path.Combine(Application.streamingAssetsPath, "Background" + (int)UnityEngine.Random.Range(1, 4) + ".jpg"));
        }
        GameObject.Find("Play Button").GetComponent<Button>().onClick.AddListener(OnGamePlayClick);
        GameObject.Find("Exit Button").GetComponent<Button>().onClick.AddListener(OnGameExitClick);
        GameObject.Find("Choose Button").GetComponent<Button>().onClick.AddListener(OnChooseClick);
    }

    // Update is called once per frame
    void Update()
    {
        if(showPlayer)
        {
            RotateObjectToAngle(showPlayer, 1);
        }
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

    private void loadGalleryScene () {
        SceneManager.sceneLoaded += loadedEve;
        Resources.UnloadUnusedAssets ();
        StartCoroutine (loadSceneAsync (1, 1));
    }

    private void unloadGalleryScene () {
        SceneManager.sceneUnloaded += unloadedEve;
        Resources.UnloadUnusedAssets ();
        StartCoroutine (unloadSceneAsync (1));
    }

    private IEnumerator loadSceneAsync (int sceneIndex, int mode = 0) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (sceneIndex, mode == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    private IEnumerator unloadSceneAsync (int sceneIndex) {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync (sceneIndex);
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    private void loadedEve (Scene scene, LoadSceneMode level) {
        ShowPlayer();
        SceneManager.sceneLoaded -= loadedEve;
    }

    private void unloadedEve (Scene scene) {
        GamePlay();
        SceneManager.sceneUnloaded -= unloadedEve;
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

    private void OnGamePlayClick()
    {
        if (!isLoadGallery)
        {
            isLoadGallery = true;
            loadGalleryScene();
        }
    }

    private void OnGameExitClick()
    {
        GameExit();
    }

    private void OnChooseClick()
    {
        if (isLoadGallery)
        {
            isLoadGallery = false;
            unloadGalleryScene ();
        }
    }

    public void GamePlay()
    {
        if(showPlayer) {
            Destroy(showPlayer);
        }
        scoreText.SetActive(true);
        playText.SetActive(false);
        exitText.SetActive(false);
        chooseText.SetActive(false);
        Instantiate(player, new Vector3(0f, 0f, -4f), Quaternion.identity);
        StartCoroutine("SpawnWaves");
    }

    public void GameOver()
    {
        scoreText.SetActive(false);
        playText.SetActive(true);
        exitText.SetActive(true);
        chooseText.SetActive(false);
        StopCoroutine("SpawnWaves");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        ShowPlayer();
    }

    public void ShowPlayer ()
    {
        scoreText.SetActive(false);
        playText.SetActive(false);
        exitText.SetActive(false);
        chooseText.SetActive(true);
        Quaternion rotation = Quaternion.Euler(-45, 180, 0);
        if(showPlayer) {
            rotation = showPlayer.transform.rotation;
            Destroy(showPlayer);
        }
        showPlayer = Instantiate(player, new Vector3(0f, 0f, 0f), rotation);
        showPlayer.transform.localScale = 0.25f * Vector3.one;
        Destroy(showPlayer.GetComponent<PlayerController>());
        Destroy(GameObject.Find("engines_player"));
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score:" + this.score;
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
                } else {
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