using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 小行星控制器
public class AsteroidController : MonoBehaviour
{

    public int life = 0;            // 生命值
    public float speed = -5f;       // 速度
    public GameObject expolsion;    // 爆炸特效

    private float tumble = 5f;      // 随机旋转

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }

    void Update()
    {
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