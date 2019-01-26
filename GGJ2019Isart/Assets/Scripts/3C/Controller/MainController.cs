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
	// To put in private after the lobby is done
	public int playerJoyId = -1;
	public int playerSlotId = -1;

	private MainCharacter currentCharacter;
	private CharacterController characterController;

	private int nbInputTypes = System.Enum.GetNames(typeof(eInputType)).Length;
	private string[] inputNameArray;

	private Vector3 moveDirection = Vector3.zero;
	private bool isActionHold = false;
	private bool isRunningHold = false;
	private float[] axisValue = new float[2];

	public void Awake()
	{
		this.currentCharacter = this.GetComponent<MainCharacter>();
		this.characterController = this.GetComponent<CharacterController>();

		this.inputNameArray = new string[nbInputTypes];

		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"));

		// To remove when the lobby is done
		this.Init(playerJoyId, playerSlotId);
	}

	public void Init(int joyId, int slotId)
	{
		this.playerJoyId = joyId;
		this.playerSlotId = slotId;

		this.CreateInputNameArray();
	}

    // Update is called once per frame
    void Update()
    {
		CheckInput();
		MoveController();
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

		//Button
		if (Input.GetButtonDown(this.inputNameArray[(int)eInputType.ACTION]) == true)
		{
			this.isActionHold = true;
		}
		else if (Input.GetButtonUp(this.inputNameArray[(int)eInputType.ACTION]) == true)
		{
			if (this.isActionHold == true)
			{

			}
			else
			{

			}
			this.isActionHold = false;
		}
		else if (Input.GetButton(this.inputNameArray[(int)eInputType.SPECIAL]) == true)
		{
			// Hide
		}
		else if (this.playerSlotId == 0 && Input.GetButton(this.inputNameArray[(int)eInputType.START]) == true)
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
}
