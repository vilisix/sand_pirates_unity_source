using System;
using UnityEngine;

//interface for Ability
public interface IAbility
{
    AbilityData Data { get; set; }
    IAbility Add(IAbility ability);

    void Execute(Transform position);
}
