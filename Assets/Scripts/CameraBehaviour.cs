using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject SpawnUnit;

    // How far can the camera be moved
    public Vector3 maxScreenPosition;
    public Vector3 minScreenPosition;

    private Vector3 orbitAxis;
    private float rotationSpeed;
    private float zoomSpeedMouse;
    private float movementSpeed;


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

        // Zoom
        Vector3 cameraToOrign = Vector3.zero - Camera.main.transform.position;
        Camera.main.transform.position += zoomSpeedMouse * cameraToOrign * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;

        // Rotation around World Origin with middle mouse
        if (Input.GetMouseButton(2))
        {
            Vector3 orbitCenter = new Vector3(transform.position.x, 1, transform.position.z);
            Camera.main.transform.RotateAround(orbitCenter, orbitAxis, rotationSpeed *  Input.GetAxis("Mouse X") * Time.deltaTime);
        }



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
            Vector3 spawnLocation = hit.point;
            Instantiate(SpawnUnit, spawnLocation, SpawnUnit.transform.rotation);
        }
    }

    private void HandlePosition()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); 
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = new Vector3(-inputX, 0, -inputZ);
        Camera.main.transform.position += movementSpeed * movementDirection * Time.deltaTime;
    }
}
