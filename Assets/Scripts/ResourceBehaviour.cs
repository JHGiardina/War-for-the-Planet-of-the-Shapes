using UnityEngine;

public class ResourceBehaviour : MonoBehaviour
{
    public int TotalExtractAmt = 30;
    public int ExtractAmt = 5;
    public float ExtractRate = 2;
    public GameObject DeathExplosion;

    private float timeLastCollection;
    private int extractAmtLeft;
    private AudioSource collectionSound;
    private Material material;

    private void Awake()
    {
        timeLastCollection = Time.time;
        collectionSound = GetComponent<AudioSource>();
        material = GetComponent<Renderer>().material;
        extractAmtLeft = TotalExtractAmt;
    }

    private void OnTriggerStay(Collider collider)
    {
        float timeSinceLastCollection = Time.time - timeLastCollection;
        if(timeSinceLastCollection < ExtractRate) return;

        if(collider.gameObject.tag == "Prism")
        {
            // Collection Logic
            timeLastCollection = Time.time;
            EngineScript.curCount += ExtractAmt;
            extractAmtLeft -= ExtractAmt;

            // Destroy used up Resources
            if(extractAmtLeft <= 0)
            {
                var explosionVfx = Instantiate(DeathExplosion, transform.position, Quaternion.identity);
                Destroy(explosionVfx, 1);
                Destroy(gameObject);
            }
            else
            {
                collectionSound.Play();

                // As Resources is collected resource becomes less visible
                SetTransparency();
            }

        }
    }

    private void SetTransparency()
    {
        // Bruh why does c# perform integer division then cast it to float
        float alpha = (float) extractAmtLeft / TotalExtractAmt;
        Debug.Log(extractAmtLeft);
        Debug.Log(TotalExtractAmt);
        Debug.Log("alpha " + alpha); 

        Color previousColor = material.color;
        material.color = new Color(previousColor.r, previousColor.g, previousColor.b, alpha);
    }
}
    
