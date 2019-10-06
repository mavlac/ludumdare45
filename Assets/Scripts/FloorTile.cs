using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
	public SpriteRenderer carpetSr;

	public bool Occupied { get; private set; }

	SpriteRenderer sr;

	Room parentRoom;

	
	
	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		parentRoom = transform.parent.parent.GetComponent<Room>();
		
		UpdateCarpetColor();
	}

	private void OnTriggerEnter(Collider other)
	{
		sr.enabled = true;
		Occupied = true;
	}
	private void OnTriggerStay(Collider other)
	{
		sr.enabled = true;
		Occupied = true;
	}
	private void OnTriggerExit(Collider other)
	{
		sr.enabled = false;
		Occupied = false;
	}



	public void UpdateCarpetColor()
	{
		if (parentRoom)
		{
			carpetSr.color = parentRoom.carpetColor;
		}
		else
		{
			//Debug.LogError("ParentRoom not found in hieararchy");
		}
	}
}