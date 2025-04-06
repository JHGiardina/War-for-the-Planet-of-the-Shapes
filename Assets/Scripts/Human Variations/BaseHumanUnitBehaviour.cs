using UnityEngine;
using UnityEngine.AI;
using System.Linq;

// Basic implementations so the user only needs to implement the hit method

public abstract class BaseHumanUnitBehaviour : MonoBehaviour
{
    public float Health = 100;
    //public float MinApproachRadius = 0;
    public GameObject DeathExplosion;
    [HideInInspector] public Collider Collider;

    protected NavMeshAgent navMeshAgent;
    protected GameObject prismTarget;
    protected Animator animator;

    private UnitHealthBar healthBar;
    private string PRISM_TAG = "Prism";
    private string BASE_TAG = "PrismBase";

    public float detectionRadius = 10f;
    private GameObject targetBase;

    // Just implement this to create your unit
    public abstract void Hit();

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(Vector3.zero);
        animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
        healthBar = GetComponentInChildren<UnitHealthBar>();
        
        var baseObj = GameObject.FindWithTag(BASE_TAG);
        if (baseObj != null)
        {
            targetBase = baseObj;
        }else
        {
            Debug.LogError($"[{name}] No GameObject with tag '{BASE_TAG}' found in scene!");
        }
    }

    public virtual void Start()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public virtual void Update()
    {
        // Try a hit within our range (will go through walls but the range is so small this is intended)
        Hit();

        //Check radius for target
        Collider[] hits= Physics.OverlapSphere(transform.position, detectionRadius);
        var prismsInRange = hits
            .Where(h => h.CompareTag(PRISM_TAG))
            .Select(h => h.transform.gameObject);

        if (prismsInRange.Any())
        {
            prismTarget = prismsInRange
                .OrderBy(prismsInRange => Vector3.Distance(transform.position, prismsInRange.transform.position))
                .First();
        }else
        {
            prismTarget = targetBase;
        }

        navMeshAgent.SetDestination(prismTarget.transform.position);

         // Get a human target if we don't have one or its dead else move towards human target
        // if(prismTarget == null)
        // {
        //     FindPrismTarget();
        // }
        // else
        // {
        //     // Jittering and glitchy should use raycasts instead 
        //     // Don't let the prism move closer than a specific radius
        //     //Vector3 humanToPrismDirection = (prismTarget.transform.position  - transform.position).normalized;
        //     //Vector3 targetPosition = prismTarget.transform.position - (MinApproachRadius * humanToPrismDirection);
        //     //navMeshAgent.SetDestination(targetPosition);

        //     navMeshAgent.SetDestination(prismTarget.transform.position);
        // }

        //Debug.Log(navMeshAgent.velocity.magnitude);
        if(navMeshAgent.velocity.magnitude >= 0 && animator != null)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        healthBar.CurrentHealth = Health;
        
        if(Health <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        var explosionVfx = Instantiate(DeathExplosion, transform.position, Quaternion.identity);
        Destroy(explosionVfx, 3);
        Destroy(gameObject);
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
