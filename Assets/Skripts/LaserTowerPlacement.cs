using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class LaserTowerPlacement : MonoBehaviour {

	int groundMask;
	int towerMask;
	int pauseMask;
	int playMask;
	int endGameMask;
	int startGameMask;
	int upgradeMask;

	LineRenderer line;
	Color groundLine;
	Color towerLine;
	Color outsideLine;
	Color failedLine;
	SteamVR_TrackedController controller;

	int towerCost = 10;
	int upgradeCost = 25;

	/*Ab jetzt aus GroundTowerPlacement*/
	public GameObject prefabPlacementObject;
	public GameObject prefabOK;
	public GameObject prefabFail;
	public GameObject ground;
	GameObject upgradeMenu;
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
	bool playButton;
	bool endGameButton;
	bool startGameButton;
	bool towerSelected;
	bool towerMenuOpen;
	bool finalExplosion;
	int upgradeButtons; //0 für default, 1 für damage, 2 für speed, 3 für splash

	GameObject pauseObject;
	GameObject playObject;
	GameObject endGameObject;
	GameObject startGameObject;
	GameObject gameOverScreen;
	GameObject startGameScreen;
	GameObject selectedTower;

	Coins coinSystem;
	AudioSource towerBuildSound;

	// Use this for initialization
	void Start () {

		towerBuildSound = GetComponent<AudioSource> ();
		groundMask = LayerMask.GetMask ("Ground");
		towerMask = LayerMask.GetMask ("Tower");
		pauseMask = LayerMask.GetMask ("Pause");
		playMask = LayerMask.GetMask ("Play");
		endGameMask = LayerMask.GetMask ("EndGame");
		startGameMask = LayerMask.GetMask ("StartGame");
		upgradeMask = LayerMask.GetMask ("Upgrade");

		line = GetComponent <LineRenderer> ();
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.enabled = false;
		gameIsPaused = true;
		groundLine = Color.green;
		towerLine = Color.blue;
		outsideLine = Color.white;
		failedLine = Color.red;

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

		pauseObject = GameObject.Find ("PauseButton");
		playObject = GameObject.Find ("PlayButton");
		endGameObject = GameObject.Find ("EndGameButton");
		startGameObject = GameObject.Find ("StartGameButton");
		gameOverScreen = GameObject.Find ("GameOverScreen");
		startGameScreen = GameObject.Find ("StartGameScreen");

		coinSystem = GameObject.Find ("CoinText").GetComponent<Coins>();

		gameIsStarted ();
	}
	
	// Update is called once per frame
	void Update () {

		if (GameObject.Find ("Home").transform.GetChild (0).GetComponent<HouseHealth> ().getGameOver ()) {
			gameOver ();
		}


		RaycastHit raycastHit;
		Ray ray = new Ray(transform.position, transform.forward);

		if (towerMenuOpen) {
			if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, upgradeMask)) {
				if (coinSystem.isPossibleToBuy (upgradeCost)) {
					line.SetColors (groundLine, groundLine);
				} else {
					line.SetColors (failedLine, failedLine);
				}
				switch (raycastHit.collider.name) {
				case ("DamageUpgrade"):
					upgradeButtons = 1;
					break;
				case ("SpeedUpgrade"):
					upgradeButtons = 2;
					break;
				case ("SplashUpgrade"):
					upgradeButtons = 3;
					break;
				default:
					upgradeButtons = 0;
					break;
				}
			} else {
				upgradeButtons = 0;
				line.SetColors (outsideLine, outsideLine);
			}
		} else {

			if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, towerMask) && !gameIsPaused) {
				selectedTower = raycastHit.transform.gameObject;
				if (selectedTower.GetComponent<Tower> ().towerUpgraded ()) {
					line.SetColors (failedLine, failedLine);
				} else {
					line.SetColors (towerLine, towerLine);
				}
				towerPoint = raycastHit.point;
				insideField = true;
				towerSelected = true;
			} else if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, groundMask) && !gameIsPaused) {
				if (coinSystem.isPossibleToBuy (towerCost)) {
					towerPoint = raycastHit.point;	
					line.SetColors (groundLine, groundLine);
					insideField = true;
					towerSelected = false;
				} else {
					towerPoint = raycastHit.point;	
					line.SetColors (failedLine, failedLine);
					insideField = true;
					towerSelected = false;
				}
			} else if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, pauseMask)) {
				line.SetColors (groundLine, groundLine);
				insideField = false;
				pauseButton = true;
				playButton = false;
				endGameButton = false;
				startGameButton = false;
				towerSelected = false;
			} else if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, playMask)) {
				line.SetColors (groundLine, groundLine);
				insideField = false;
				pauseButton = false;
				playButton = true;
				endGameButton = false;
				startGameButton = false;
				towerSelected = false;
			} else if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, endGameMask)) {
				line.SetColors (groundLine, groundLine);
				insideField = false;
				pauseButton = false;
				playButton = false;
				endGameButton = true;
				startGameButton = false;
				towerSelected = false;
			} else if (Physics.Raycast (ray, out raycastHit, Mathf.Infinity, startGameMask)) {
				line.SetColors (groundLine, groundLine);
				insideField = false;
				pauseButton = false;
				playButton = false;
				endGameButton = false;
				startGameButton = true;
				towerSelected = false;
			} else {
				line.SetColors (outsideLine, outsideLine);
				insideField = false;
				towerSelected = false;
			}
		}

		line.SetPosition (0, controller.transform.position);
		line.SetPosition (1, controller.transform.TransformDirection (Vector3.forward) * 1000);


		/*Ground Tower Placement*/

		// Check for mouse ray collision with this object

		if (towerMenuOpen) {
			if (padWasClicked) {
				if (coinSystem.isPossibleToBuy (upgradeCost)) {
					switch (upgradeButtons) {
					case 0:
						break;
					case 1:
					//damage
						selectedTower.GetComponent<Tower> ().upgradeDamage (upgradeCost);
						break;
					case 2:
					//speed upgraden
						selectedTower.GetComponent<Tower> ().upgradeSpeed (upgradeCost);
						break;
					case 3:
					//splash einbinden
						selectedTower.GetComponent<Tower> ().upgradeSplash (upgradeCost);
						break;
					}
					closeTowerMenu ();
				}
			}
		} else {

			if (line.enabled && insideField) {
				GameObject coinCounter = GameObject.Find ("CoinText");
				Coins coins = coinCounter.GetComponent <Coins> ();


				//I'm lazy and use the object size from the renderer..
				Vector3 halfSlots = ground.GetComponent<Renderer> ().bounds.size / 2.0f;

				Vector3 groundPosition = GameObject.Find ("Ground").transform.position;


				// Transform position is the center point of this object, x and z are grid slots from 0..slots-1
				int x = (int)Math.Round (Math.Round (towerPoint.x - groundPosition.x + halfSlots.x - grid / 2.0f) / grid);
				int z = (int)Math.Round (Math.Round (towerPoint.z - groundPosition.z + halfSlots.z - grid / 2.0f) / grid);

				// Calculate the quantized world coordinates on where to actually place the object
				towerPoint.x = (float)(x) * grid - halfSlots.x + groundPosition.x + grid / 2.0f;
				towerPoint.z = (float)(z) * grid - halfSlots.z + groundPosition.z + grid / 2.0f;
				towerPoint.y = 0.15f;

				//Debug.Log ("towerPoint: " + towerPoint);

				// Create an object to show if this area is available for building
				// Re-instantiate only when the slot has changed or the object not instantiated at all
				if (lastPos.x != x || lastPos.z != z || areaObject == null) {
					lastPos.x = x;
					lastPos.z = z;
					if (areaObject != null) {
						Destroy (areaObject);
					}
					//areaObject = (GameObject)Instantiate ((usedSpace [x, z] == 0 && coins.isPossibleToBuy (towerCost)) ? prefabOK : prefabFail, towerPoint, Quaternion.identity);
				}

				// Create or move the object
				if (!placementObject) {
					placementObject = (GameObject)Instantiate (prefabPlacementObject, towerPoint, Quaternion.identity);
				} else {
					placementObject.transform.position = towerPoint;
				}

				// On left click, insert the object to the area and mark it as "used"
				if (padWasClicked) {
					// Place the object
					if (usedSpace [x, z] == 0 && coins.isPossibleToBuy (towerCost)) {
						coins.useCoinsToBuy (towerCost);
						Debug.Log ("Placement Position: " + x + ", " + z);
						usedSpace [x, z] = 1;

						// ToDo: place the result somewhere..
						towerBuildSound.Play();
						GameObject placedTower = (GameObject)Instantiate (prefabPlacementObject, towerPoint, Quaternion.identity);
						placedTower.AddComponent <Tower> ();
						NavMeshObstacle navMeshObstacle = placedTower.AddComponent<NavMeshObstacle> () as NavMeshObstacle;
						navMeshObstacle.carving = true;
						CapsuleCollider capsuleCollider = placedTower.AddComponent<CapsuleCollider> () as CapsuleCollider;

					} else if (towerSelected && !(selectedTower.GetComponent<Tower> ().towerUpgraded ())) {
						openTowerMenu ();
					}
					padWasClicked = false;
				}

			} else if (padWasClicked && pauseButton) {
				pauseGameIsClicked ();
				gameIsPaused = true;
				padWasClicked = false;
			} else if (padWasClicked && playButton) {
				startGameIsClicked ();
				gameIsPaused = false;
				padWasClicked = false;
			} else if (padWasClicked && endGameButton) {
				endGameIsClicked ();
				padWasClicked = false;
			} else if (padWasClicked && startGameButton) {
				startGameIsClicked ();
				padWasClicked = false;
			} else {
				if (placementObject) {
					Destroy (placementObject);
					placementObject = null;
				}
				if (areaObject) {
					Destroy (areaObject);
					areaObject = null;
				}
			}
		}
	}

	void DoClick(object sender, ClickedEventArgs e){
		if (line.enabled == false) {
			line.enabled = true;
		} else if (line.enabled == true) {
			line.enabled = false;
			if (towerMenuOpen)
				closeTowerMenu ();
		}
	}

	void PadCLicked(object sender, ClickedEventArgs e){
		if (line.enabled && insideField)
			padWasClicked = true;
		else if (line.enabled && (pauseButton || playButton || startGameButton || endGameButton))
			padWasClicked = true;
		else if (towerMenuOpen)
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

	void gameIsStarted(){
		Time.timeScale = 0;
		startGameScreen.SetActive (true);
		startGameObject.SetActive (true);
	}
		

	void startGameIsClicked(){
		pauseObject.SetActive (true);
		pauseObject.transform.position = new Vector3(-10.0f, 5.0f, -5.0f);
		Time.timeScale = 1;
		playObject.transform.position = new Vector3(-10.0f, 5.0f, -5.0f);
		playObject.SetActive (false);
		endGameObject.transform.position = new Vector3(-10.0f, 2.0f, -5.0f);
		endGameObject.SetActive (false);
		gameOverScreen.transform.position = new Vector3(-10.0f, 6.0f, -5.0f);
		gameOverScreen.SetActive (false);
		startGameObject.SetActive (false);
		startGameScreen.SetActive (false);
		gameIsPaused = false;
	}

	void pauseGameIsClicked(){
		playObject.SetActive (true);
		endGameObject.SetActive (true);
		Time.timeScale = 0;
		pauseObject.SetActive (false);
	}

	void endGameIsClicked(){
		gameOver ();
	}

	public void gameOver(){
		//spiel vorbei von househealth
		gameOverScreen.SetActive (true);
		String endScore = GameObject.Find("TimeText").GetComponent<Timer>().getTimeScore();
		endScore = "Game Over! \n Your time: " + endScore;
		GameObject temp = GameObject.Find ("GameOverText");
		TextMesh tempMesh = temp.GetComponent<TextMesh> ();
		tempMesh.text = endScore;
		Time.timeScale = 0;
		playObject.SetActive (false);
		endGameObject.SetActive (false);
	}

	void openTowerMenu (){
		towerMenuOpen = true;
		upgradeMenu = Instantiate (Resources.Load ("UpgradeBox")) as GameObject;
		Vector3 temp = new Vector3 (towerPoint.x, 4, towerPoint.z);
		upgradeMenu.transform.position = temp;
	}

	void closeTowerMenu(){
		towerMenuOpen = false;
		padWasClicked = false;
		Destroy (upgradeMenu);
	}
		
}
