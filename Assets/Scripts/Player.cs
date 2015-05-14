using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Player: MonoBehaviour
{
	public AudioSource rollSource, knockSource, pickupSource;
	public Text capsuleCountText;
	new private Rigidbody rigidbody;
	new private SphereCollider collider;
	private int capsuleCount = 0;
	private int cameraResetTime = 30;
	private Vector3 cameraBegin;
	private float oldMagnitude;
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody> ();
		collider = GetComponent<SphereCollider> ();
		setCapsuleCountText ();
	}
	void Update ()
	{
		if (Input.GetKey (KeyCode.Escape)) Application.Quit ();
		bool onGround = isGrounded ();
		if (onGround && rigidbody.velocity.magnitude > 0.00001f && !rollSource.isPlaying)
		{
			rollSource.Play ();
		}
		else if (!onGround || rigidbody.velocity.magnitude <= 0.00001f)
		{
			rollSource.Pause ();
		}
		rollSource.pitch = rigidbody.velocity.magnitude * 0.2f;
		if (cameraResetTime == 30) Camera.main.transform.localPosition = transform.localPosition + Vector3.up * 2 + Vector3.back * 5;
		else
		{
			float x = camTween (cameraResetTime, cameraBegin.x, -cameraBegin.x, 30);
			float y = camTween (cameraResetTime, cameraBegin.y, 4 - cameraBegin.y, 30);
			float z = camTween (cameraResetTime++, cameraBegin.z, -5 - cameraBegin.z, 30);
			Camera.main.transform.localPosition = new Vector3 (x, y, z);
		}
		if (rigidbody.velocity.magnitude - oldMagnitude < -0.5f)
		{
			knockSource.volume = rigidbody.velocity.magnitude * 0.5f;
			knockSource.Play ();
		}
	}
	void FixedUpdate ()
	{
		oldMagnitude = rigidbody.velocity.magnitude;
		Vector3 impulse = Vector3.zero;
		impulse.x = Input.GetAxis ("Horizontal");
		impulse.z = Input.GetAxis ("Vertical");
		impulse *= 2;
		rigidbody.AddForce (impulse);
	}
	void LateUpdate ()
	{
		if (rigidbody.velocity.y < -10 && transform.localPosition.y < -4)
		{
			GameObject capsules = GameObject.Find ("Collectibles");
			foreach (Transform t in capsules.transform)
			{
				t.gameObject.SetActive (true);
			}
			capsuleCount = 0;
			setCapsuleCountText ();
			cameraBegin = Camera.main.transform.localPosition;
			transform.localPosition = new Vector3 (0, 2, 0);
			rigidbody.velocity = Vector3.zero;
			cameraResetTime = 0;
		}
	}
	void OnTriggerEnter (Collider other)
	{
		pickupSource.Play ();
		other.gameObject.SetActive (false);
		capsuleCount++;
		setCapsuleCountText ();
	}
	void setCapsuleCountText ()
	{
		capsuleCountText.text = "Capsules: " + capsuleCount + " / 127";
	}
	bool isGrounded ()
	{
		return Physics.Raycast (transform.position, -Vector3.up, collider.bounds.extents.y + 0.1f);
	}
	float camTween (int t, float b, float c, int d)
	{
		return c * t / d + b;
	}
}