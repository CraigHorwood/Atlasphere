using UnityEngine;
using System.Collections;
public class FloatingPlatform: MonoBehaviour
{
	public float xRange = 0, yRange = 0, zRange = 5;
	private Vector3 origin;
	private Player player;
	void Start ()
	{
		origin = transform.localPosition;
	}
	void Update ()
	{
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		float z = transform.localPosition.z;
		float sin = Mathf.Sin (Time.time);
		if (xRange != 0) x = origin.x + sin * xRange;
		if (yRange != 0) y = origin.y + sin * yRange;
		if (zRange != 0) z = origin.z + sin * zRange;
		Vector3 oldPosition = transform.localPosition;
		transform.localPosition = new Vector3 (x, y, z);
		Vector3 lastVelocity = transform.localPosition - oldPosition;
		if (player != null) player.transform.localPosition += lastVelocity;
	}
	void OnCollisionEnter (Collision collision)
	{
		player = collision.gameObject.GetComponent<Player> ();
	}
	void OnCollisionExit ()
	{
		player = null;
	}
}