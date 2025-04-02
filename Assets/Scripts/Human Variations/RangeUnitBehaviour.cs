using UnityEngine;

// Haven't tested it yet

public class RangeUnitBehaviour : BaseHumanBehaviour
{
    public float AttackRange = 2;
    public float AttackDamage = 10;

    public float attackCooldown = 2;
    public float BaseSpeed = 3.5f;

    private float timeLastLaser;
    private LineRenderer laser;

    public override void Start()
    {
        base.Start();

        navMeshAgent.speed = BaseSpeed;
        laser = GetComponent<LineRenderer>();
        timeLastLaser = float.NegativeInfinity;
    }

    public override void Hit()
    {
        // This is super ugly lots of nest ifs, but I'm going fast. I can refactor later

        // Check if we can attack or on cooldown
        float timeSinceLastAttack = Time.time - timeLastAttack;
        if(timeSinceLastAttack < attackCooldown) return;

        // Hit the first prism within a certain radius
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider hit in hits)
        {
            // Is the collider from a prism?
            if(hit.gameObject.TryGetComponent<PrismUnitBehaviour>(out PrismUnitBehaviour prism))
            {
                // Can we actually hit that prism from our position by drawing a straight line?
                if(Physics.Linecast(transform.position, prism.transform.position, out RaycastHit lineHit))
                {
                    Debug.Log(lineHit.collider.gameObject);
                    // Is what we got from ray casting the straight line the prism target or a wall?
                    if(lineHit.collider.gameObject == prism.gameObject)
                    {
                        // Shoot Laser Logic
                        DrawLaser(prism.transform.position);
                        timeLastAttack = Time.time;
                        prism.OnHit(AttackDamage);
                        break;
                    }
                }
            }
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