using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		print(2222);
		//Vector2 v = new Vector2(-1 * Mathf.Cos(transform.rotation.eulerAngles.z), 1 * Mathf.Sin(transform.rotation.eulerAngles.z)).normalized;
		Vector2 v = Vector2.left;
		Debug.DrawRay(gameObject.transform.position, v);
		collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.TransformVector(v * 20f));
	}
}