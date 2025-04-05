using UnityEngine;
using UnityEngine.AI;

// Basic implementations so the user only needs to implement the hit method

public abstract class BaseHumanUnitBehaviour : MonoBehaviour
{
    public float Health = 100;
    public GameObject DeathExplosion;
    [HideInInspector] public float Speed;
    [HideInInspector] public Collider Collider;

    protected NavMeshAgent navMeshAgent;
    protected GameObject prismTarget;
    protected Animator animator;

    private string PRISM_TAG = "Prism";

    // Just implement this to create your unit
    public abstract void Hit();

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(Vector3.zero);
        animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
    }

    public virtual void Start()
    {
        // Expose Speed for animations
        Speed = navMeshAgent.velocity.magnitude;
        
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public virtual void Update()
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

        if(Speed >= 0 && animator != null)
        {
            animator.SetBool("Run", true);
        }
    }

    public void OnHit(float damage)
    {
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
