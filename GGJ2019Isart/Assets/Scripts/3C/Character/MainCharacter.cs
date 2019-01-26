using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
	[SerializeField]
	private float speed = 6.0f;
	[SerializeField]
	private float speedMultiplier = 1.5f;

	public bool IsCaptured { get; set; }

	public float GetSpeed()
	{
		return this.speed;
	}

	public float GetSpeedMultiplier()
	{
		return this.speedMultiplier;
	}

	//private void OnObjectNearEnter(Activable objScript)
	//{
	//}

	//private void OnObjectNearExit(Activable objScript)
	//{
	//}
}
