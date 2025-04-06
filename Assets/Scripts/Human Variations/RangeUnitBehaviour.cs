using Unity.Mathematics;
using UnityEngine;

// Haven't tested it yet

public class RangeUnitBehaviour : BaseHumanUnitBehaviour
{
    public float AttackRange = 2;
    public float AttackDamage = 10;
    public float AttackCooldown = 2;
    public float LaserVisibilityTime = 0.5f;
    public float BaseSpeed = 3.5f;

    private float timeLastAttack;
    private float timeLastLaser;
    private LineRenderer laser;

    private AudioSource laserSound;

    // Layer mask to ignore self collisions
    private int layerMask;

    public override void Start()
    {
        base.Start();

        navMeshAgent.speed = BaseSpeed;
        laser = GetComponent<LineRenderer>();
        timeLastLaser = float.NegativeInfinity;
        timeLastAttack = float.NegativeInfinity;

        // Do collisions with everything but the Human layer
        layerMask = ~LayerMask.GetMask("Human");

        // Hard coded the order and amount of audio sources in prefab
        AudioSource[] audioSources = GetComponents<AudioSource>();
        laserSound = audioSources[0];
    }

    public override void Update()
    {
        base.Update();

        // Remove any old lasers
        float timeSinceLastLaser = Time.time - timeLastLaser;
        if(laser.enabled && (timeSinceLastLaser >= LaserVisibilityTime))
        {
            laser.enabled = false;
        }
    }

    public override void Hit()
    {
        // FYI lasers are shot from collider center to avoid colliding with the floor

        // Check if we can attack or on cooldown
        float timeSinceLastAttack = Time.time - timeLastAttack;
        if(timeSinceLastAttack < AttackCooldown) return;

        // Hit the first prism within a certain radius
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider hit in hits)
        {
            // Is the collider from a prism?
            if(hit.gameObject.TryGetComponent<PrismUnitBehaviour>(out PrismUnitBehaviour prism))
            {

                // Can we actually hit that prism from our position by drawing a straight line?
                if(Physics.Linecast(Collider.bounds.center, prism.Collider.bounds.center, out RaycastHit lineHit, layerMask))
                {
                    // Is what we got from ray casting the straight line the prism target or a wall?
                    if(lineHit.collider.gameObject == prism.gameObject)
                    {
                        // Shoot Laser Logic
                        Vector3 targetPosiion = lineHit.collider.bounds.center;
                        DrawLaser(targetPosiion);
                        timeLastAttack = Time.time;
                        prism.OnHit(AttackDamage);
                        laserSound.Play();
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
        Vector3 laserBeginPosition = Collider.bounds.center;
        laser.SetPosition(0, laserBeginPosition);
        laser.SetPosition(1, target);
    }
}