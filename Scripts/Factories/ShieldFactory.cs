using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFactory
{
    // Первый
    public static void CreateShieldTierOne(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        ShieldTierOneModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<ShieldTierOneModelView>();
        modelView.Duration = data.ShieldDuration;
        modelView.Durability = data.ShieldDurability;
    }

    // Второй
    public static void CreateShieldTierTwo(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        ShieldTierTwoModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<ShieldTierTwoModelView>();
        modelView.Duration = data.ShieldDuration;
        modelView.Durability = data.ShieldDurability;
    }

    // Третий
    public static void CreateShieldTierThree(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        ShieldTierThreeModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<ShieldTierThreeModelView>();
        modelView.Duration = data.ShieldDuration;
        modelView.Durability = data.ShieldDurability;
    }

    // Четвертый
    public static void CreateShieldTierFour(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        ShieldTierFourModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<ShieldTierFourModelView>();
        modelView.Duration = data.ShieldDuration;
        modelView.Durability = data.ShieldDurability;
    }
}
