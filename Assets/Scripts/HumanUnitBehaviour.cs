using UnityEngine;
using UnityEngine.AI;

public class HumanUnitBehaviour : MonoBehaviour
{
    public float AttackRange = 2;
    public float AttackDamage = 10;
    public float Health = 100;
    public float attackCooldown = 2;
    
    [SerializeField] GameObject DeathExplosion;

    private NavMeshAgent navMeshAgent;
    private float timeLastAttack;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(Vector3.zero);
        timeLastAttack = float.NegativeInfinity;
    }

    private void Update()
    {
        Hit();
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
        Health -= damage;
        if(Health <= 0)
        {
            var explosionVfx = Instantiate(DeathExplosion, gameObject.transform.position, Quaternion.identity);
            Destroy(explosionVfx, 3);
            Destroy(gameObject);
        }
    }
}
