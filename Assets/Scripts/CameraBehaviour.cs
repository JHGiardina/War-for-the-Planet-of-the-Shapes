using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Vector3 target;

    void Start()
    {
        // Don't move the camera anywhere on start
        target = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 100);
    }

    void LateUpdate()
    {
        // Yes I could use input actions but this works for now
        if(Input.GetMouseButtonDown(0))
        {
            target = GetMouseTargetPosition();
        }
    }

    private Vector3 GetMouseTargetPosition()
    {
        //https://stackoverflow.com/questions/72975015/how-do-i-get-mouse-world-position-x-y-plane-only-in-unity
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        target = new Vector3(target.x, transform.position.y, target.z);
        return target;
    }
}
