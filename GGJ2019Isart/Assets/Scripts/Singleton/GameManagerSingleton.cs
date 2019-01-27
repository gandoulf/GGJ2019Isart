using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSingleton : MonoBehaviour
{
	private static GameManagerSingleton m_Instance = null;

	public static GameManagerSingleton Instance
	{
		get
		{
			return m_Instance;
		}
	}

	// Then add whatever code to the class you need as you normally would.
	public APathNode[] pathNode;

	public Dictionary<int, int> indexSlotDictionnary;
	public GameObject playerPrefab;
	public int score;
	public int rage;
	public float timer;
	public List<int> scoreNeeded;
	public List<int> rageLevel;
	public int currentRageLevel;

	private int currentScoreLevel;
	private HUD hud;
	private float currentTimer;

	[SerializeField]
    private Material outline;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color[] playerColors;
    public List<Material> PlayerOutlineColor = new List<Material>();

    public GameObject[] randomThroablePrefab;
    public GameObject RandomThroablePrefab { get { return (randomThroablePrefab[Random.Range(0, randomThroablePrefab.Length)]); } }

	private List<Door> doorList;

	private void OnEnable()
	{
		if (m_Instance == null)
		{
			m_Instance = GameObject.Find("GameManager").GetComponent<GameManagerSingleton>();
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	private void Start()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	private void Update()
	{
		if (this.hud != null)
		{
			this.currentTimer -= Time.deltaTime;
			if (this.currentTimer <= 0.0f)
			{
				this.currentTimer = 0.0f;
				this.Lose();
			}
			else
			{
				this.hud.UpdateTimer(this.timer);
			}
		}
	}

	private void Lose()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < players.Length; i++)
		{
			Destroy(players[i]);
		}
		Destroy(Camera.main.transform.parent.gameObject);
		this.score = 0;
		this.rage = 0;
		this.currentRageLevel = 0;
		this.indexSlotDictionnary.Clear();
		this.hud = null;
		SceneManager.LoadScene("Menu");
	}

	public void SpawnPlayer()
	{
		int nbPlayer = this.indexSlotDictionnary.Count + 1;
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawn");
		
		for (int i = 1; i < nbPlayer; i++)
		{
			MainController controller = Instantiate(playerPrefab).GetComponent<MainController>();

			controller.transform.position = spawners[i - 1].transform.position;
			controller.transform.rotation = spawners[i - 1].transform.rotation;
			controller.Init(i, this.indexSlotDictionnary[i]);
		}
	}

	public void IncRage(int gain)
	{
		this.rage += gain;
		this.hud.UpdateRage(this.rage);
		if ((this.currentRageLevel == 0 && this.rage >= this.rageLevel[0]) ||
			(this.currentRageLevel == 1 && this.rage >= this.rageLevel[1]) ||
			(this.currentRageLevel == 2 && this.rage >= this.rageLevel[2]))
		{
			this.currentRageLevel++;
			MusicManager.Instance.UpdateLayerMusic(this.currentRageLevel);
		}
	}

	public void IncScore(int gain)
	{
		this.score += gain;
		this.hud.UpdateScore(this.score);
		this.CheckScoreLevel();
		this.IncRage(gain);
		this.hud.UpdateRage(this.rage);
	}

	private void CheckScoreLevel()
	{
		if ((this.currentScoreLevel == 0 && this.score >= this.scoreNeeded[0]) ||
			(this.currentScoreLevel == 1 && this.score >= this.scoreNeeded[1]) ||
			(this.currentScoreLevel == 2 && this.score >= this.scoreNeeded[2]) ||
			(this.currentScoreLevel == 3 && this.score >= this.scoreNeeded[3]))
		{
			int nbDoor = this.doorList.Count;

			for (int i = 0; i < nbDoor; i++)
			{
				if (this.doorList[i].id == this.currentScoreLevel)
				{
					Destroy(this.doorList[i].gameObject);
				}
			}
			this.currentScoreLevel++;
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name.Contains("Menu") == false)
		{
			this.SpawnPlayer();
			this.currentTimer = this.timer;
			this.hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();
			this.hud.Init(this.scoreNeeded[this.scoreNeeded.Count - 1]);
			GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

			this.doorList = new List<Door>();
			for (int i = 0; i < doors.Length; i++)
			{
				this.doorList.Add(doors[i].GetComponent<Door>());
			}
		}
	}
}