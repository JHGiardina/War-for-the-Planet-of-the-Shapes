using UnityEngine;
using UnityEngine.AI;

public class HumanUnitBehaviour : MonoBehaviour
{
    public float AttackRange = 2;
    public float AttackDamage = 10;
    public float Health = 100;
    public float attackCooldown = 2;
    [HideInInspector] public float speed;
    
    [SerializeField] GameObject DeathExplosion;

    private NavMeshAgent navMeshAgent;
    private GameObject prismTarget;
    private float timeLastAttack;

    private string PRISM_TAG = "Prism";

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(Vector3.zero);
        timeLastAttack = float.NegativeInfinity;
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

        // Expose speed for animations
        speed = navMeshAgent.speed;
    }

    public void Hit()
    {
        // Check if we can attack or on cooldown
        float timeSinceLastAttack = Time.time - timeLastAttack;
        if(timeSinceLastAttack < attackCooldown) return;

        // Hit the first prism within a certain radius
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider hit in hits)
        {
            //Debug.Log(hit);
            PrismUnitBehaviour prism;
            Debug.Log(hit.gameObject);
            if(hit.gameObject.TryGetComponent<PrismUnitBehaviour>(out prism))
            {
                Debug.Log(hit.gameObject);
                prism.OnHit(AttackDamage);
                break;
            }
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
