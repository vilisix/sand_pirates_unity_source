using UnityEngine;

public class SeaMineShotAbility : IAbility,IPrimary
{
    public AbilityData Data { get; set; }

    public SeaMineShotAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Projectiles/SeaMine");
    }
    public IAbility Add(IAbility ability)
    {
        if (ability is CannonballShotAbility) return new GatlingShotAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Sea mine fired!");

        AmmoFactory.CreateSeaMineShot(position, Data);
        ParticleFactory.CreateShotSmoke(position);
    }
}