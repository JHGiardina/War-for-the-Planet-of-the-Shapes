using UnityEngine;

public class ResourceBehaviour : MonoBehaviour
{
    public int extractAmt = 10;
    public float extractRate = 2;

    private float timeLastCollection;
    private AudioSource collectionSound;

    private void Awake()
    {
        timeLastCollection = Time.time;
        collectionSound = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider collider)
    {
        float timeSinceLastCollection = Time.time - timeLastCollection;
        if(timeSinceLastCollection < extractRate) return;

        if(collider.gameObject.tag == "Prism")
        {
            // Collection Logic
            timeLastCollection = Time.time;
            collectionSound.Play();
            EngineScript.curCount += extractAmt;
        }
    }
}
    
