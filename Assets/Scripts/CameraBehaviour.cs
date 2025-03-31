using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject followObject;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        // Ignore y coordinates
        Vector3 target = GetMouseOn3DWorld();
        target = new Vector3(target.x, transform.position.y, target.z);

        transform.position = Vector3.MoveTowards(transform.position, target, 10);
    }

    private Vector3 GetMouseOn3DWorld()
    {
        //https://stackoverflow.com/questions/72975015/how-do-i-get-mouse-world-position-x-y-plane-only-in-unity
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Debug.Log(worldPosition);
        return worldPosition;
    }

    private void MoveTo(Vector3 position)
    {
        
    }
}
