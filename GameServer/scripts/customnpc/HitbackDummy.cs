using System;

namespace DOL.GS;

public class HitbackDummy : GameTrainingDummy
{
    private int Damage = 0;
    private DateTime StartTime;
    private TimeSpan TimePassed;
    private bool StartCheck = true;

    public override short MaxSpeedBase => 0;

    public override bool FixedSpeed => true;

    public override ushort Heading
    {
        get => base.Heading;
        set => base.Heading = SpawnHeading;
    }

    public override bool Interact(GamePlayer player)
    {
        if (!base.Interact(player)) return false;

        Damage = 0;
        StartCheck = true;
        StopAttack();
        return true;
    }

    public override void OnAttackedByEnemy(AttackData ad)
    {
        if (StartCheck)
        {
            StartTime = DateTime.Now;
            StartCheck = false;
        }

        Damage += ad.Damage + ad.CriticalDamage;
        TimePassed = DateTime.Now - StartTime;

        if (!attackComponent.AttackState)
            attackComponent.RequestStartAttack(ad.Attacker);
    }

    public override bool AddToWorld()
    {
        Name = "Hitback Dummy - Right Click to Reset";
        GuildName = "Atlas Dummy Union";
        Model = 34;
        Strength = 10;
        ScalingFactor = 4;
        return base.AddToWorld(); // Finish up and add him to the world.
    }
}