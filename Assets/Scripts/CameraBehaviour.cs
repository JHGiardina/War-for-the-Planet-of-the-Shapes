using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject SpawnUnit;
    public Transform RoundTextCameraLocation;

    // How far can the camera be moved
    public Vector3 maxScreenPosition;
    public Vector3 minScreenPosition;

    private Vector3 orbitAxis;
    private float rotationSpeed;
    private float zoomSpeedMouse;
    private float movementSpeed;

    private Transform previousCameraLocation;

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
        HandlePosition();

        HandleZoom();

        HandleUnitSpawn();
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

    public void CameraToRoundTextPosition()
    {
        // Save where we where previously we will need to come back
        previousCameraLocation = transform;

        transform.position = RoundTextCameraLocation.position;
        transform.rotation = RoundTextCameraLocation.rotation;
    }

    public void ReturnCameraToPreviousPosition()
    {
        if(previousCameraLocation != null)
        {
            transform.position = previousCameraLocation.position;
            transform.rotation = previousCameraLocation.rotation;
        }
    }
}
