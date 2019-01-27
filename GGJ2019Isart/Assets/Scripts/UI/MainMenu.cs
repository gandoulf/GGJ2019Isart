using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Image title;
	public Image start;
	public Image credits;
	public Image quit;

	private void Start()
	{
		this.StartCoroutine(this.FadeTitle());
	}

	private IEnumerator FadeTitle()
	{
		yield return new WaitForSeconds(2.0f);

		float currentAlpha = 1.0f;

		while (currentAlpha >= 0.0f)
		{
			currentAlpha -= Time.deltaTime;
			this.title.color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);

			yield return null;
		}
		currentAlpha = 0.0f;
		while (currentAlpha < 1.0f)
		{
			currentAlpha += Time.deltaTime;
			this.start.color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);
			this.credits.color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);
			this.quit.color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);

			yield return null;
		}
	}
}
