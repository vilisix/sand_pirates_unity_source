using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFactory
{
    public static CannonModelView CreateCannonModelView(Transform slot)
    {
        // Получаем GO пушки
        GameObject cannonPrefab = Resources.Load<GameObject>("Prefabs/Cannons/DefaultCannon");

        // Создаем инстанс и получаем модель-представление пушки
        CannonModelView modelView = UnityEngine.Object.Instantiate(cannonPrefab, slot.transform).GetComponent<CannonModelView>();

        return modelView;
    }
}
