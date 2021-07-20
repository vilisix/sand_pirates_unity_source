using UnityEngine;

public class GrapplingHookAbility : IAbility, IPrimary
{
    public AbilityData Data { get; set; }

    public GrapplingHookAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Projectiles/GrapplingHook");
    }
    public IAbility Add(IAbility ability)
    {
        if (ability is CannonballShotAbility) return new SeaMineShotAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Hook fired!");

        AmmoFactory.CreateHookShot(position, Data);
        ParticleFactory.CreateShotSmoke(position);
    }
}
