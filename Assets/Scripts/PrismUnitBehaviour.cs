using UnityEngine;
using UnityEngine.AI;

public class PrismUnitBehaviour : MonoBehaviour
{
    public float AttackRange = 30;
    public float AttackDamage = 50;
    public float Health = 100;
    public float AttackCooldown = 2;
    public float LaserVisibilityTime = 0.5f;
    [HideInInspector] public Collider Collider;

    [SerializeField] GameObject DeathExplosion;
    [SerializeField] GameObject WayPointObject;

    private NavMeshAgent navMeshAgent;
    private float timeLastAttack;
    private float timeLastLaser;
    private LineRenderer laser;
    private Vector3 targetPosition;
    private int layerMask;
    private float curTime = 0f;

    //public GameObject gameEngine;
    //private EngineScript engine;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        laser = GetComponent<LineRenderer>();
        //engine = gameEngine.GetComponent<EngineScript>();
    }

    private void Start()
    {
        timeLastAttack = float.NegativeInfinity;
        timeLastLaser = float.NegativeInfinity;

        // Do collisions with everything but the Prism layer
        layerMask = ~LayerMask.GetMask("Prism");
    }
        
    private void Update()
    {
        // Attempt to hit by checking if enemy is in range
        Hit();

        // Remove any old lasers
        float timeSinceLastLaser = Time.time - timeLastLaser;
        if(laser.enabled && (timeSinceLastLaser >= LaserVisibilityTime))
        {
            laser.enabled = false;
        }

        // Move Units towards mouse when 
        if (Input.GetMouseButtonDown(1))
        {
            MoveUnitsTowardsMouseRay();
        }
    }
    
    public void Hit()
    {
        // This is super ugly lots of nest ifs, but I'm going fast. I can refactor later

        // Check if we can attack or on cooldown
        float timeSinceLastAttack = Time.time - timeLastAttack;
        if(timeSinceLastAttack < AttackCooldown) return;

        // Hit the first human within a certain radius
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider hit in hits)
        {
            // Is the Collider from a human?
            if(hit.gameObject.TryGetComponent<BaseHumanUnitBehaviour >(out BaseHumanUnitBehaviour  human))
            {
                // Can we actually hit that human from our position by drawing a straight line?
                if(Physics.Linecast(Collider.bounds.center, human.Collider.bounds.center, out RaycastHit lineHit, layerMask))
                {
        
                    // Is what we got from ray casting the straight line the human target or a wall?
                    if(lineHit.collider.gameObject == human.gameObject)
                    {
                        // Shoot Laser Logic
                       Vector3 targetPosiion = lineHit.collider.bounds.center;

                        DrawLaser(targetPosiion);
                        DrawLaser(human.transform.position);
                        timeLastAttack = Time.time;
                        human.OnHit(AttackDamage);
                        break;
                    }
                }
            }
        }
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            var explosionVfx = Instantiate(DeathExplosion, transform.position, Quaternion.identity);
            Destroy(explosionVfx, 1);
            Destroy(gameObject);
        }
    }

    private void MoveUnitsTowardsMouseRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPosition = hit.point;

            // Destroy previous waypoints
            WaypointBehaviour[] previousWayPoints = Object.FindObjectsByType<WaypointBehaviour>(FindObjectsSortMode.None);
            foreach(WaypointBehaviour previousWayPoint in previousWayPoints)
            {
                Destroy(previousWayPoint.gameObject);
            }

            //Spawn waypoint marker 
            // 3d pivot is placed wrong so I have to translate it up when spawning (I'll fix it in blender later)
            Vector3 waypointPosition = targetPosition + new Vector3(0, 4, 0);
            var waypointObject = Instantiate(WayPointObject, waypointPosition, Quaternion.identity);
            Destroy(waypointObject, 3);
            
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    private void DrawLaser(Vector3 target)
    {
        timeLastLaser = Time.time;
        laser.enabled = true;
        Vector3 laserBeginPosition = Collider.bounds.center;
        laser.SetPosition(0, laserBeginPosition);
        laser.SetPosition(1, target);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Collector"))
        {

            Collection collection = other.GetComponent<Collection>();
            if(curTime >= collection.extractRate)
            {
                EngineScript.curCount += collection.extractAmt;
                curTime = 0f;

            }else
            {
                curTime += Time.deltaTime;
            }
        }
    }



}

