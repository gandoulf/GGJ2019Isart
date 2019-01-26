using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	private const float speed = 2.0f;
	private Material mat;

	private void Start()
	{
		this.mat = this.gameObject.GetComponent<Renderer>().material;
	}

	public void ChangeAlpha(bool hiding)
	{
		this.StopAllCoroutines();
		this.StartCoroutine(this.UpdateAlpha(hiding));
	}

	private IEnumerator UpdateAlpha(bool hiding)
	{
		float targetAlpha = hiding ? 0.8f : 1.0f;
		float currentAlpha = this.mat.GetFloat("_Alpha");

		if (hiding)
		{
			while (currentAlpha > targetAlpha)
			{
				currentAlpha -= Time.deltaTime * speed;
				this.mat.SetFloat("_Alpha", currentAlpha);
				if (currentAlpha <= targetAlpha)
				{
					this.mat.SetFloat("_Alpha", targetAlpha);
					break;
				}

				yield return null;
			}
		}
		else
		{
			while (currentAlpha < targetAlpha)
			{
				currentAlpha += Time.deltaTime * speed;
				this.mat.SetFloat("_Alpha", currentAlpha);
				if (currentAlpha >= targetAlpha)
				{
					this.mat.SetFloat("_Alpha", targetAlpha);
					break;
				}

				yield return null;
			}
		}
	}
}
