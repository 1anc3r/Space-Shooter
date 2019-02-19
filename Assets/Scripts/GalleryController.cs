using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject player5;
    public GameObject player6;
    public GameObject player7;
    public GameObject player8;
    public GameObject player9;
    public GameObject player10;
    public GameObject player11;
    public GameObject player12;
    public GameObject player13;

    // Use this for initialization
    void Start()
    {
        GameObject.Find("Player1 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(1));
        GameObject.Find("Player2 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(2));
        GameObject.Find("Player3 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(3));
        GameObject.Find("Player4 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(4));
        GameObject.Find("Player5 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(5));
        GameObject.Find("Player6 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(6));
        GameObject.Find("Player7 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(7));
        GameObject.Find("Player8 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(8));
        GameObject.Find("Player9 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(9));
        GameObject.Find("Player10 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(10));
        GameObject.Find("Player11 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(11));
        GameObject.Find("Player12 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(12));
        GameObject.Find("Player13 Button").GetComponent<Button>().onClick.AddListener(() => OnItemClick(13));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnItemClick(int index)
    {
        if (GameObject.Find("Main Camera"))
        {
            GameObject player = player1;
            switch (index)
            {
                case 1:
                    player = player1;
                    break;
                case 2:
                    player = player2;
                    break;
                case 3:
                    player = player3;
                    break;
                case 4:
                    player = player4;
                    break;
                case 5:
                    player = player5;
                    break;
                case 6:
                    player = player6;
                    break;
                case 7:
                    player = player7;
                    break;
                case 8:
                    player = player8;
                    break;
                case 9:
                    player = player9;
                    break;
                case 10:
                    player = player10;
                    break;
                case 11:
                    player = player11;
                    break;
                case 12:
                    player = player12;
                    break;
                case 13:
                    player = player13;
                    break;
            }
            GameObject.Find("Main Camera").GetComponent<GameController>().ShowPlayer(player);
        }
    }
}