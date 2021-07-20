using UnityEngine;

public class OilAbility : IAbility, ISecondary, IHazard
{
    public AbilityData Data { get; set; }

    public OilAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Hazard/Oil");
    }

    public IAbility Add(IAbility ability)
    {
        //if (ability is OilAbility) return new SpikesAbility();

        if (ability is OilAbility) return new LandMineAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Oil Bomb launched!");
        HazardFactory.CreateOilHazard(position, Data);
    }
}
