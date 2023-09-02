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
        SpiderCombatAndAttackCheck();
        if (shouldAttack && !attackBreak && !hit && !dead) Attack();
    }

    private void SpiderCombatAndAttackCheck()
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

    public void Damage(int attackPower)
    {
        if (hit || dead) return;

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
        acidObj.transform.SetParent(gameObject.transform);
        StartCoroutine(WaitAndDestroyAcidPrefab());
    }

    private IEnumerator WaitAndDestroyAcidPrefab()
    {
        yield return new WaitForSeconds(5);
        Destroy(acidObj);
        StopCoroutine(WaitAndDestroyAcidPrefab());
    }
}
