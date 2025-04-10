using UnityEngine;
using UnityEngine.AI;

public class PrismUnitBehaviour : MonoBehaviour
{
    public float AttackRange = 30;
    public float AttackDamage = 50;
    public float Health = 100;
    public float AttackCooldown = 2;
    public float LaserVisibilityTime = 0.5f;
    public bool IsBase = false;
    [HideInInspector] public Collider Collider;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [SerializeField] GameObject DeathExplosion;
    [SerializeField] GameObject SpawnExplosion;
    [SerializeField] GameObject WayPointObject;
    
    private float timeLastAttack;
    private float timeLastLaser;
    private LineRenderer laser;
    private UnitHealthBar healthBar;
    private int layerMask;

    private AudioSource laserSound;
    private AudioSource spawnSound;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        laser = GetComponent<LineRenderer>();
        healthBar = GetComponentInChildren<UnitHealthBar>();
        
        // Hard coded the order and amount of audio sources in prefab
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if(audioSources.Length == 2)
        {
            spawnSound = audioSources[0];
            laserSound = audioSources[1];
        }
    }

    private void Start()
    {
        timeLastAttack = float.NegativeInfinity;
        timeLastLaser = float.NegativeInfinity;

        // Do collisions with everything but the Prism layer
        layerMask = ~LayerMask.GetMask("Prism");
        
        if(SpawnExplosion != null)
        {
            var spawnExplosion = Instantiate(SpawnExplosion, transform.position, Quaternion.identity);
            Destroy(spawnExplosion, 1);
        }
    }
        
    private void Update()
    {
        // Attempt to hit by checking if enemy is in range
        Hit();

        RemoveOldLasers();
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
                        if(laserSound != null)
                        {
                            laserSound.Play();
                        }
                        break;
                    }
                }
            }
        }
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        healthBar.CurrentHealth = Health;

        if(Health <= 0)
        {
            if(DeathExplosion != null)
            {
                var explosionVfx = Instantiate(DeathExplosion, transform.position, Quaternion.identity);
                Destroy(explosionVfx, 1);
                Destroy(gameObject);
            }
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


    // Moved to ResourceBehaviour
    /*private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Collector"))
        {
            Collection collection = other.GetComponent<Collection>();
            if(curTime >= collection.extractRate)
            {
                EngineScript.curCount += collection.extractAmt;
                curTime = 0f;

            }
            else
            {
                curTime += Time.deltaTime;
            }
        }
    }
    */

    private void RemoveOldLasers()
    {
        float timeSinceLastLaser = Time.time - timeLastLaser;
        if(laser.enabled && (timeSinceLastLaser >= LaserVisibilityTime))
        {
            laser.enabled = false;
        }
    }

}

