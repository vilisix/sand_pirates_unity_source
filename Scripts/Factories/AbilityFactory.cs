using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class AbilityFactory
{
    /// <summary>
    /// возвращает рандомную способность первого уровня
    /// </summary>
    /// <returns></returns>
    public static IAbility CreateRandomAbility()
    {
        IAbility ability = null;

        AbilityDropChance table = Resources.Load<AbilityDropChance>("AbilityData/ChanceTable");

        int currentRoll = Random.Range(1, table.HazardChance);

        if (currentRoll <= table.ProjectileChance)
        {
            if (table.ProjectileChance != 0)
                ability = CreateShootingAbility();
        }

        else if (currentRoll <= table.SpeedUpChance)
        {
            if (table.SpeedUpChance != 0)
                ability = CreateSpeedBoostAbility();
        }

        else if (currentRoll <= table.ShieldChance)
        {
            if (table.ShieldChance != 0)
                ability = CreateShieldAbility();
        }

        else if (currentRoll <= table.HazardChance)
        {
            if (table.HazardChance != 0)
                ability = CreateHazardAbility();
        }


        return ability;

        //IAbility ability = null;
        //switch (Random.Range(0, 4))
        //{
        //    case 0: ability = CreateShootingAbility(); break;
        //    case 1: ability = CreateSpeedBoostAbility(); break;
        //    case 2: ability = CreateShieldAbility(); break;
        //    case 3: ability = CreateHazardAbility(); break;
        //    default: ability = CreateShootingAbility(); break;
        //}
        //return ability;
    }

    /// <summary>
    /// возвращает стрелковую способность первого уровня
    /// </summary>
    /// <returns></returns>
    public static IAbility CreateShootingAbility()
    {
        return new CannonballShotAbility();
    }

    /// <summary>
    /// возвращает способность ускорения первого уровня
    /// </summary>
    /// <returns></returns>
    public static IAbility CreateSpeedBoostAbility()
    {
        return new SpeedSmallBoostAbility();
    }

    /// <summary>
    /// возвращает способность щита первого уровня
    /// </summary>
    /// <returns></returns>
    public static IAbility CreateShieldAbility()
    {
        return new ShieldTierOneAbility();
    }

    /// <summary>
    /// возвращает опасную способность первого уровня
    /// </summary>
    /// <returns></returns>
    public static IAbility CreateHazardAbility()
    {
        return new OilAbility();
    }
}
