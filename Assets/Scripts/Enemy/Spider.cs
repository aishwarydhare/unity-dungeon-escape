using System.Collections;
using UnityEngine;

public class Spider : Enemy, IDamageable
{

    [SerializeField]
    private GameObject acidPrefab;
    private GameObject acidObj;
        
    public int Health { get; set; }

    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    protected override void Update()
    {
        // do nothing
        CombatAndAttackCheck();
        if (shouldAttack && !attackBreak) Attack();
    }

    protected override void CombatAndAttackCheck()
    {
        inCombat = false;
        shouldAttack = false;
        float distanceFromPlayer = Vector3.Distance(transform.position, playerObj.transform.position);
        if (distanceFromPlayer <= combatDistanceTrigger)
        {
            inCombat = true;
            shouldAttack = true;
        }
    }


    protected override void Attack()
    {
        base.Attack();
    }

    public void Damage(int attackPower)
    {
        Health -= attackPower;
        if (Health <= 0)
        {
            StartCoroutine(DieWaitThenDestroy());
        }
        else
        {
            StartCoroutine(HitWaitThenContinue());
        }
    }

    public void FireAcidProjectile()
    {
        acidObj = Instantiate(acidPrefab, transform.position, Quaternion.identity);
        StartCoroutine(WaitAndDestroyPrefab());
    }

    private IEnumerator WaitAndDestroyPrefab()
    {
        yield return new WaitForSeconds(5);
        Destroy(acidObj);
        StopCoroutine(WaitAndDestroyPrefab());
    }
}
