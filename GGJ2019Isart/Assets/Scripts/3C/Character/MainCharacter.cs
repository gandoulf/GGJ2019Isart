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

	public bool isHidden { get; set; } = false;

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
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Nun"));
				this.freePlayerUsableGO.SetActive(true);
			}
			else
			{
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), false);
				this.freePlayerUsableGO.SetActive(false);
			}
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

	public void HideCharacter(HideOutObj HideOutScript, bool hide)
	{
		if (hide == true)
		{
			this.GetComponent<Renderer>().enabled = false;
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Nun"));
			this.isHidden = true;
		}
		else
		{
			this.GetComponent<Renderer>().enabled = true;
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Nun"), false);
			this.isHidden = false;
		}
	}
}
