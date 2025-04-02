using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    void Update()
    {
        // Rotate
        transform.RotateAround(transform.position, new Vector3(0,1,0), 90 * Time.deltaTime);

        // Move Up and Down
        transform.position += new Vector3(0,1,0) * Mathf.Sin(Time.deltaTime);
    }
}
