using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{ 

    [SerializeField] private Image score;
	[SerializeField] private Text timer;
	[SerializeField] private Image rage;

    [SerializeField]
    List<UnityEvent> openingEvent;

	private float maxScore;

	public List<GameObject> stateList;

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
		if (newRage >= 0.0f && newRage < 50.0f)
		{
			this.stateList[0].SetActive(true);
			this.stateList[1].SetActive(false);
			this.stateList[2].SetActive(false);
		}
		else if (newRage >= 50.0f && newRage < 160.0f)
		{
			this.stateList[0].SetActive(false);
			this.stateList[1].SetActive(true);
			this.stateList[2].SetActive(false);
		}
		else if (newRage >= 280.0f)
		{
			this.stateList[0].SetActive(false);
			this.stateList[1].SetActive(false);
			this.stateList[2].SetActive(true);
		}
	}

	public void UpdateTimer(float newTimer)
	{
		string minutes = Mathf.Floor(newTimer / 60).ToString("00");
		string seconds = (newTimer % 60.0f).ToString("00");

		this.timer.text = minutes + ":" + seconds;
		//this.timer.fillAmount -= 1.0f / newTimer * Time.deltaTime;
        //if (timer.fillAmount < 50 && openingEvent.Count != 0)
        //{
        //    foreach (var item in openingEvent)
        //    {
        //        item.Invoke();
        //    }
        //    openingEvent.Clear();
        //}
    }
}
