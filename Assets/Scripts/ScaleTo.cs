using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleTo : MonoBehaviour
{
	[Header("Start from specified or default scale")]
	public bool translateStartingFrom;
	[ReadOnly("translateStartingFrom")]
	public Vector3 initialLocalScale;

	[Space]
	public Vector3 finalScale = Vector3.one * 1.5f;

	[Space]
	public float speed = 5f;
	public AnimationCurve progression = AnimationCurve.EaseInOut(0, 0, 1f, 1f);

	[Space]
	public UnityEvent onScaleChangeFinished;



	float t = 0f;

	void Awake()
	{
		if (!this.enabled) return; // avoid call of Awake on disabled component - TODO include some playOnAwake flag and triggering
		
		if (!translateStartingFrom)
		{
			initialLocalScale = transform.localScale;
		}
		else
		{
			transform.localScale = initialLocalScale;
		}
	}
	
	void Update()
	{
		t = Mathf.Clamp01(t + Time.deltaTime * speed);
		
		transform.localScale =
			Vector3.Lerp(initialLocalScale, finalScale, progression.Evaluate(t));
		
		
		if (t == 1f)
		{
			onScaleChangeFinished.Invoke();
			this.enabled = false;
			return;
		}
	}
}