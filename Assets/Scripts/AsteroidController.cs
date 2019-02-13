using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{

    public int type = 0;
    public GameObject expolsion;

    private float speed = -5f;
    private float tumble = 5f;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -5.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Fire")
        {
            if (type - 1 == 0)
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
                type--;
            }
        }
    }
}