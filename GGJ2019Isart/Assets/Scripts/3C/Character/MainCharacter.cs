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
			StartCoroutine(LaunchHideCoroutine());
		}
		else
		{
			this.GetComponent<Renderer>().enabled = true;
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Nun"), false);
			this.isHidden = false;
		}
	}

	private IEnumerator LaunchHideCoroutine()
	{
		Animator animator = this.GetComponent<Animator>();
		animator.SetBool("IsHidding", true);
		while (animator.GetCurrentAnimatorStateInfo(0).IsName("HidingState") == false)
		{
			yield return null;
		}
		while (animator.GetCurrentAnimatorStateInfo(0).IsName("HidingState") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
		{
			//Wait every frame until animation has finished
			yield return null;
		}
		animator.SetBool("IsHidding", false);
		this.GetComponent<Renderer>().enabled = false;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Nun"));
		this.isHidden = true;
		yield return null;
	}
}
