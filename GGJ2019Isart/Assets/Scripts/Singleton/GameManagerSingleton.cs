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
	public int score;
	public int rage;
	public float timer;
	public List<int> scoreNeeded;
	public List<int> rageLevel;
	public int currentRageLevel;

	private HUD hud;

	private void Start()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		DontDestroyOnLoad(this.gameObject);
	}

	private void Update()
	{
		if (this.hud != null)
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0.0f)
			{
				this.timer = 0.0f;
				SceneManager.LoadScene("Menu");
			}
			else
			{
				this.hud.UpdateTimer(this.timer);
			}
		}
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
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.SpawnPlayer();
		this.hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();
		this.hud.Init(this.scoreNeeded[this.scoreNeeded.Count - 1]);
	}
}