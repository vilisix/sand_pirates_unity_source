using UnityEngine;

public class SuperMegaWTFSpeedAbility : IAbility, ISecondary, ISpeedUp
{
    public AbilityData Data { get; set; }

    public SuperMegaWTFSpeedAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/SpeedUp/SuperMegaWTFSpeed");
    }
    public IAbility Add(IAbility ability)
    {
        if (ability is SpeedSmallBoostAbility) return this;
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("SUPER MEGA SPEED BOOST LAUNCHED!");

        SpeedUpFactory.CreateSuperMegaWTFSpeed(position, Data);
    }
}