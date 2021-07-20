using UnityEngine;

public class ShieldTierThreeAbility : IAbility, ISecondary, IShield
{
    public AbilityData Data { get; set; }

    public ShieldTierThreeAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Shields/ShieldTierThree");
    }

    public IAbility Add(IAbility ability)
    {
        //if (ability is ShieldTierOneAbility) return new ShieldTierFourAbility();

        if (ability is ShieldTierOneAbility) return this;
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Shield tier 3 launched!");

        ShieldFactory.CreateShieldTierThree(position, Data);
    }
}
