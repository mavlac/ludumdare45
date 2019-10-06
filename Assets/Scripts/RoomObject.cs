using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
	[ReadOnly]
	public bool invalidDraggedPos = false;

	[Space, ReadOnly]
	public bool pivotOnHalfX;
	[ReadOnly]
	public bool pivotOnHalfZ;

	[Space]
	public AudioClip onMouseDownSound;

	const float defaultElevation = 0f;
	const float pickupElevation = 0f;

	Vector3 dragPickupPos;
	Quaternion turnAttemptRot;

	bool parking = false;
	bool turning = false;
	const float parkingSpeed = 10f;
	const float rotationSpeed = 5f;



	private void Start()
	{
		if (Mathf.Abs(transform.position.x - Mathf.RoundToInt(transform.position.x)) > 0.3f) pivotOnHalfX = true;
		if (Mathf.Abs(transform.position.z - Mathf.RoundToInt(transform.position.z)) > 0.3f) pivotOnHalfZ = true;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(gameObject.tag))
			invalidDraggedPos = true;
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag(gameObject.tag))
			invalidDraggedPos = true;
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(gameObject.tag))
			invalidDraggedPos = false;
	}
	private void OnMouseDown()
	{
		if (onMouseDownSound)
			LD45.Game.instance.PlayAudio(onMouseDownSound);
	}



	public void Pickup()
	{
		if (parking) return;
		if (turning) return;
		
		dragPickupPos = transform.position;
		turnAttemptRot = transform.rotation;
		invalidDraggedPos = false;

		transform.Translate(Vector3.up * pickupElevation, Space.World);
	}
	public void ParkOnGrid()
	{
		// reset pickup elevation
		transform.position = new Vector3(transform.position.x, defaultElevation, transform.position.z);
		
		// dragged to wrong position - park back, rotate back too
		if (invalidDraggedPos)
		{
			if (this.gameObject.activeInHierarchy)
			{
				StartCoroutine(ParkCoroutine(dragPickupPos));

				// if there was some unsuccesfull turning, reset back
				if (transform.rotation != turnAttemptRot)
				{
					// if one of size is even, switch while turning
					if ((pivotOnHalfX && !pivotOnHalfZ) || (!pivotOnHalfX && pivotOnHalfZ))
					{
						pivotOnHalfX = !pivotOnHalfX;
						pivotOnHalfZ = !pivotOnHalfZ;
					}
					StartCoroutine(TurnCoroutine(turnAttemptRot));
				}
			}
			return;
		}
		
		
		Vector3 parkedPosition = transform.position;
		if (!pivotOnHalfX)
			parkedPosition.x = Mathf.RoundToInt(parkedPosition.x);
		else
		{
			parkedPosition.x = Mathf.RoundToInt(parkedPosition.x + 0.5f);
			parkedPosition.x -= 0.5f;
		}
		if (!pivotOnHalfZ)
			parkedPosition.z = Mathf.RoundToInt(parkedPosition.z);
		else
		{
			parkedPosition.z = Mathf.RoundToInt(parkedPosition.z + 0.5f);
			parkedPosition.z -= 0.5f;
		}

		if (this.gameObject.activeInHierarchy)
			StartCoroutine(ParkCoroutine(parkedPosition));
	}
	IEnumerator ParkCoroutine(Vector3 targetPosition)
	{
		parking = true;
		Vector3 initialPosition = transform.position;
		
		for (float t = 0f; t <= 1f; t += Time.deltaTime * parkingSpeed)
		{
			transform.position = Vector3.Lerp(initialPosition, targetPosition, Mathf.SmoothStep(0, 1, t));
			yield return null;
		}
		transform.position = targetPosition;


		LD45.Game.instance.CheckCurrentRoomCompletition();
		parking = false;
	}

	public void Turn()
	{
		if (parking) return;
		if (turning) return;

		turnAttemptRot = transform.rotation;

		// if one of size is even, switch while turning
		if ((pivotOnHalfX && !pivotOnHalfZ) || (!pivotOnHalfX && pivotOnHalfZ))
		{
			pivotOnHalfX = !pivotOnHalfX;
			pivotOnHalfZ = !pivotOnHalfZ;
		}

		StartCoroutine(TurnCoroutine(transform.rotation * Quaternion.Euler(0, 90, 0)));
	}
	IEnumerator TurnCoroutine(Quaternion targetRotation)
	{
		turning = true;
		Quaternion initialRotation = transform.rotation;

		for (float t = 0f; t <= 1f; t += Time.deltaTime * rotationSpeed)
		{
			transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, Mathf.SmoothStep(0, 1, t));
			yield return null;
		}
		transform.rotation = targetRotation;

		turning = false;
		// also try to park when rotation done
		ParkOnGrid();
	}
}