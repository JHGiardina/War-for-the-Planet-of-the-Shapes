using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject SpawnUnit;
    private Vector3 orbitCenter;
    private Vector3 orbitAxis;
    private float rotationSpeed;
    private float zoomSpeed;

    void Start()
    {
        orbitCenter = Vector3.zero;
        // Rotate around y-axis
        orbitAxis = new Vector3(0, 1, 0);
        rotationSpeed = 1000;
        zoomSpeed = 30;
    }

    void Update()
    {
        // Zoom
        Vector3 cameraToOrign = orbitCenter - Camera.main.transform.position;
        Camera.main.transform.position += zoomSpeed * cameraToOrign * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;

        // Rotation around World Origin
        if (Input.GetMouseButton(1))
        {
            Camera.main.transform.RotateAround(orbitCenter, orbitAxis, rotationSpeed *  Input.GetAxis("Mouse X") * Time.deltaTime);
        }

        // Spawn Units
        if (Input.GetMouseButtonDown(0))
        {
            SpawnAtRayHit();
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
}
