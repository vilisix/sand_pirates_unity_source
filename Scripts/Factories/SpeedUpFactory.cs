using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpFactory
{
    // Первый
    public static void CreateSmallBoost(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        SpeedUpSmallBoostModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<SpeedUpSmallBoostModelView>();
        modelView.Duration = data.SpeedUpDuration;
        modelView.Intensity = data.SpeedUpIntensity;
        modelView.MaxSpeed = data.SpeedUpMaxSpeed;
    }

    // Второй
    public static void CreateBigBoost(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        SpeedUpBigBoostModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<SpeedUpBigBoostModelView>();
        modelView.Duration = data.SpeedUpDuration;
        modelView.Intensity = data.SpeedUpIntensity;
        modelView.MaxSpeed = data.SpeedUpMaxSpeed;
    }

    // Третий
    public static void CreateFlyingBoost(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        SpeedUpFlyingBoostModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<SpeedUpFlyingBoostModelView>();
        modelView.Duration = data.SpeedUpDuration;
        modelView.Intensity = data.SpeedUpIntensity;
        modelView.MaxSpeed = data.SpeedUpMaxSpeed;
    }

    // Четвертый
    public static void CreateSuperMegaWTFSpeed(Transform origin, AbilityData data)
    {
        GameObject shield = data.Prefab;
        SuperMegaWTFSpeedModelView modelView = UnityEngine.Object.Instantiate(shield, origin.position, origin.rotation, origin.transform).GetComponent<SuperMegaWTFSpeedModelView>();
        modelView.Duration = data.SpeedUpDuration;
        modelView.Intensity = data.SpeedUpIntensity;
        modelView.MaxSpeed = data.SpeedUpMaxSpeed;
    }
}
