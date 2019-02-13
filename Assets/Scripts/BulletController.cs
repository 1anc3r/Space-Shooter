using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed = 5f;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > 5.5f)
        {
            Destroy(gameObject);
        }
    }
}