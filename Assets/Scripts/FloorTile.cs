using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
	SpriteRenderer sr;
	
	
	
	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter(Collider other)
	{
		sr.enabled = true;
	}
	private void OnTriggerStay(Collider other)
	{
		sr.enabled = true;
	}
	private void OnTriggerExit(Collider other)
	{
		sr.enabled = false;
	}
}