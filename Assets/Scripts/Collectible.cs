using UnityEngine;
using System.Collections;
public class Collectible: MonoBehaviour
{
	private Vector3 rotation;
	void Start ()
	{
		rotation = new Vector3 (0, Random.Range (30, 300), 90);
	}
	void Update ()
	{
		rotation.y++;
		transform.localRotation = Quaternion.Euler (rotation);
	}
}