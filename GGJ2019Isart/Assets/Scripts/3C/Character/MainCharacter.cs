using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
	[SerializeField]
	private float speed = 6.0f;
	[SerializeField]
	private float speedMultiplier = 1.5f;
	[SerializeField]
	private GameObject freePlayerUsableGO;

	private bool isCaptured = false;
	public bool IsCaptured
	{
		get
		{
			return this.isCaptured;
		}
		set
		{
			this.isCaptured = value;
			if (this.isCaptured == true)
			{
				this.freePlayerUsableGO.SetActive(true);
			}
			else
			{
				this.freePlayerUsableGO.SetActive(false);
			}
			Debug.Log("Is captured? " + this.isCaptured.ToString());
		}
	}

	public float GetSpeed()
	{
		return this.speed;
	}

	public float GetSpeedMultiplier()
	{
		return this.speedMultiplier;
	}
}
