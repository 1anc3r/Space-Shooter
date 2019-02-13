using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject expolsion;
    public GameObject bullet;
    public Transform leftLauncher;
    public Transform centerLauncher;
    public Transform rightLauncher;

    private float fireNext = 0f;
    private float fireInterval = 0.25f;
    private float speed = 5;
    private float tilt = 5;
    private float moveHorizontal;
    private float moveVertical;
    private Vector3 moveVector;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > fireNext)
        {
            fireNext = Time.time + fireInterval;
            // Instantiate (bullet, leftLauncher.position, leftLauncher.rotation);
            Instantiate(bullet, centerLauncher.position, centerLauncher.rotation);
            // Instantiate (bullet, rightLauncher.position, rightLauncher.rotation);
            GetComponent<AudioSource>().Play();
        }
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        moveVector = new Vector3(moveHorizontal, 0f, moveVertical);
        GetComponent<Rigidbody>().velocity = moveVector * speed;
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0f, 0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        GetComponent<Rigidbody>().position = new Vector3(Mathf.Clamp(GetComponent<Rigidbody>().position.x, -2.13f, 2.13f), 0f, Mathf.Clamp(GetComponent<Rigidbody>().position.z, -4.45f, 4.45f));
#elif UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 position = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.GetTouch(0).position);
            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0f, 0f, Input.GetTouch(0).deltaPosition.x * -tilt);
            GetComponent<Rigidbody>().position = new Vector3(Mathf.Clamp(position.x, -2.13f, 2.13f), 0f, Mathf.Clamp(position.z, -4.45f, 4.45f));
        }
#endif
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid" || other.tag == "Enemy")
        {
            Instantiate(expolsion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}