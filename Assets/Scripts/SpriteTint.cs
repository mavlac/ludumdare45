using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTint : MonoBehaviour
{
	public bool useDefinedStartColor = false;
	public Color startColor = Color.white;
	public Color targetColor = Color.white;

	[Space]
	public bool playOnAwake;
	public bool disableSpriteOnAwake;
	public AnimationCurve colorFadeProgression = AnimationCurve.EaseInOut(0, 0, 1, 1);
	public float animationSpeed = 1f;

	[Space]
	public UnityEvent onColorFadeFinished;

	bool running = false;
	float t = 0f;

	SpriteRenderer sr;


	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();

		if (playOnAwake) TriggerAnimation();
		else if (disableSpriteOnAwake) sr.enabled = false;
	}

	void Update()
	{
		if (!running) return;

		t = Mathf.Clamp01(t + Time.deltaTime * animationSpeed);


		sr.color = Color.Lerp(startColor, targetColor, colorFadeProgression.Evaluate(t));

		if (t == 1f)
		{
			onColorFadeFinished.Invoke();
			this.enabled = false;
			running = false;
			return;
		}
	}



	public void TriggerAnimation()
	{
		if (useDefinedStartColor)
		{
			sr.color = startColor;
		}
		else
		{
			startColor = sr.color;
		}

		sr.enabled = true;

		t = 0f;
		running = true;
	}
}