using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{ 

    [SerializeField] private Image score;
	[SerializeField] private Image timer;
	[SerializeField] private Image rage;

    [SerializeField]
    List<UnityEvent> openingEvent;

	private float maxScore;

	public void Init(float maxScore)
	{
		this.maxScore = maxScore;
		this.UpdateRage(0);
		this.UpdateScore(0);
	}

	public void UpdateScore(float newScore)
	{
		this.score.fillAmount = newScore / this.maxScore; 
	}

	public void UpdateRage(float newRage)
	{
		this.rage.fillAmount = newRage / 100.0f;
	}

	public void UpdateTimer(float newTimer)
	{
		//string minutes = Mathf.Floor(newTimer / 60).ToString("00");
		//string seconds = (newTimer % 60).ToString("00");

		this.timer.fillAmount -= 1.0f / newTimer * Time.deltaTime;
        if (timer.fillAmount < 50 && openingEvent.Count != 0)
        {
            foreach (var item in openingEvent)
            {
                item.Invoke();
            }
            openingEvent.Clear();
        }
    }
}
