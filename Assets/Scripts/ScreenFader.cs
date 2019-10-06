using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(SpriteRenderer))]
public class ScreenFader : MonoBehaviour
{
	public enum FadeOutCallback
	{
		None,
		RestartScene,
		LoadScene,
		QuitApp
	}

	public Color32 defaultFadeOutColor = Color.black;
	public AnimationCurve progression = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	public float duration = 0.75f;

	public bool FadeInProgress { get; private set; } = false;
	
	const float delay = 0.1f;
	const float transparent = 0f;
	const float opaque = 1f;

	SpriteRenderer sr;
	public SpriteRenderer OverlayRenderer
	{
		get {
			return (sr ?? (sr = GetComponent<SpriteRenderer>()));
		}
	}



	private void Reset()
	{
		OverlayRenderer.enabled = false;
		OverlayRenderer.color = Color.black;
	}
	private void Awake ()
	{
		ResetFader();
	}



	public void FadeScreenOut()
	{
		FadeScreenOut(defaultFadeOutColor, FadeOutCallback.None);
	}
	public void FadeScreenOut(FadeOutCallback callback, string sceneToLoad = null)
	{
		FadeScreenOut(defaultFadeOutColor, callback, sceneToLoad);
	}
	public void FadeScreenOut(Color fadeTo, FadeOutCallback callback, string sceneToLoad = null)
	{
		ResetFader();
		OverlayRenderer.color = fadeTo;
		OverlayRenderer.enabled = true;
		
		StartCoroutine(FadeScreenOutCoroutine(callback, sceneToLoad));
	}
	IEnumerator FadeScreenOutCoroutine(FadeOutCallback callback, string sceneToLoad)
	{
		FadeInProgress = true;
		
		for (float time = 0f; time <= duration; time += Time.smoothDeltaTime)
		{
			float t = Mathf.InverseLerp(0f, duration, time);
			SetAlpha(progression.Evaluate(t));
			yield return null;
		}

		SetAlpha(opaque);
		
		FadeScreenOutCallback(callback, sceneToLoad);
		FadeInProgress = false;
	}
	void FadeScreenOutCallback(FadeOutCallback callback, string sceneToLoad)
	{
		switch(callback)
		{
			case FadeOutCallback.RestartScene:
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				break;
			
			case FadeOutCallback.LoadScene:
				if (sceneToLoad == null)
					Debug.LogError("ScreenFader callback attempts to load scene but scene name unspecified.");
				else
					SceneManager.LoadScene(sceneToLoad);
				break;
			
			case FadeOutCallback.QuitApp:
				Application.Quit(); // implement via whatever controller of your game responsible
				break;
		}
	}
	
	
	
	/// <summary>
	/// Fades screen in from white
	/// </summary>
	public void FadeScreenInFromWhite()
	{
		FadeScreenIn(Color.white);
	}
	/// <summary>
	/// Fades screen in from current fader color
	/// </summary>
	/// <param name="delayed">Waits for hardcoded 0.1 seconds before fading in</param>
	public void FadeScreenIn(bool delayed = false)
	{
		FadeScreenIn(OverlayRenderer.color, delayed);
	}
	public void FadeScreenIn(Color fadeFrom, bool delayed = false)
	{
		ResetFader();
		SetAlpha(opaque);
		OverlayRenderer.color = fadeFrom;
		OverlayRenderer.enabled = true;
		
		StartCoroutine(FadeScreenInCoroutine(delayed));
	}
	IEnumerator FadeScreenInCoroutine(bool delayed)
	{
		FadeInProgress = true;

		for (float time = 0f; time <= duration; time += Time.smoothDeltaTime)
		{
			float t = Mathf.InverseLerp(0f, duration, time);
			SetAlpha(progression.Evaluate(1 - t));
			
			if (delayed)
			{
				yield return new WaitForSecondsRealtime(delay);
				delayed = false;
			}

			yield return null;
		}
		
		SetAlpha(transparent);
		OverlayRenderer.enabled = false;
		FadeInProgress = false;
	}



	private void SetAlpha(float a)
	{
		Color c = OverlayRenderer.color;
		c.a = Mathf.Clamp(a, 0, 1);
		OverlayRenderer.color = c;
	}



	public void ResetFader()
	{
		StopAllCoroutines();
		SetAlpha(transparent);
		OverlayRenderer.enabled = false;
		FadeInProgress = false;
	}
}