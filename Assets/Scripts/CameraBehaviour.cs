using System.Collections;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject SpawnUnit;
    public GameObject WayPointObject;
    public Transform RoundTextCameraLocation;
    public float transitionTime = 2;
    public int UnitSpawnCost = 5;
    [HideInInspector] public bool IsUserControllable;
    [HideInInspector] public bool isTransitioning;

    private float zoomSpeedMouse;
    private float movementSpeed;
    private float rotationSpeed;

    private Vector3 previousCameraPosition;
    private Quaternion previousCameraRotation;

    private string SPAWNABLE_SURFACE_TAG = "Prism Terrain";

    private int resourceLayerMask;

    void Start()
    {
        // Arbitrary speeds based on testing
        movementSpeed = 20;
        zoomSpeedMouse = 30;
        isTransitioning = false;
        rotationSpeed= 700;

        // Layer masks so we can spawn way points in resources
        resourceLayerMask = ~LayerMask.GetMask("Resource");
    }

    void Update()
    {
        if(IsUserControllable && !isTransitioning)
        {
            HandlePosition();
            HandleZoom();
            HandleUnitSpawn();
            HandleWayPoints();
            HandleRotation();
        }
    }

    private void HandleUnitSpawn()
    {
        // Spawn Units
        if (Input.GetMouseButtonDown(0) /*&& EngineScript.curCount >= 15*/)
        {
            SpawnAtRayHit();
        }
    }

    private void SpawnAtRayHit()
    {
        // If you can't afford the unit you can't spawn it
        if(!(EngineScript.curCount - UnitSpawnCost >= 0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.tag == SPAWNABLE_SURFACE_TAG)
            {
                Vector3 spawnLocation = hit.point;
                Instantiate(SpawnUnit, spawnLocation, SpawnUnit.transform.rotation);
            }
        }

        // That'll cost you resources ;)
        EngineScript.curCount -= UnitSpawnCost;
    }

    // Relative to where camera is facing
    private void HandlePosition()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); 
        float inputZ = Input.GetAxisRaw("Vertical");

        // Front and back movement
        Vector3 cameraForwardDirection = Camera.main.transform.forward;
        Vector3 forwardMovementDirection = new Vector3(cameraForwardDirection.x, 0, cameraForwardDirection.z).normalized;
        Camera.main.transform.position += movementSpeed * forwardMovementDirection  * inputZ * Time.deltaTime;

        // Right and Left movement
        Vector3 cameraRightDirection = Camera.main.transform.right;
        Vector3 rightMovementDirection = new Vector3(cameraRightDirection.x, 0, cameraRightDirection.z).normalized;
        Camera.main.transform.position += movementSpeed * rightMovementDirection  * inputX * Time.deltaTime;
    }

    private void HandleZoom()
    {
        Vector3 cameraToOrign = Vector3.zero - Camera.main.transform.position;
        Camera.main.transform.position += zoomSpeedMouse * cameraToOrign * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
    }

    private void HandleRotation()
    {
        // Rotate around the z y-axis when middle mouse is selected
        if(Input.GetMouseButton(2))
        {
            float inputX = Input.GetAxis("Mouse X");
            Vector3 yAxis = new Vector3(0, 1, 0);
            transform.RotateAround(transform.position, yAxis, inputX * rotationSpeed * Time.deltaTime);
        }
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
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, resourceLayerMask))
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
                if(!prism.IsBase)
                {
                    prism.navMeshAgent.SetDestination(hit.point);
                }
            }
            
        }        
    }

    public void CameraToRoundTextPosition()
    {            
        // Save where we where previously we will need to come back
        previousCameraPosition = transform.position;
        previousCameraRotation = transform.rotation;

        Vector3 targetPosition = RoundTextCameraLocation.position;
        Quaternion targetRotation = RoundTextCameraLocation.rotation;

        StartCoroutine(TransitionBetweenPositions(targetPosition, targetRotation));
    }

    public void ReturnCameraToPreviousPosition()
    {
        Vector3 targetPosition = previousCameraPosition;
        Quaternion targetRotation = previousCameraRotation;

        StartCoroutine(TransitionBetweenPositions(targetPosition, targetRotation));
    }

    public IEnumerator TransitionBetweenPositions(Vector3 targetPosition, Quaternion targetRotation)
    {
        // I had to ask Gemini how to use lerp, slerp, and coroutines for transitioning

        isTransitioning = true;

        Vector3 orignalPosition = transform.position;
        Quaternion orignalRotation = transform.rotation;
        
        for(float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(orignalPosition, targetPosition, t / transitionTime);
            Camera.main.transform.rotation = Quaternion.Slerp(orignalRotation, targetRotation, t / transitionTime);

            t += Time.deltaTime;
            yield return null;
        }

        // Just in case the time steps dont bring us to our exact location
        Camera.main.transform.position = targetPosition;
        Camera.main.transform.rotation = targetRotation;

        isTransitioning = false;

    }
}
