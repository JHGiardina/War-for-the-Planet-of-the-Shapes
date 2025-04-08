using UnityEngine;

using System.Collections;

public class CollectionSpawning : MonoBehaviour
{
    public int numCollectors;
    public GameObject prefab;

    public Transform[] a_spawnLoc;

    private AudioSource collectionSound;
    
    // Just Going to Remove this for now
    //[Header("Modifer Settings")]
    //public float modMin;
    //public float modMax;
    
    [Range(0,1)] public float delay;

    private void Awake()
    {
        collectionSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(a_spawnLoc.Length >= numCollectors)
        {
            StartCoroutine(SpawnCollectors());
        }
        else
        {
            Debug.LogError("Trying to spawn too many collectors!");
        }     
    }

    private IEnumerator SpawnCollectors()
    {
        for (int i = 0; i <= numCollectors; i++)
        {
            int index = Random.Range(0, a_spawnLoc.Length);
            GameObject newCollector = Instantiate(prefab, a_spawnLoc[index].position, a_spawnLoc[index].rotation);
            newCollector.transform.SetParent(transform, worldPositionStays: true);
            yield return new WaitForSeconds(delay);
        }  
    }
}


