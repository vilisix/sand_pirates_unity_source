using UnityEngine;

public class SpikesAbility : IAbility, ISecondary, IHazard
{
    public AbilityData Data { get; set; }

    public SpikesAbility()
    {
        Data = Resources.Load<AbilityData>("AbilityData/Hazard/Spike");
    }

    public IAbility Add(IAbility ability)
    {
        if (ability is OilAbility) return new LandMineAbility();
        else return ability;
    }

    public void Execute(Transform position)
    {
        Debug.Log("Spikes launched!");
        HazardFactory.CreateSpikesHazard(position, Data);
    }
}
