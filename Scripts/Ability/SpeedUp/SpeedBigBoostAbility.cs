using UnityEngine;

public class SpeedBigBoostAbility : IAbility, ISecondary, ISpeedUp
{
    public AbilityData Data { get; set; }

    public SpeedBigBoostAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/SpeedUp/SpeedBigBoost");
    }
    public IAbility Add(IAbility ability)
    {
        if (ability is SpeedSmallBoostAbility) return new SpeedFlyingBoostAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Big Speed boost launched!");

        SpeedUpFactory.CreateBigBoost(position, Data);
    }
}