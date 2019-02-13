using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject expolsion;

    private float speed = -1f;
    private int life = 3;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("StarSparrow1(Clone)"))
        {
			transform.LookAt(GameObject.Find("StarSparrow1(Clone)").transform.position);
			transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("StarSparrow1(Clone)").transform.position, Time.deltaTime * 0.5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Fire")
        {
            if (life - 1 == 0)
            {
                Instantiate(expolsion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            if (other.tag == "Player")
            {
                GameObject.Find("Main Camera").GetComponent<GameController>().GameOver();
            }
            if (other.tag == "Fire")
            {
                GameObject.Find("Main Camera").GetComponent<GameController>().AddScore(1);
                Destroy(other.gameObject);
                life--;
            }
        }
    }
}