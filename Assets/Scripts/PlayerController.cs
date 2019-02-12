using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5;
	public float tilt = 5;
	private float moveHorizontal;
	private float moveVertical;
	private Vector3 moveVector;

	private float fireNext;
	private float fireInterval = 0.2f;
	public GameObject bullet;
	public Transform leftLauncher;
	public Transform centerLauncher;
	public Transform rightLauncher;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Time.time > fireNext) {
			fireNext = Time.time + fireInterval;
			Instantiate (bullet, leftLauncher.position, leftLauncher.rotation);
			Instantiate (bullet, centerLauncher.position, centerLauncher.rotation);
			Instantiate (bullet, rightLauncher.position, rightLauncher.rotation);
		}
	}

	void FixedUpdate () {

#if UNITY_EDITOR || UNITY_STANDALONE
		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");
#elif UNITY_IPHONE || UNITY_ANDROID
		moveHorizontal = Input.GetAxis ("Mouse X");
		moveVertical = Input.GetAxis ("Mouse Y");
#endif
		moveVector = new Vector3 (moveHorizontal, moveVertical, 0f);
		GetComponent<Rigidbody> ().velocity = moveVector * speed;
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (-90f, GetComponent<Rigidbody> ().velocity.x * -tilt, 0f);
		GetComponent<Rigidbody> ().position = new Vector3 (Mathf.Clamp (GetComponent<Rigidbody> ().position.x, -2.13f, 2.13f), Mathf.Clamp (GetComponent<Rigidbody> ().position.y, -4.45f, 4.45f), 0f);
	}
}