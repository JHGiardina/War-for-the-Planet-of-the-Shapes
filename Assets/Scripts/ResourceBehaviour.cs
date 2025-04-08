using UnityEngine;

public class ResourceBehaviour : MonoBehaviour
{
    public int extractAmt = 10;
    public float extractRate = 2;

    private float timeLastCollection;

    private void Awake()
    {
        timeLastCollection = Time.time;
    }

    private void OnTriggerStay(Collider collider)
    {
        float timeSinceLastCollection = Time.time - timeLastCollection;
        if(timeSinceLastCollection < extractRate) return;

        if(collider.CompareTag("Prism"))
        {
            EngineScript.curCount += extractAmt;
        }
    }
}
    
