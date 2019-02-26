using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int type = 1;                // 类型
    public float speed = 5;             // 移动速度
    public bool isUpgrade = false;      // 是否升级
    public GameObject expolsion;        // 爆炸特效
    public GameObject bullet;           // 子弹
    public Transform leftLauncher;      // 左子弹发射口
    public Transform centerLauncher;    // 中子弹发射口
    public Transform rightLauncher;     // 右子弹发射口

    private float tilt = 5;             // 倾斜
    private float fireNext = 0f;        // 下次开火
    private float fireInterval = 0.25f; // 开火间隔
    private float moveHorizontal;       // 水平移动偏移
    private float moveVertical;         // 垂直移动偏移
    private Vector3 moveVector;         // 移动偏移

    void Start()
    {

    }

    void Update()
    {
        Fire();
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid" || other.tag == "Enemy" || other.tag == "Fire")
        {
            if (other.name != "Bullet1(Clone)")
            {
                Instantiate(expolsion, transform.position, transform.rotation);
                Destroy(gameObject);
                if (other.tag == "Fire")
                {
                    Destroy(other.gameObject);
                }
                Camera.main.GetComponent<CameraController>().ShakeX();
                Camera.main.GetComponent<CameraController>().ShakeY();
                Camera.main.GetComponent<GameController>().GameOver();
            }
        }
    }

    // 开火
    private void Fire()
    {
        if (Time.time > fireNext)
        {
            fireNext = Time.time + fireInterval;
            if (isUpgrade)
            {
                Instantiate(bullet, leftLauncher.position, leftLauncher.rotation);
                Instantiate(bullet, rightLauncher.position, rightLauncher.rotation);
            }
            Instantiate(bullet, centerLauncher.position, centerLauncher.rotation);
            GetComponent<AudioSource>().Play();
        }
    }

    // 移动
    private void Move()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        moveVector = new Vector3(moveHorizontal, 0f, moveVertical);
        GetComponent<Rigidbody>().velocity = moveVector * speed;
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0f, 0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        GetComponent<Rigidbody>().position = new Vector3(Mathf.Clamp(GetComponent<Rigidbody>().position.x, -2.13f, 2.13f), 0f, Mathf.Clamp(GetComponent<Rigidbody>().position.z, -4.45f, 4.45f));
#elif UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 position = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(Input.GetTouch(0).position);
            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(Input.GetTouch(0).deltaPosition.x, -5f, 5f) * -tilt);
            GetComponent<Rigidbody>().position = new Vector3(Mathf.Clamp(position.x, -2.13f, 2.13f), 0f, Mathf.Clamp(position.z, -4.45f, 4.45f));
        }
#endif
    }

    // 升级
    public void Upgrade()
    {
        isUpgrade = true;
    }

    // 降级
    public void Downgrade()
    {
        isUpgrade = false;
    }
}