using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	[SerializeField] private Image score;
	[SerializeField] private Text timer;
	[SerializeField] private Image rage;

	private int maxScore;

	public void Init(int maxScore)
	{
		this.maxScore = maxScore;
		this.UpdateRage(0);
		this.UpdateScore(0);
	}

	public void UpdateScore(int newScore)
	{
		this.score.fillAmount = newScore / this.maxScore; 
	}

	public void UpdateRage(int newRage)
	{
		this.rage.fillAmount = newRage / 100.0f;
	}

	public void UpdateTimer(float newTimer)
	{
		string minutes = Mathf.Floor(newTimer / 60).ToString("00");
		string seconds = (newTimer % 60).ToString("00");

		this.timer.text = minutes + ":" + seconds;
	}
}
