using UnityEngine;
using UnityEngine.AI;

public class PrismUnitBehaviour : MonoBehaviour
{
    public float AttackRange = 30;
    public float AttackDamage = 50;
    public float Health = 100;
    public float attackCooldown = 2;
    public float laserVisibilityTime = 0.5f;

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
        // This is super ugly lots of nest ifs, but I'm going fast. I can refactor later

        // Check if we can attack or on cooldown
        float timeSinceLastAttack = Time.time - timeLastAttack;
        if(timeSinceLastAttack < attackCooldown) return;

        // Hit the first human within a certain radius
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider hit in hits)
        {
            // Is the collider from a human?
            if(hit.gameObject.TryGetComponent<HumanUnitBehaviour>(out HumanUnitBehaviour human))
            {
                // Can we actually hit that human from our position by drawing a straight line?
                if(Physics.Linecast(transform.position, human.transform.position, out RaycastHit lineHit))
                {
                    Debug.Log(lineHit.collider.gameObject);
                    // Is what we got from ray casting the straight line the human target or a wall?
                    if(lineHit.collider.gameObject == human.gameObject)
                    {
                        // Shoot Laser Logic
                        DrawLaser(human.transform.position);
                        timeLastAttack = Time.time;
                        human.OnHit(AttackDamage);
                        break;
                    }
                    else{
                        Debug.Log("false");
                    }
                }
            }
        }
    }

    public void OnHit(float damage)
    {
        Debug.Log("Prism Health" + Health);
        Health -= damage;
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
