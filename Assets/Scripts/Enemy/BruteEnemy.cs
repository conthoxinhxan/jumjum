public class BruteEnemy : CreepEnemy
{
    public override void Awake()
    {
        base.Awake();
        MoveSpeed *= 0.65f;
        AttackRange *= 1.25f;
        AttackCooldown *= 1.5f;
        MeleeDamage *= 2;
    }
}
