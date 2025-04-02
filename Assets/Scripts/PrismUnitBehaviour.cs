using UnityEngine;
using UnityEngine.AI;

public class PrismUnitBehaviour : MonoBehaviour
{
    public float AttackRange = 30;
    public float AttackDamage = 50;
    public float Health = 100;
    public float attackCooldown = 2;
    public float laserVisibilityTime = 0.1f;

    private NavMeshAgent navMeshAgent;
    private float timeLastAttack;
    private float timeLastLaser;
    private LineRenderer laser;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(Vector3.zero);
        laser = GetComponent<LineRenderer>();
        timeLastAttack = float.NegativeInfinity;
        timeLastLaser = float.NegativeInfinity;
    }
        
    private void Update()
    {
        // Attempt to hit by checking if enemy is in range
        Hit();

        // Remove any old lasers
        float timeSinceLastLaser = Time.time - timeLastLaser;
        if(laser.enabled && (timeSinceLastLaser >= laserVisibilityTime))
        {
            laser.enabled = false;
        }
    }
    
    public void Hit()
    {
        // Check if we can attack or on cooldown
        float timeSinceLastAttack = Time.time - timeLastAttack;
        if(timeSinceLastAttack < attackCooldown) return;

        // Hit the first human within a certain radius
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider hit in hits)
        {
            HumanUnitBehaviour human;
            if(hit.gameObject.TryGetComponent<HumanUnitBehaviour>(out human))
            {
                DrawLaser(human.transform.position);
                timeLastAttack = Time.time;
                human.OnHit(AttackDamage);
                break;
            }
        }
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        Debug.Log("hit");
        Debug.Log(Health);
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DrawLaser(Vector3 target)
    {
        timeLastLaser = Time.time;
        laser.enabled = true;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, target);
    }

}
