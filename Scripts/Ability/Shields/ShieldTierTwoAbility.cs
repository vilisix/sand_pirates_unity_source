using UnityEngine;

public class ShieldTierTwoAbility : IAbility, ISecondary, IShield
{
    public AbilityData Data { get; set; }

    public ShieldTierTwoAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Shields/ShieldTierTwo");
    }

    public IAbility Add(IAbility ability)
    {
        if (ability is ShieldTierOneAbility) return new ShieldTierThreeAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Shield tier 2 launched!");

        ShieldFactory.CreateShieldTierTwo(position, Data);
    }
}
