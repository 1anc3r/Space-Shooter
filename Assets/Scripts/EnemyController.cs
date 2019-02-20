using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人控制器
public class EnemyController : MonoBehaviour
{

    public int life = 3;                // 生命值
    public float speed = -1f;           // 速度
    public GameObject expolsion;        // 爆炸特效
    public GameObject bullet;           // 子弹
    public Transform centerLauncher;    // 中子弹发射口

    private float fireNext = 0f;        // 下次开火
    private float fireInterval = 1f;    // 开火间隔

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

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
                Camera.main.GetComponent<GameController>().AddScore(1);
                Destroy(other.gameObject);
                life--;
            }
        }
    }
}