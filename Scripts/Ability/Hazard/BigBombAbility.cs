using UnityEngine;

public class BigBombAbility : IAbility, ISecondary, IHazard
{
    public AbilityData Data { get; set; }

    public BigBombAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Hazard/BigBomb");
    }

    public IAbility Add(IAbility ability)
    {
        if (ability is OilAbility) return this;
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Big Bomb launched!");
        HazardFactory.CreateBigBombHazard(position, Data);
    }
}
