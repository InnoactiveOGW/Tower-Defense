using UnityEngine;
using System.Collections;
using System;

public class LaserTowerPlacement : MonoBehaviour {

	int groundMask;
	int towerMask;
	int pauseMask;
	LineRenderer line;
	Color groundLine;
	Color towerLine;
	Color outsideLine;
	SteamVR_TrackedController controller;

	public int towerCost = 5;

	/*Ab jetzt aus GroundTowerPlacement*/
	public GameObject prefabPlacementObject;
	public GameObject prefabOK;
	public GameObject prefabFail;
	public GameObject ground;
	public float grid = 2.0f;

	// Store which spaces are in use
	int[,] usedSpace;

	GameObject placementObject = null;
	GameObject areaObject = null;

	bool padWasClicked = false;
	Vector3 lastPos;
	Vector3 towerPoint;
	bool towerPlacement;
	bool insideField;
	bool pauseButton;
	bool gameIsPaused;


	// Use this for initialization
	void Start () {
		groundMask = LayerMask.GetMask ("Ground");
		towerMask = LayerMask.GetMask ("Tower");
		pauseMask = LayerMask.GetMask ("Pause");
		line = GetComponent <LineRenderer> ();
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.enabled = false;
		pauseButton = false;
		gameIsPaused = false;
		groundLine = Color.green;
		towerLine = Color.red;
		outsideLine = Color.white;
		//outsideLine = new Color (1, 1, 1, 0);

		controller = GetComponent<SteamVR_TrackedController>();
		if (controller == null)
		{
			controller = gameObject.AddComponent<SteamVR_TrackedController>();
		}
		controller.TriggerClicked += new ClickedEventHandler(DoClick);
		controller.PadClicked += new ClickedEventHandler (PadCLicked);

		/*GroundTowerPlacement*/
		if (ground == null) {
			ground = GameObject.Find ("Ground");
		}
		Vector3 slots = ground.GetComponent<Renderer>().bounds.size / grid;
		usedSpace = new int[Mathf.CeilToInt(slots.x), Mathf.CeilToInt(slots.z)];
		for(var x = 0; x < Mathf.CeilToInt(slots.x); x++)
		{
			for (var z = 0; z < Mathf.CeilToInt(slots.z); z++)
			{
				usedSpace[x, z] = 0;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit raycastHit;
		Ray ray = new Ray(transform.position, transform.forward);
		pauseButton = false;

		if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, towerMask) && !gameIsPaused) {
			towerPoint = raycastHit.point;
			line.SetColors (towerLine, towerLine);
			insideField = true;
		} else if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, groundMask) && !gameIsPaused) {
			towerPoint = raycastHit.point;	
			line.SetColors (groundLine, groundLine);
			insideField = true;
		} else if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, pauseMask)) {
			line.SetColors (groundLine, groundLine);
			insideField = false;
			pauseButton = true;
		} else {
			line.SetColors (outsideLine, outsideLine);
			insideField = false;
		}

		line.SetPosition (0, controller.transform.position);
		line.SetPosition (1, controller.transform.TransformDirection (Vector3.forward) * 1000);


		/*Ground Tower Placement*/

		// Check for mouse ray collision with this object
		if (line.enabled && insideField)
		{
			GameObject coinCounter = GameObject.Find("CoinCounter");
			Coins coins = coinCounter.GetComponent <Coins>();

			//I'm lazy and use the object size from the renderer..
			Vector3 halfSlots = ground.GetComponent<Renderer>().bounds.size / 2.0f;

			Vector3 groundPosition = GameObject.Find ("Ground").transform.position;


			// Transform position is the center point of this object, x and z are grid slots from 0..slots-1
			int x = (int)Math.Round(Math.Round(towerPoint.x - groundPosition.x + halfSlots.x - grid / 2.0f) / grid);
			int z = (int)Math.Round(Math.Round(towerPoint.z - groundPosition .z + halfSlots.z - grid / 2.0f) / grid);

			// Calculate the quantized world coordinates on where to actually place the object
			towerPoint.x = (float)(x) * grid - halfSlots.x + groundPosition.x + grid / 2.0f;
			towerPoint.z = (float)(z) * grid - halfSlots.z + groundPosition.z + grid / 2.0f;
			towerPoint.y = 0.15f;

			//Debug.Log ("towerPoint: " + towerPoint);

			// Create an object to show if this area is available for building
			// Re-instantiate only when the slot has changed or the object not instantiated at all
			if (lastPos.x != x || lastPos.z != z || areaObject == null)
			{
				lastPos.x = x;
				lastPos.z = z;
				if (areaObject != null)
				{
					Destroy(areaObject);
				}
				areaObject = (GameObject)Instantiate((usedSpace[x, z] == 0  && coins.isPossibleToBuy(towerCost))? prefabOK : prefabFail, towerPoint, Quaternion.identity);
			}

			// Create or move the object
			if (!placementObject)
			{
				placementObject = (GameObject)Instantiate(prefabPlacementObject, towerPoint, Quaternion.identity);
			}
			else
			{
				placementObject.transform.position = towerPoint;
			}

			// On left click, insert the object to the area and mark it as "used"
			if (padWasClicked)
			{
				// Place the object
				if (usedSpace[x, z] == 0 && coins.isPossibleToBuy(towerCost))
				{
					coins.useCoinsToBuy (towerCost);
					Debug.Log("Placement Position: " + x + ", " + z);
					usedSpace[x, z] = 1;

					// ToDo: place the result somewhere..
					GameObject placedTower = (GameObject)Instantiate(prefabPlacementObject, towerPoint, Quaternion.identity);
					placedTower.AddComponent <Tower> ();
					NavMeshObstacle navMeshObstacle = placedTower.AddComponent<NavMeshObstacle>() as NavMeshObstacle;
					navMeshObstacle.carving = true;
					CapsuleCollider capsuleCollider = placedTower.AddComponent<CapsuleCollider>() as CapsuleCollider;

				}
				padWasClicked = false;
			}

		}
		else if (padWasClicked && pauseButton){
			if (!gameIsPaused) {
				Debug.Log ("Pause");
				Time.timeScale = 0;
				gameIsPaused = true;
			} else {
				Debug.Log ("Nicht mehr Pause");
				Time.timeScale = 1;
				gameIsPaused = false;

			}
			padWasClicked = false;
		}
		else
		{
			if (placementObject)
			{
				Destroy(placementObject);
				placementObject = null;
			}
			if(areaObject)
			{
				Destroy(areaObject);
				areaObject = null;
			}
		}
	}

	void DoClick(object sender, ClickedEventArgs e){
		if (line.enabled == false) {
			line.enabled = true;
		} else if (line.enabled == true) {
			line.enabled = false;
		}
	}

	void PadCLicked(object sender, ClickedEventArgs e){
		if (line.enabled && insideField)
			padWasClicked = true;
		else if (line.enabled && pauseButton)
			padWasClicked = true;
	}

	public void updateUsedSpaceAt(Transform tower, int value){
		if (value < 0 || value > 1) {
			Debug.Log ("Invalid Value");
			return;
		}
		Vector3 halfSlots = ground.GetComponent<Renderer>().bounds.size / 2.0f;

		int x = (int)Math.Round(Math.Round(tower.position.x - transform.position.x + halfSlots.x - grid / 2.0f) / grid);
		int z = (int)Math.Round(Math.Round(tower.position.z - transform.position.z + halfSlots.z - grid / 2.0f) / grid);

		usedSpace[x,z] = value;
	}
}
