using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSingleton : Singleton<GameManagerSingleton>
{
    // (Optional) Prevent non-singleton constructor use.
    protected GameManagerSingleton() { }

    // Then add whatever code to the class you need as you normally would.
    public APathNode[] pathNode;

	public Dictionary<int, int> indexSlotDictionnary;
	public GameObject playerPrefab;

	private void Start()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		DontDestroyOnLoad(this.gameObject);
	}

	public void SpawnPlayer()
	{
		int nbPlayer = this.indexSlotDictionnary.Count + 1;

		Debug.Log("NB PLAYERS: " + (nbPlayer -1));
		for (int i = 1; i < nbPlayer; i++)
		{
			MainController controller = Instantiate(playerPrefab).GetComponent<MainController>();

			Debug.Log("SPAWN PLAYER: " + i + " FROM SLOT: " + this.indexSlotDictionnary[i]);
			controller.Init(i, this.indexSlotDictionnary[i]);
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.SpawnPlayer();
	}
}