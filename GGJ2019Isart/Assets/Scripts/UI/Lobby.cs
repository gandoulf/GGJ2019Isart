using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
	[SerializeField] private List<GameObject> slotList;
	[SerializeField] private List<GameObject> textList;
	[SerializeField] private string sceneToLaunch;

	private void Start()
	{
		GameManagerSingleton.Instance.indexSlotDictionnary = new Dictionary<int, int>();
		int nbJoy = Input.GetJoystickNames().Length;

		for (int i = 0; i < nbJoy; i++)
		{
			Debug.Log(Input.GetJoystickNames()[i]);
		}
	}

	private void Update()
	{
		int slot = this.GetFreeSlot();

		if (Input.GetButtonDown("Joy1Action"))
		{
			if (GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(1) == false)
			{
				this.Join("Joy1Action", slot);
			}
			else
			{
				this.Ready(1);
			}
		}
		else if (Input.GetButtonDown("Joy2Action"))
		{
			if (GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(2) == false)
			{
				this.Join("Joy2Action", slot);
			}
			else
			{
				this.Ready(2);
			}
		}
		else if (Input.GetButtonDown("Joy3Action") && GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(3) == false)
		{
			if (GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(3) == false)
			{
				this.Join("Joy3Action", slot);
			}
			else
			{
				this.Ready(3);
			}
		}
		else if (Input.GetButtonDown("Joy4Action") && GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(4) == false)
		{
			if (GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(4) == false)
			{
				this.Join("Joy4Action", slot);
			}
			else
			{
				this.Ready(4);
			}
		}
		if (Input.GetButtonDown("Joy1Special") && GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(1) &&
			this.slotList[GameManagerSingleton.Instance.indexSlotDictionnary[1]].GetComponent<Image>().color != Color.green)
		{
			this.Leave("Joy1Special");
		}
		else if (Input.GetButtonDown("Joy2Special") && GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(2) &&
			this.slotList[GameManagerSingleton.Instance.indexSlotDictionnary[2]].GetComponent<Image>().color != Color.green)
		{
			this.Leave("Joy2Special");
		}
		else if (Input.GetButtonDown("Joy3Special") && GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(3) &&
			this.slotList[GameManagerSingleton.Instance.indexSlotDictionnary[3]].GetComponent<Image>().color != Color.green)
		{
			this.Leave("Joy3Special");
		}
		else if (Input.GetButtonDown("Joy4Special") && GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(4) &&
			this.slotList[GameManagerSingleton.Instance.indexSlotDictionnary[4]].GetComponent<Image>().color != Color.green)
		{
			this.Leave("Joy4Special");
		}
		if (Input.GetButtonDown("Joy1Start") && this.CheckCanLaunch())
		{
			this.LaunchGame();
		}
	}

	private void Join(string author, int slot)
	{
		int index = -1;

		this.transform.parent.GetComponent<NavigationMenu>().click.Play();
		switch (author)
		{
			case "Joy1Action":
				index = 1;
				break;
			case "Joy2Action":
				index = 2;
				break;
			case "Joy3Action":
				index = 3;
				break;
			case "Joy4Action":
				index = 4;
				break;
		}
		GameManagerSingleton.Instance.indexSlotDictionnary.Add(index, slot);
		this.textList[slot].SetActive(false);
		this.slotList[slot].SetActive(true);
	}

	private void Leave(string author)
	{
		int index = -1;
		int slot = -1;

		switch (author)
		{
			case "Joy1Special":
				index = 1;
				break;
			case "Joy2Special":
				index = 2;
				break;
			case "Joy3Special":
				index = 3;
				break;
			case "Joy4Special":
				index = 4;
				break;
		}
		slot = GameManagerSingleton.Instance.indexSlotDictionnary[index];
		GameManagerSingleton.Instance.indexSlotDictionnary.Remove(index);
		this.textList[slot].SetActive(true);
		this.slotList[slot].SetActive(false);
	}

	private void Ready(int index)
	{
		GameObject slot = this.slotList[GameManagerSingleton.Instance.indexSlotDictionnary[index]];

		if (slot.GetComponent<Image>().color != Color.green)
		{
			this.transform.parent.GetComponent<NavigationMenu>().click.Play();
			slot.GetComponent<Image>().color = Color.green;
		}
		/*if (index == 1)
		{
			slot.GetComponentInChildren<Text>().text = "Press Start to start";
		}
		else
		{
			slot.GetComponentInChildren<Text>().text = "";
		}*/
	}

	private int GetFreeSlot()
	{
		int nbSlot = this.slotList.Count;

		for (int i = 0; i < nbSlot; i++)
		{
			if (!this.slotList[i].activeSelf)
			{
				return i;
			}
		}

		return -1;
	}

	private bool CheckCanLaunch()
	{
		int nbSlot = this.slotList.Count;

		for (int i = 0; i < nbSlot; i++)
		{
			if (GameManagerSingleton.Instance.indexSlotDictionnary.ContainsKey(i + 1) && this.slotList[i].GetComponent<Image>().color != Color.green)
			{
				return false;
			}
		}

		return true;
	}

	private void LaunchGame()
	{
		this.transform.parent.GetComponent<NavigationMenu>().click.Play();
		SceneManager.LoadScene(this.sceneToLaunch);
	}
}
