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

	private enum eDirection
	{
		NONE = 0,
		FRONT,
		BACK,
		RIGHT,
		LEFT
	}

	private int playerJoyId = -1;
	private int playerSlotId = -1;

	private MainCharacter currentCharacter;
	private CharacterController characterController;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private int nbInputTypes = System.Enum.GetNames(typeof(eInputType)).Length;
	private string[] inputNameArray;

	private Vector3 moveDirection = Vector3.zero;
	private bool isMoving = false;
	private bool isSprintingHold = false;
	private float[] axisValue = new float[2];

	private bool isHidden = false;

	private List<AUsable> usableOjectList = new List<AUsable>();
	private AUsable currentUsableObject = null;

	private eDirection direction = eDirection.NONE;

	public void Awake()
	{
		this.currentCharacter = this.GetComponent<MainCharacter>();
		this.characterController = this.GetComponent<CharacterController>();
		this.animator = this.GetComponent<Animator>();
		this.spriteRenderer = this.GetComponent<SpriteRenderer>();

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
		if (this.usableOjectList.Contains(usableScript) == false)
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
		this.SetAnimationStates();
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
		this.isSprintingHold = false;
		this.isMoving = false;
		this.axisValue[0] = 0.0f;
		this.axisValue[1] = 0.0f;
		this.direction = eDirection.NONE;

		//Axis
		if (Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.X])) > 0.2 ||
			Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.X])) < -0.2)
		{
			this.axisValue[0] = Input.GetAxis(this.inputNameArray[(int)eInputType.X]);
			this.isMoving = true;
			if (this.axisValue[0] > 0)
			{
				this.direction = eDirection.RIGHT;
				this.spriteRenderer.flipX = true;
			}
			else
			{
				this.direction = eDirection.LEFT;
				this.spriteRenderer.flipX = false;
			}
		}
		if (Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.Y])) > 0.2 ||
			Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.Y])) < -0.2)
		{
			this.axisValue[1] = -Input.GetAxis(this.inputNameArray[(int)eInputType.Y]);
			this.isMoving = true;
			if (this.axisValue[1] > 0)
			{
				this.direction = eDirection.FRONT;
			}
			else
			{
				this.direction = eDirection.BACK;
			}
		}

		if (this.isMoving  == true && Mathf.Abs(Input.GetAxis(this.inputNameArray[(int)eInputType.RUN])) > 0.2)
		{
			this.isSprintingHold = true;
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
		this.moveDirection = this.moveDirection * (this.currentCharacter.GetSpeed() * (this.isSprintingHold ? this.currentCharacter.GetSpeedMultiplier() : 1.0f));
		this.characterController.Move(moveDirection * Time.deltaTime);

		Vector3 pos = Camera.main.WorldToViewportPoint(this.transform.position);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		this.transform.position = Camera.main.ViewportToWorldPoint(pos);
		pos = this.transform.position;
		pos.y = 0.0f;
		this.transform.position = pos;
	}
	
	void SetAnimationStates()
	{
		this.animator.SetBool("IsRunning", this.isMoving);
		this.animator.SetBool("IsSprinting", this.isSprintingHold);
		this.animator.SetInteger("Direction", (int)this.direction);
	}

	void UnSelectUsableObject()
	{
		this.currentUsableObject.OnButtonReleased(AUsable.ButtonType.ACTION);
		this.currentUsableObject.OnObjectFocused(false);
		this.currentUsableObject = null;
	}
}
