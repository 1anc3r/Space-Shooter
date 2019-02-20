using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆炸控制器
public class ExplosionController : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject, 2);
    }

    void Update()
    {

    }
}