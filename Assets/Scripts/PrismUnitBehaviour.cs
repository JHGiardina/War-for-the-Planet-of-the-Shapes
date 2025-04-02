using UnityEngine;
using UnityEngine.AI;

public class PrismUnitBehaviour : MonoBehaviour
{
    public float AttackRange = 30;
    public float AttackDamage = 50;
    public float Health = 100;
    public float attackCooldown = 2;
    public float laserVisibilityTime = 0.5f;

    [SerializeField] GameObject DeathExplosion;

    private NavMeshAgent navMeshAgent;
    private float timeLastAttack;
    private float timeLastLaser;
    private LineRenderer laser;
    private GameObject humanTarget;

    private string HUMAN_TAG = "Human";

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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

        // Get a human target if we don't have one else move towards human target
        if(humanTarget == null)
        {
            FindHumanTarget();
        }
        else
        {
            navMeshAgent.SetDestination(humanTarget.transform.position);
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
            var explosionVfx = Instantiate(DeathExplosion, transform.position, Quaternion.identity);
            Destroy(explosionVfx, 3);
            Destroy(gameObject);
        }
    }

    // Set target to be a random human
    public void FindHumanTarget()
    {
        GameObject[] humans = GameObject.FindGameObjectsWithTag(HUMAN_TAG);

        if(humans.Length > 0)
        {
            int randomIndex = Random.Range(0, humans.Length);
            humanTarget = humans[randomIndex];
        }
    }

    private void DrawLaser(Vector3 target)
    {
        timeLastLaser = Time.time;
        laser.enabled = true;
        Vector3 laserBeginPosition = transform.position;
        laser.SetPosition(0, laserBeginPosition);
        laser.SetPosition(1, target);
    }

}
