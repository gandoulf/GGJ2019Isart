using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MainCharacter))]
[RequireComponent(typeof(CharacterController))]
public class MainController : MonoBehaviour
{
	private enum eInputType : int
	{
		X = 0,
		Y,
		RUN,
		START,
		ACTION,
		SPECIAL
	}

	private int playerJoyId = -1;
	private int playerSlotId = -1;
    public int PlayerSlotId { get { return playerSlotId; } }

	private MainCharacter currentCharacter;
	private CharacterController characterController;

	private int nbInputTypes = System.Enum.GetNames(typeof(eInputType)).Length;
	private string[] inputNameArray;

	private Vector3 moveDirection = Vector3.zero;
	private bool isRunningHold = false;
	private float[] axisValue = new float[2];

	private bool isHidden = false;

	private List<AUsable> usableOjectList = new List<AUsable>();
	private AUsable currentUsableObject = null;

    public bool IsThrowingObjAvailable = false;
    [SerializeField]
    GameObject throwingPosition;
    [SerializeField]
    float throwingPower = 10.0f;

	public void Awake()
	{
		this.currentCharacter = this.GetComponent<MainCharacter>();
		this.characterController = this.GetComponent<CharacterController>();

		this.inputNameArray = new string[nbInputTypes];

		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"));
	}

	public void Init(int joyId, int slotId)
	{
		this.playerJoyId = joyId;
		this.playerSlotId = slotId;

		this.CreateInputNameArray();
	}

	public void OnObjectNearEnter(AUsable usableScript)
	{
		if (this.usableOjectList.Contains(usableScript) == false)
		{
			this.usableOjectList.Add(usableScript);
		}
	}

	public void OnObjectNearExit(AUsable usableScript)
	{
		if (this.usableOjectList.Contains(usableScript) == true)
		{
			if (this.currentUsableObject == usableScript)
			{
				this.UnSelectUsableObject();
			}
			this.usableOjectList.Remove(usableScript);
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (this.currentCharacter.IsCaptured == true)
			return;

		this.UpdateCurrentActiveObject();
		this.CheckInput();
		this.MoveController();
	}

	void CreateInputNameArray()
	{
		this.inputNameArray[(int)eInputType.X] = "Joy" + this.playerJoyId + "Horizontal";
		this.inputNameArray[(int)eInputType.Y] = "Joy" + this.playerJoyId + "Vertical";
		this.inputNameArray[(int)eInputType.RUN] = "Joy" + this.playerJoyId + "Run";
		this.inputNameArray[(int)eInputType.START] = "Joy" + this.playerJoyId + "Start";
		this.inputNameArray[(int)eInputType.ACTION] = "Joy" + this.playerJoyId + "Action";
		this.inputNameArray[(int)eInputType.SPECIAL] = "Joy" + this.playerJoyId + "Special";
	}

	void UpdateCurrentActiveObject()
	{
		foreach (AUsable usableObject in this.usableOjectList)
		{
			if (this.currentUsableObject == null ||
				Vector3.Distance(usableObject.transform.position, this.transform.position) < Vector3.Distance(this.currentUsableObject.transform.position, this.transform.position))
			{
				if (this.currentUsableObject != null)
				{
					this.UnSelectUsableObject();
					//Stop showing button hover object
					this.currentUsableObject.OnObjectFocused(false);
				}
				this.currentUsableObject = usableObject;
				//Show button hover object
				this.currentUsableObject.OnObjectFocused(true);
			}
		}
	}

	//Urgh
	void CheckInput()
	{
		this.isRunningHold = false;
		this.axisValue[0] = 0.0f;
		this.axisValue[1] = 0.0f;

		//Axis
		if (Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.X])) > 0.2 ||
			Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.X])) < -0.2)
		{
			this.axisValue[0] = Input.GetAxis(this.inputNameArray[(int)eInputType.X]);
		}
		if (Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.Y])) > 0.2 ||
			Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.Y])) < -0.2)
		{
			this.axisValue[1] = -Input.GetAxis(this.inputNameArray[(int)eInputType.Y]);
		}
		if (Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.RUN])) > 0.2)
		{
			this.isRunningHold = true;
		}

        if(IsThrowingObjAvailable && Input.GetButtonUp(this.inputNameArray[(int)eInputType.ACTION]) == true)
        {
            IsThrowingObjAvailable = false;
            Debug.Log("toto");
            Vector3 dir;
            var horiz = this.axisValue[0];
            var vert = this.axisValue[1];
            Vector3 tmp = new Vector3(horiz, 0.05f, vert);
            /*if (tmp.sqrMagnitude < 0.2)
                tmp = new Vector3(Random.Range(-0.05f, 0.05f), 0.2f, Random.Range(-0.05f, 0.05f));*/
            dir = tmp.normalized;
            GameObject go = Instantiate(Singleton<GameManagerSingleton>.Instance.RandomThroablePrefab);
            go.transform.position = throwingPosition.transform.position;
            go.GetComponent<Rigidbody>().velocity = dir * throwingPower;
        }

		if (this.currentUsableObject != null)
		{
			//Button
			if (Input.GetButtonDown(this.inputNameArray[(int)eInputType.ACTION]) == true)
			{
				this.currentUsableObject.OnButtonPressed(AUsable.ButtonType.ACTION, this.gameObject);
			}
			else if (Input.GetButtonUp(this.inputNameArray[(int)eInputType.ACTION]) == true)
			{
				this.currentUsableObject.OnButtonReleased(AUsable.ButtonType.ACTION);
			}
			else if (Input.GetButtonDown(this.inputNameArray[(int)eInputType.SPECIAL]) == true)
			{
				if (this.isHidden == false)
				{
					// Exit hiding place
					this.currentUsableObject.OnButtonPressed(AUsable.ButtonType.SPECIAL, this.gameObject);
				}
				else
				{
					// Hide
					this.currentUsableObject.OnButtonReleased(AUsable.ButtonType.SPECIAL);
				}
			}
		}
		if (this.playerSlotId == 0 && Input.GetButtonDown(this.inputNameArray[(int)eInputType.START]) == true)
		{
			// Start menu
		}
	}

	void MoveController()
	{
		this.moveDirection = new Vector3(this.axisValue[0], 0.0f, this.axisValue[1]);
		this.moveDirection = this.transform.TransformDirection(this.moveDirection);
		this.moveDirection = this.moveDirection * (this.currentCharacter.GetSpeed() * (this.isRunningHold ? this.currentCharacter.GetSpeedMultiplier() : 1.0f));
		this.characterController.Move(moveDirection * Time.deltaTime);

		Vector3 pos = Camera.main.WorldToViewportPoint(this.transform.position);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		this.transform.position = Camera.main.ViewportToWorldPoint(pos);
		pos = this.transform.position;
		pos.y = 1.0f;
		this.transform.position = pos;
	}

	void UnSelectUsableObject()
	{
		this.currentUsableObject.OnButtonReleased(AUsable.ButtonType.ACTION);
		this.currentUsableObject.OnObjectFocused(false);
		this.currentUsableObject = null;
	}
}
