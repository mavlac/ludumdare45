using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour
{
	public bool active = true;

	[Header("Boundaries")]
	public bool limitDragWithinArea = false;
	[ReadOnly("limitDragWithinArea")]
	public Rect dragArea = new Rect(-8f, -4.5f, 16f, 9f);

	[Space]
	[PhysicsLayer] public int layer;
	public float rayDistance = Mathf.Infinity;
	
	[Header("Sounds")]
	public AudioSource audioSrc;
	public AudioClip dragBeginSound;
	public AudioClip dragEndSound;
	
	[HideInInspector]
	public Draggable draggedObject;
	
	public readonly Color dragAreaGizmoColor = Color.magenta;

	Camera mainCamera;


	void Awake()
	{
		mainCamera = Camera.main;
	}
		private void Update()
	{
		if (active && Input.GetMouseButtonDown(0))
		{
			int layerMask = 1 << layer;
			
			// tap ignored when over GUI element
			//if (EventSystem.current.IsPointerOverGameObject()) return;


			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
			{
				//Debug.Log("DragController hit: " + hit.transform.gameObject.name);
				
				if (draggedObject = hit.transform.gameObject.GetComponent<Draggable>())
				{
					draggedObject.InitiateDrag(this);
					
					PlayAudio(dragBeginSound);
				}
			}
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			if (draggedObject)
			{
				draggedObject.EndDrag(true);
				
				PlayAudio(dragEndSound);
				draggedObject = null;
			}
		}
	}
	private void OnDrawGizmosSelected()
	{
		if (limitDragWithinArea)
		{
			Gizmos.color = dragAreaGizmoColor;
			// horiz
			Gizmos.DrawLine(new Vector3(dragArea.xMin, dragArea.yMin), new Vector3(dragArea.xMax, dragArea.yMin));
			Gizmos.DrawLine(new Vector3(dragArea.xMin, dragArea.yMax), new Vector3(dragArea.xMax, dragArea.yMax));
			// vert
			Gizmos.DrawLine(new Vector3(dragArea.xMin, dragArea.yMin), new Vector3(dragArea.xMin, dragArea.yMax));
			Gizmos.DrawLine(new Vector3(dragArea.xMax, dragArea.yMin), new Vector3(dragArea.xMax, dragArea.yMax));
		}
	}





	void PlayAudio(AudioClip clip)
	{
		if (!audioSrc) return;
		
		audioSrc.clip = clip;
		audioSrc.Play();
	}




	public void DelayedActivation(float time)
	{
		StartCoroutine(DelayedActivationCoroutine(time));
	}
	IEnumerator DelayedActivationCoroutine(float time)
	{
		yield return new WaitForSeconds(time);
		active = true;
	}
}