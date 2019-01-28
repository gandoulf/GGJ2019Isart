using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationMenu : MonoBehaviour
{
	private Button previousButton;
	[SerializeField] private float scaleAmount = 1.4f;
	[SerializeField] private GameObject defaultButton;
	[SerializeField] private GameObject main;
	[SerializeField] private GameObject lobby;
	public StudioEventEmitter click;
	[SerializeField] private StudioEventEmitter switchUI;

	private float timer;

	private void Start()
	{
		if (this.defaultButton != null)
		{
			EventSystem.current.SetSelectedGameObject(this.defaultButton);
		}
	}

	private void Update()
	{
		if (this.timer == 0.0f)
		{
			if (Input.GetAxis("Joy1Vertical") > 0.8f)
			{
				EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnDown().gameObject);
				this.timer = 0.2f;
			}
			else if (Input.GetAxis("Joy1Vertical") < -0.8f)
			{
				EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnUp().gameObject);
				this.timer = 0.2f;
			}
		}
		else
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0.0f)
			{
				this.timer = 0.0f;
			}
		}
		GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
		Button selectedAsButton = null;

		if (selectedObj == null)
		{
			return;
		}
		selectedAsButton = selectedObj.GetComponent<Button>();
		if (selectedAsButton != null && selectedAsButton != this.previousButton)
		{
			this.HighlightButton(selectedAsButton);
		}
		if (this.previousButton != null && this.previousButton != selectedAsButton)
		{
			this.UnHighlightButton(previousButton);
		}
		previousButton = selectedAsButton;
		if (Input.GetButtonDown("Joy1Special") && this.lobby.activeSelf && GameManagerSingleton.Instance.indexSlotDictionnary.Count == 0)
		{
			this.BackToMainMenu();
		}
	}

	private void OnDisable()
	{
		if (previousButton != null)
		{
			this.UnHighlightButton(previousButton);
		}
	}

	private void HighlightButton(Button button)
	{
		this.switchUI.Play();
		button.transform.localScale = new Vector3(this.scaleAmount, this.scaleAmount, this.scaleAmount);
		button.Select();
		button.OnSelect(null);
	}

	private void UnHighlightButton(Button button)
	{
		button.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	private void BackToMainMenu()
	{
		this.lobby.SetActive(false);
		this.main.SetActive(true);
		EventSystem.current.SetSelectedGameObject(this.defaultButton);
		this.HighlightButton(this.defaultButton.GetComponent<Button>());
	}

	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void OnStart()
	{
		this.click.Play();
		this.main.SetActive(false);
		this.lobby.SetActive(true);
	}
}