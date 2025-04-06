using UnityEngine;

// Haven't tested yet

public class MeleeUnitBehaviour : BaseHumanUnitBehaviour
{
    public float AttackRange = 2;
    public float AttackDamage = 10;
    public float attackCooldown = 2;
    public float BaseSpeed = 20f;

    private float timeLastAttack;

    public override void Start()
    {
        base.Start();

        navMeshAgent.speed = BaseSpeed;
        timeLastAttack = float.NegativeInfinity;

        // Hard coded the order and amount of audio sources in prefab
        AudioSource[] audioSources = GetComponents<AudioSource>();
    }

    public override void Hit()
    {
        // Check if we can attack or on cooldown
        float timeSinceLastAttack = Time.time - timeLastAttack;
        if(timeSinceLastAttack < attackCooldown) return;

        // Hit the first prism within a certain radius
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider hit in hits)
        {
            PrismUnitBehaviour prism;
            if(hit.gameObject.TryGetComponent<PrismUnitBehaviour>(out prism))
            {
                prism.OnHit(AttackDamage);
                break;
            }
        }
    }
}