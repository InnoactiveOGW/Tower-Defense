using UnityEngine;
using System.Collections;
using System;

public class GroundTowerPlacement : MonoBehaviour {

    public GameObject prefabPlacementObject;
    public GameObject prefabOK;
    public GameObject prefabFail;

	public GameObject ground;

    public float grid = 2.0f;

	public int towerCost = 5;

    // Store which spaces are in use
    int[,] usedSpace;

    GameObject placementObject = null;
    GameObject areaObject = null;

    bool mouseClick = false;
    Vector3 lastPos;

    // Use this for initialization
    void Start () {
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
        Vector3 point;
        
        // Check for mouse ray collision with this object
        if (getTargetLocation(out point))
		{
			GameObject coinCounter = GameObject.Find("CoinCounter");
			Coins coins = coinCounter.GetComponent <Coins>();

            //use the object size from the renderer
			Vector3 halfSlots = ground.GetComponent<Renderer>().bounds.size / 2.0f;

            // Transform position is the center point of this object, x and z are grid slots from 0..slots-1
            int x = (int)Math.Round(Math.Round(point.x - transform.position.x + halfSlots.x - grid / 2.0f) / grid);
            int z = (int)Math.Round(Math.Round(point.z - transform.position.z + halfSlots.z - grid / 2.0f) / grid);

            // Calculate the quantized world coordinates on where to actually place the object
            point.x = (float)(x) * grid - halfSlots.x + transform.position.x + grid / 2.0f;
            point.z = (float)(z) * grid - halfSlots.z + transform.position.z + grid / 2.0f;
			point.y = 0.15f;

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
				areaObject = (GameObject)Instantiate((usedSpace[x, z] == 0 && coins.isPossibleToBuy(towerCost))? prefabOK : prefabFail, point, Quaternion.identity);
            }

            // Create or move the object
            if (!placementObject)
            {
                placementObject = (GameObject)Instantiate(prefabPlacementObject, point, Quaternion.identity);
            }
            else
            {
                placementObject.transform.position = point;
            }

            // On left click, insert the object to the area and mark it as "used"
            if (Input.GetMouseButtonDown(0) && mouseClick == false)
            {
                mouseClick = true;

				if (usedSpace[x, z] == 0 && coins.isPossibleToBuy(towerCost))
                {
					coins.useCoinsToBuy (towerCost);
                    Debug.Log("Placement Position: " + x + ", " + z);
                    usedSpace[x, z] = 1;
                    
                    // ToDo: place the result somewhere..
					GameObject placedTower = (GameObject)Instantiate(prefabPlacementObject, point, Quaternion.identity);
					placedTower.AddComponent <Tower> ();
					NavMeshObstacle navMeshObstacle = placedTower.AddComponent<NavMeshObstacle>() as NavMeshObstacle;
					navMeshObstacle.carving = true;
					CapsuleCollider capsuleCollider = placedTower.AddComponent<CapsuleCollider>() as CapsuleCollider;

				}
            }
            else if (!Input.GetMouseButtonDown(0))
            {
                mouseClick = false;
            }

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

    bool getTargetLocation(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
//			if (hitInfo.collider == ground.GetComponent<Collider>())
//            {
                point = hitInfo.point;
                return true;
//            }
        }
        point = Vector3.zero;
        return false;
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