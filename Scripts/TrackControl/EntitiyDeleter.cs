using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс автоматически добавляется к созданному бусту и при destroy удаляет этот буст
/// из списка EntityOnTrackCreator, чтобы на его месте создался новый
/// </summary>
public class EntitiyDeleter : MonoBehaviour
{
    private EntityOnTrackCreator parentCreator;
    public void SetParentCreator(EntityOnTrackCreator incCreator)
    {
        parentCreator = incCreator;
    }

    private void OnDestroy()
    {
        parentCreator.RemoveObjectFromSlot(gameObject);
    }
}
