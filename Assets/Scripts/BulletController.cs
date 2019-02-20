using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 子弹控制器
public class BulletController : MonoBehaviour
{

    public float speed = 5f;    // 速度

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.up * speed;
    }

    void Update()
    {
        if (transform.position.z < -5.5f || transform.position.z > 5.5f)
        {
            Destroy(gameObject);
        }
    }
}