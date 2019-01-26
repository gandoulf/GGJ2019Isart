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

	private void Start()
	{
		if (this.defaultButton != null)
		{
			EventSystem.current.SetSelectedGameObject(this.defaultButton);
		}
	}

	private void Update()
	{
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
		this.main.SetActive(false);
		this.lobby.SetActive(true);
	}
}