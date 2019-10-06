using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
	public float waitBeforeMove = 0.5f;
	public AudioClip moveSound;
	public float moveSpeed = 2f;
	public AnimationCurve moveProgression;

	public void MoveToRoom(int roomIndex)
	{
		Vector3 currentPos = transform.position;

		Vector3 currentRoomPos = LD45.Game.instance.roomConfig[roomIndex - 1].centerPos.position;
		Vector3 targetRoomPos = LD45.Game.instance.roomConfig[roomIndex].centerPos.position;
		Vector3 moveVector = targetRoomPos - currentRoomPos;
		
		StartCoroutine(MoveToRoomCoroutine(currentPos, currentPos + moveVector));
	}
	IEnumerator MoveToRoomCoroutine(Vector3 currentPos, Vector3 targetPos)
	{
		yield return new WaitForSeconds(waitBeforeMove);
		LD45.Game.instance.PlayAudio(moveSound);
		
		for (float t = 0f; t <= 1f; t += Time.deltaTime * moveSpeed)
		{
			transform.position = Vector3.Lerp(currentPos, targetPos, moveProgression.Evaluate(t));
			yield return null;
		}
		transform.position = targetPos;
	}
}