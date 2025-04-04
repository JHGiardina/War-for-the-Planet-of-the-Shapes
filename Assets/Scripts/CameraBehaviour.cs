using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject SpawnUnit;
    private Vector3 orbitCenter;
    private Vector3 orbitAxis;
    private float rotationSpeedMouse;
    private float zoomSpeedMouse;
    private float rotationSpeedKeys;

    void Start()
    {
        orbitCenter = Vector3.zero;
        // Rotate around y-axis
        orbitAxis = new Vector3(0, 1, 0);

        // Arbitrary speeds based on testing
        rotationSpeedKeys = 100;
        rotationSpeedMouse = 1000;
        zoomSpeedMouse = 30;
    }

    void Update()
    {
        // I could use input actions instead of Unity's old input system, but someone else can pick up that task

        // Zoom
        Vector3 cameraToOrign = orbitCenter - Camera.main.transform.position;
        Camera.main.transform.position += zoomSpeedMouse * cameraToOrign * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;

        // Rotation around World Origin with middle mouse
        if (Input.GetMouseButton(2))
        {
            Camera.main.transform.RotateAround(orbitCenter, orbitAxis, rotationSpeedMouse *  Input.GetAxis("Mouse X") * Time.deltaTime);
        }

        // Rotation around World Origin with wasd and arrow keys
        Camera.main.transform.RotateAround(orbitCenter, orbitAxis, rotationSpeedKeys *  Input.GetAxis("Horizontal") * Time.deltaTime);


        // Spawn Units
        if (Input.GetMouseButtonDown(0) /*&& EngineScript.curCount >= 15*/)
        {
            SpawnAtRayHit();
            //EngineScript.curCount -= 15;
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
