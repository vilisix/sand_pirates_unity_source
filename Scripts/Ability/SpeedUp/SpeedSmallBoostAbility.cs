using UnityEngine;

public class SpeedSmallBoostAbility : IAbility, ISecondary, ISpeedUp
{
    public AbilityData Data { get; set; }

    public Rigidbody connection;

    public SpeedSmallBoostAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/SpeedUp/SpeedSmallBoost");
    }
    public IAbility Add(IAbility ability)
    {
        if (ability is SpeedSmallBoostAbility) return new SpeedBigBoostAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Small speed Boost launched!");

        SpeedUpFactory.CreateSmallBoost(position, Data);
    }
}
