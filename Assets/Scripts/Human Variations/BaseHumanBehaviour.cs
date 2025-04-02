using UnityEngine;
using UnityEngine.AI;

// Basic implementations so the user only needs to implement the hit method

public abstract class BaseHumanBehaviour : MonoBehaviour
{
    public float Health = 100;
    public GameObject DeathExplosion;
    [HideInInspector] public float speed;

    protected NavMeshAgent navMeshAgent;
    protected GameObject prismTarget;
    protected float timeLastAttack;

    private string PRISM_TAG = "Prism";

    // Just implement this to create your unit
    public abstract void Hit();

    // overridable parameters
    public float baseSpeed = 1;

    public virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(Vector3.zero);
        timeLastAttack = float.NegativeInfinity;

        // Expose speed for animations
        speed = navMeshAgent.velocity.magnitude;
    }

    private void Update()
    {
        // Try a hit within our range (will go through walls but the range is so small this is intended)
        Hit();

         // Get a human target if we don't have one or its dead else move towards human target
        if(prismTarget == null)
        {
            FindPrismTarget();
        }
        else
        {
            navMeshAgent.SetDestination(prismTarget.transform.position);
        }
    }

    public void OnHit(float damage)
    {
        Debug.Log("Human Health" + Health);
        Health -= damage;
        if(Health <= 0)
        {
            var explosionVfx = Instantiate(DeathExplosion, transform.position, Quaternion.identity);
            Destroy(explosionVfx, 3);
            Destroy(gameObject);
        }
    }

    // Set target to be a random human
    public void FindPrismTarget()
    {
        GameObject[] prisms = GameObject.FindGameObjectsWithTag(PRISM_TAG);

        if(prisms.Length > 0)
        {
            int randomIndex = Random.Range(0, prisms.Length);
            prismTarget = prisms[randomIndex];
        }
    }
}
