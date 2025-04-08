using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrismBase : MonoBehaviour
{
    public GameObject BaseFire;
    public GameObject BaseExplosion;
    [HideInInspector] public PrismUnitBehaviour prismBehavior;

    private AudioSource baseExplosionSound;
    private bool isDead;

    void Start()
    {
        prismBehavior = GetComponentInChildren<PrismUnitBehaviour>();
        baseExplosionSound = GetComponent<AudioSource>();
        isDead = false;
    }

    void Update()
    {
        if (prismBehavior.Health <= 0)
        {
            Debug.Log("dead");
            if(!isDead)
            {
                StartCoroutine(WaitForExplosionAndTransition("DefeatScene", 2));
            }
        }
    }

    // Derived from king pin project
    private IEnumerator WaitForExplosionAndTransition(string sceneName, int seconds)
    {
        isDead = true;
        baseExplosionSound.Play();
        Instantiate(BaseExplosion, transform.position, Quaternion.identity);
        Instantiate(BaseFire, transform.position, BaseFire.transform.rotation);
        yield return new WaitForSeconds(seconds);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
