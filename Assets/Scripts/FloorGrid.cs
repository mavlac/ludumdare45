using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
	const float tileSize = 1f;

	public Vector2Int size;
	public Vector2Int offset;
	public GameObject tilePrefab;

	List<FloorTile> floorTiles;
	
	
	void Awake()
	{
		
	}
	
	void Start()
	{
		Generate();
	}



	void Generate()
	{
		floorTiles = new List<FloorTile>();

		Vector2 localPosOffset = new Vector2(0f - size.x / 2f, 0f - size.y / 2f);
		Vector2Int shift = Vector2Int.zero;

		for (int y = 0; y < size.y; y++)
		{
			shift.y = 0;
			for (int x = 0; x < size.x; x++)
			{
				GameObject go = Instantiate(tilePrefab, transform) as GameObject;
				go.transform.localPosition =
					new Vector3(
						localPosOffset.x + (offset.x + x + shift.x) * tileSize,
						0,
						localPosOffset.y + (offset.y + y + shift.y) * tileSize);
				
				floorTiles.Add(go.GetComponent<FloorTile>());
				
				shift.y--;
			}
		}
	}
}