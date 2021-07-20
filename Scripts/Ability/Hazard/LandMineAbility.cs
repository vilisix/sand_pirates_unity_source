using UnityEngine;

public class LandMineAbility : IAbility, ISecondary, IHazard
{
    public AbilityData Data { get; set; }

    public LandMineAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Hazard/LandMine");
    }

    public IAbility Add(IAbility ability)
    {
        if (ability is OilAbility) return new BigBombAbility();
        else return ability;
    }


    public void Execute(Transform position)
    {
        Debug.Log("Land Mine launched!");
        HazardFactory.CreateLandMineHazard(position, Data);
    }
}
