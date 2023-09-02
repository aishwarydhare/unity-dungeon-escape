using UnityEngine;

public class MossGiant : Enemy, IDamageable
{
    public int Health { get; set; }

    public override void Init()
    {
        base.Init();
        Health = base.health;
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
}
