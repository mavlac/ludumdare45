using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public Color carpetColor = Color.white;
	public Transform centerPos;
	public GameObject completeMark;

	FloorTile[] tiles;
	
	
	
	void Awake()
	{
		tiles = GetComponentsInChildren<FloorTile>();
	}


	public bool CheckRoomForCompletition()
	{
		bool completed = true;
		foreach (FloorTile ft in tiles)
			if (!ft.Occupied) { completed = false; break; }
		
		if (completed) VisualiseRoomCompletition();
		
		return completed;
	}
	void VisualiseRoomCompletition()
	{
		completeMark.SetActive(true);
	}
}