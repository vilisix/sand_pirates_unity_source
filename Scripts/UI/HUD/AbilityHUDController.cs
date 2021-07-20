using System;
using UnityEngine;

public class AbilityHUDController
{
    private readonly AbilityHUDModelView hudMV;
    private readonly ShipModelView shipMV;

    private Sprite defaultSprite;

    public AbilityHUDController(AbilityHUDModelView modelView, ShipModelView ship)
    {
        hudMV = modelView;
        defaultSprite = hudMV.PrimarySlot.sprite;
        shipMV = ship;
        shipMV.OnPrimaryAbilityChanged += HandnePrimaryAbilityChanged;
        shipMV.OnSecondaryAbilityChanged += HandleSecondaryAbilityChanged;

    }

    private void HandleSecondaryAbilityChanged(object sender, Sprite e)
    {
        if (e != null)
            hudMV.SecondarySlot.sprite = e;
        else hudMV.SecondarySlot.sprite = defaultSprite;
    }

    private void HandnePrimaryAbilityChanged(object sender, Sprite e)
    {
        if (e != null)
            hudMV.PrimarySlot.sprite = e;
        else hudMV.PrimarySlot.sprite = defaultSprite;
    }
}
