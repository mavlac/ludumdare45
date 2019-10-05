using UnityEngine;
using UnityEngine.Events;

public class Draggable : MonoBehaviour
{
	[Space, ReadOnly]
	public bool dragged = false;

	[Space]
	public bool showAndHideMouseCursor = false;
	public bool useRigidbody = false;

	[Header("Touch - as drag within distance limit")]
	public float maxTouchDistance = 0.25f;
	public UnityEvent onTouch;

	[Space]
	public UnityEvent onDragBegin;
	public UnityEvent onDragEnd;


	Vector3 dragInitPosition;
	Vector3 CurrentPosition
	{
		get
		{
			return useRigidbody ? (Vector3)rb.position : transform.position;
		}
	}
	Vector3 desiredPosition;
	public Vector3 DesiredPosition => desiredPosition;


	Vector3 dragHandleOffset;
	
	const float posUpdSpeed = 20f;


	DragController dragController;
	
	SpriteRenderer sr;
	
	Rigidbody rb;
	bool initialRbType;

	Camera mainCamera;


	void Awake()
	{
		mainCamera = Camera.main;

		if (useRigidbody)
		{
			rb = GetComponent<Rigidbody>();
			initialRbType = rb.isKinematic;
		}
		
		SetDesiredPosToCurrent();
	}
	void Update()
	{
		float currentPositionY = transform.position.y;

		if (dragged)
		{
			Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
			Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(cursorScreenPoint);

			desiredPosition = cursorPosition + dragHandleOffset;
			desiredPosition.y = currentPositionY;
			
			if (dragController.limitDragWithinArea)
			{
				desiredPosition.x = Mathf.Clamp(desiredPosition.x, dragController.dragArea.xMin, dragController.dragArea.xMax);
				desiredPosition.y = Mathf.Clamp(desiredPosition.y, dragController.dragArea.yMin, dragController.dragArea.yMax);
			}

			// movement (not using rigidbody)
			if (!useRigidbody)
			{
				transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * posUpdSpeed);
			}
		}
	}
	private void FixedUpdate()
	{
		if (dragged && useRigidbody)
		{
			rb.position = Vector3.Lerp(rb.position, desiredPosition, Time.fixedDeltaTime * posUpdSpeed);
		}
	}
	private void OnDisable()
	{
		EndDrag();
	}



	public void InitiateDrag(DragController dragController)
	{
		if (!this.enabled) return; // no dragging with deactivated Component
		
		this.dragController = dragController;
		
		dragged = true;
		if (showAndHideMouseCursor) Cursor.visible = false;
		//Debug.Log(gameObject.name + " InitiateDrag()");
		
		Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
		Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(cursorScreenPoint);
		
		if (useRigidbody)
		{
			dragInitPosition = rb.position;
			
			rb.isKinematic = true;
			rb.velocity = Vector3.zero;
		}
		else
		{
			dragInitPosition = transform.position;
		}
		
		dragHandleOffset = dragInitPosition - cursorPosition;
		
		onDragBegin.Invoke();
	}
	public void EndDrag(bool checkDistanceForTouch = false)
	{
		dragged = false;
		if (showAndHideMouseCursor) Cursor.visible = true;
		//Debug.Log(gameObject.name + " EndDrag()");
		
		if (useRigidbody)
		{
			rb.isKinematic = initialRbType;
		}

		if (checkDistanceForTouch && Vector3.Distance(dragInitPosition, CurrentPosition) < maxTouchDistance)
		{
			onTouch.Invoke();
		}
		
		onDragEnd.Invoke();
	}
	public void EndDragAndDisableSelf()
	{
		EndDrag();
		this.enabled = false;
	}



	public void SetDesiredPosToCurrent()
	{
		desiredPosition = CurrentPosition;
	}
}