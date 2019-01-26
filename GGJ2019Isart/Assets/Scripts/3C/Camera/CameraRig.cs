using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
	[SerializeField]
	public float dampTime = 0.2f;                 // Approximate time for the camera to refocus.
	[SerializeField]
	public float screenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
	[SerializeField]
	public float minSize = 6.5f;                  // The smallest orthographic size the camera can be.
	[SerializeField]
	public float maxSize = 10.5f;                 // The largest orthographic size the camera can be.

	private GameObject[] targets;				  // All the targets the camera needs to encompass.
	private Camera currentCamera;                 // Used for referencing the camera.
	private float zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	private Vector3 moveVelocity;                 // Reference velocity for the smooth damping of the position.
	private Vector3 desiredPosition;              // The position the camera is moving towards.

	private List<GameObject> wallOccludedList;

	private void Awake()
	{
		this.currentCamera = this.GetComponentInChildren<Camera>();
		this.wallOccludedList = new List<GameObject>();
	}

	private void Start()
	{
		this.targets = GameObject.FindGameObjectsWithTag("Player");
		UnityEngine.Assertions.Assert.IsTrue(this.targets.Length > 0, "Players were not found.");
		if (this.targets.Length == 1)
		{
			this.minSize = 8.0f;
		}
		this.SetStartPositionAndSize();
	}

	private void LateUpdate()
	{
		// Move the camera towards a desired position.
		this.Move();

		// Change the size of the camera based.
		this.Zoom();
		// Raycast in order to change wall alpha
		this.CheckWall();
	}

	private void Move()
	{
		// Find the average position of the targets.
		this.FindAveragePosition();

		// Smoothly transition to that position.
		this.transform.position = Vector3.SmoothDamp(transform.position, this.desiredPosition, ref this.moveVelocity, this.dampTime);
	}


	private void FindAveragePosition()
	{
		Vector3 averagePos = new Vector3();
		int numTargets = 0;

		// Go through all the targets and add their positions together.
		for (int i = 0; i < this.targets.Length; i++)
		{
			// If the target isn't active, go on to the next one.
			if (!this.targets[i].gameObject.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += this.targets[i].transform.position;
			numTargets++;
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		if (numTargets > 0)
			averagePos /= numTargets;

		// Keep the same y value.
		averagePos.y = transform.position.y;

		// The desired position is the average position;
		this.desiredPosition = averagePos;
	}


	private void Zoom()
	{
		// Find the required size based on the desired position and smoothly transition to that size.
		float requiredSize = FindRequiredSize();
		this.currentCamera.orthographicSize = Mathf.SmoothDamp(this.currentCamera.orthographicSize, requiredSize, ref this.zoomSpeed, this.dampTime);
	}


	private float FindRequiredSize()
	{
		// Find the position the camera rig is moving towards in its local space.
		Vector3 desiredLocalPos = this.transform.InverseTransformPoint(this.desiredPosition);

		// Start the camera's size calculation at zero.
		float size = 0f;

		// Go through all the targets...
		for (int i = 0; i < this.targets.Length; i++)
		{
			// ... and if they aren't active continue on to the next target.
			if (!this.targets[i].activeSelf)
				continue;

			// Otherwise, find the position of the target in the camera's local space.
			Vector3 targetLocalPos = transform.InverseTransformPoint(this.targets[i].transform.position);

			// Find the position of the target from the desired position of the camera's local space.
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			// Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.z));

			// Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / this.currentCamera.aspect);
		}

		// Add the edge buffer to the size.
		size += this.screenEdgeBuffer;

		size = Mathf.Clamp(size, this.minSize, this.maxSize);
		return size;
	}

	public void SetStartPositionAndSize()
	{
		// Find the desired position.
		this.FindAveragePosition();

		// Set the camera's position to the desired position without damping.
		this.transform.position = this.desiredPosition;

		// Find and set the required size of the camera.
		this.currentCamera.orthographicSize = this.FindRequiredSize();
	}

	private void CheckWall()
	{
		int nbPlayer = GameManagerSingleton.Instance.indexSlotDictionnary.Count;

		for (int i = 0; i < nbPlayer; i++)
		{
			RaycastHit hit;
			
			if (Physics.Raycast(this.currentCamera.transform.position, this.targets[i].transform.position - this.currentCamera.transform.position, out hit, Mathf.Infinity))
			{
				if (hit.collider.CompareTag("Wall") == true)
				{
					hit.collider.gameObject.GetComponent<Wall>().ChangeAlpha(true);
					this.wallOccludedList.Add(hit.collider.gameObject);
				}
				else if (this.wallOccludedList.Count > 0)
				{
					this.wallOccludedList[0].GetComponent<Wall>().ChangeAlpha(false);
					this.wallOccludedList.Clear();
				}
			}
		}
	}
}
