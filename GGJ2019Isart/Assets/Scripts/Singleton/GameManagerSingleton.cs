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
		
		for (int i = 1; i < nbPlayer; i++)
		{
			MainController controller = Instantiate(playerPrefab).GetComponent<MainController>();
			
			controller.Init(i, this.indexSlotDictionnary[i]);
		}
	}

	public void IncRage(int gain)
	{
		this.rage += gain;
		this.hud.UpdateRage(this.rage);
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
		//this.hud.maxScore = this.scoreNeeded[this.scoreNeeded.Count - 1];
	}
}