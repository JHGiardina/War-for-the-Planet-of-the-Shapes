using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject SpawnUnit;
    public GameObject WayPointObject;
    public Transform RoundTextCameraLocation;
    [HideInInspector] public bool IsUserControllable;

    private Vector3 orbitAxis;
    private float rotationSpeed;
    private float zoomSpeedMouse;
    private float movementSpeed;

    private Vector3 previousCameraPosition;
    private Quaternion previousCameraRotation;

    private string SPAWNABLE_SURFACE_TAG = "Prism Terrain";

    void Start()
    {
        // Rotate around y-axis
        orbitAxis = new Vector3(0, 1, 0);

        // Arbitrary speeds based on testing
        movementSpeed = 20;
        rotationSpeed = 1000;
        zoomSpeedMouse = 30;
    }

    void Update()
    {
        if(IsUserControllable)
        {
            HandlePosition();
            HandleZoom();
            HandleUnitSpawn();
            HandleWayPoints();
        }
    }

    private void HandleUnitSpawn()
    {
        // Spawn Units
        if (Input.GetMouseButtonDown(0) /*&& EngineScript.curCount >= 15*/)
        {
            SpawnAtRayHit();
            //EngineScript.curCount -= 15;
            EngineScript.curPop += 1;
        }
    }

    private void SpawnAtRayHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.tag == SPAWNABLE_SURFACE_TAG)
            {
                Vector3 spawnLocation = hit.point;
                Instantiate(SpawnUnit, spawnLocation, SpawnUnit.transform.rotation);
            }
        }
    }

    private void HandlePosition()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); 
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = new Vector3(inputX, 0, inputZ);
        Camera.main.transform.position += movementSpeed * movementDirection * Time.deltaTime;
    }

    private void HandleZoom()
    {
        Vector3 cameraToOrign = Vector3.zero - Camera.main.transform.position;
        Camera.main.transform.position += zoomSpeedMouse * cameraToOrign * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
    }

    private void HandleWayPoints()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SpawnWayPoints();
        }
    }

    private void SpawnWayPoints()
    {
        // get all prisms
        PrismUnitBehaviour[] prisms = GameObject.FindObjectsByType<PrismUnitBehaviour>(FindObjectsSortMode.None);

        // Spawn WayPoint object and calculate position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Destroy old waypoints
            WaypointBehaviour[] previousWayPoints = Object.FindObjectsByType<WaypointBehaviour>(FindObjectsSortMode.None);
            foreach(WaypointBehaviour previousWayPoint in previousWayPoints)
            {
                Destroy(previousWayPoint.gameObject);
            }

            // spawn new waypoints
            Vector3 waypointPosition = hit.point + new Vector3(0, 4, 0);
            var waypointObject = Instantiate(WayPointObject, waypointPosition, Quaternion.identity);
            Destroy(waypointObject, 3);
            
            // move units toward new waypoint
            foreach(PrismUnitBehaviour prism in prisms)
            {
                prism.navMeshAgent.SetDestination(hit.point);
            }
            
        }        
    }

    public void CameraToRoundTextPosition()
    {
        // Save where we where previously we will need to come back
        previousCameraPosition = transform.position;
        previousCameraRotation = transform.rotation;

        Camera.main.transform.position = RoundTextCameraLocation.position;
        Camera.main.transform.rotation = RoundTextCameraLocation.rotation;
    }

    public void ReturnCameraToPreviousPosition()
    {
        if(previousCameraPosition != null)
        {
            Camera.main.transform.position = previousCameraPosition;
            Camera.main.transform.rotation = previousCameraRotation;
        }
    }
}
