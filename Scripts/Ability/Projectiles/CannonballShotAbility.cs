using UnityEngine;

public class CannonballShotAbility : IAbility,IPrimary
{
    public AbilityData Data { get; set; }

    public CannonballShotAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Projectiles/Cannonball");
    }

    public IAbility Add(IAbility ability)
    {
        //if (ability is CannonballShotAbility) return new GrapplingHookAbility();
        //else return ability;

        if (ability is CannonballShotAbility) return new SeaMineShotAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Cannonball fired!");

        AmmoFactory.CreateCannonballShot(position, Data);
        ParticleFactory.CreateShotSmoke(position);
    }
}
