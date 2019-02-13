using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int life = 3;
    public float speed = -1f;
    public GameObject expolsion;
    public GameObject bullet;
    public Transform centerLauncher;

    private float fireNext = 0f;
    private float fireInterval = 1f;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player"))
        {
			transform.LookAt(GameObject.FindWithTag("Player").transform.position);
			transform.position = Vector3.MoveTowards(transform.position, GameObject.FindWithTag("Player").transform.position, Time.deltaTime * 0.5f);
        	if (Time.time > fireNext)
        	{
        	    fireNext = Time.time + fireInterval;
       	     	Instantiate(bullet, centerLauncher.position, centerLauncher.rotation);
            	GetComponent<AudioSource>().Play();
        	}
        }
        if (transform.position.z < -5.5f)
        {
            Destroy(gameObject);
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
            if (other.tag == "Fire")
            {
                GameObject.Find("Main Camera").GetComponent<GameController>().AddScore(1);
                Destroy(other.gameObject);
                life--;
            }
        }
    }
}