using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUsable : MonoBehaviour
{
	public enum ActionType
	{
		NONE = 0,
		MASH,
		HOLD
	}

    protected bool bIsUseable = true;
    public virtual bool IsUseable { get { return bIsUseable; } }

    private Material baseMat;
    [SerializeField]
    private Renderer renderer;
	private SpriteRenderer spriteRenderer;
	private List<GameObject> players = new List<GameObject>();

	[SerializeField]
	private Sprite[] spriteButtons;

	public bool isAction = false;
	public bool isSpecial = false;

	public ActionType actionType = ActionType.NONE;

	protected virtual void Start()
    {
        if (!renderer)
            renderer = GetComponent<Renderer>();
        if (renderer)
            baseMat = renderer.material;
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		if (spriteRenderer != null)
			spriteRenderer.enabled = false;
    }

    public virtual void OnButtonPressed(GameObject player, MainController.eInputType buttonTypePressed)
    {

    }

    public virtual void OnButtonReleased()
    {

    }

	public virtual void OnObjectFocused(bool isFocused)
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.enabled = isFocused;
			if (actionType == ActionType.MASH)
			{
				spriteRenderer.sprite = spriteButtons[((int)ActionType.MASH) - 1];
			}
			else if (actionType == ActionType.HOLD)
			{
				spriteRenderer.sprite = spriteButtons[((int)ActionType.HOLD) - 1];
			}
			if (isSpecial == true)
			{
				//Todo Add two types to display
			}
		}
	}

	public virtual void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Player") == true && IsUseable == true)
		{
			collider.gameObject.GetComponent<MainController>().OnObjectNearEnter(this);
            players.Add(collider.gameObject);
            if (renderer)
                setMaterial();
        }
    }

	public virtual void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("Player") == true)
		{
			collider.gameObject.GetComponent<MainController>().OnObjectNearExit(this);
            players.Remove(collider.gameObject);
            if (renderer)
                setMaterial();
        }
    }

    protected virtual void setMaterial()
    {
		if (players.Count == 0)
			renderer.material = baseMat;
		else
		{
			int closestPlayer = 0;
			for (int i = 0; i < players.Count; i++)
			{
				if (Vector3.Distance(players[i].transform.position, gameObject.transform.position) < Vector3.Distance(players[closestPlayer].transform.position, gameObject.transform.position))
					closestPlayer = i;
			}
			renderer.material = Singleton<GameManagerSingleton>.Instance.PlayerOutlineColor[players[closestPlayer].GetComponent<MainController>().PlayerSlotId];
		}
	}
}
