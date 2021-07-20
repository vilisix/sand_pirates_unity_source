using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Drop Chance Table", menuName = "ScriptableObjects/Ability Drop Chance Table", order = 2)]
public class AbilityDropChance : ScriptableObject
{
    [SerializeField] private int projectileAbilityChance = 25;
    [SerializeField] private int speedUpAbilityChance = 25;
    [SerializeField] private int shieldAbilityChance = 25;
    [SerializeField] private int hazardAbilityChance = 25;

    public int ProjectileChance { get => projectileAbilityChance; }
    public int SpeedUpChance { get => projectileAbilityChance + speedUpAbilityChance; }
    public int ShieldChance { get => projectileAbilityChance + speedUpAbilityChance + shieldAbilityChance; }
    public int HazardChance { get => projectileAbilityChance + speedUpAbilityChance + shieldAbilityChance + hazardAbilityChance; }
}
