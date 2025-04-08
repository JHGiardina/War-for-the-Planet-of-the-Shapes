using UnityEngine;

using System.Collections;

public class CollectionSpawning : MonoBehaviour
{
    public GameObject prefab;

    public Transform[] a_spawnLoc;
    public Transform self;

    public int numCollectors;
    
    [Header("Modifer Settings")]
    public float modMin;
    public float modMax;
    [Range(0,1)] public float delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(a_spawnLoc.Length >= numCollectors)
        {
            StartCoroutine(SpawnCollectors());
        }else
        {
            Debug.LogError("Trying to spawn too many collectors!");
        }     
    }

    private IEnumerator SpawnCollectors()
    {
       for (int i = 0; i <= numCollectors; i++ )
            {
                int modifier = Mathf.RoundToInt(Random.Range(modMin, modMax));
                int index = Random.Range(0, a_spawnLoc.Length);
                index =+ modifier;
                index = Mathf.Clamp(index, 0, a_spawnLoc.Length);
                GameObject newCollector = Instantiate(prefab, a_spawnLoc[index].position, a_spawnLoc[index].rotation);
                newCollector.transform.SetParent(self, worldPositionStays: true);
                yield return new WaitForSeconds(delay);
            }  
    }
}


