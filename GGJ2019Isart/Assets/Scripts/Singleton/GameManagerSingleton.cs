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

    [SerializeField]
    private Material outline;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color[] playerColors;
    public List<Material> PlayerOutlineColor = new List<Material>();

    public GameObject[] randomThroablePrefab;
    public GameObject RandomThroablePrefab { get { return (randomThroablePrefab[Random.Range(0, randomThroablePrefab.Length)]); } }

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
            /*Material tmp = new Material(outline);
            tmp.SetColor("_EmissionColor", playerColors[i]);
            PlayerOutlineColor.Add(tmp);*/
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.SpawnPlayer();
	}
}