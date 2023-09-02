public class MossGiant : Enemy, IDamageable
{
    public int Health { get; set; }

    public override void Init()
    {
        base.Init();
        Health = base.health;
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
}
