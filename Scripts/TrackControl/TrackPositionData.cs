using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// Класс содержит данные по связке объекта и его текущего чекпоинта
/// </summary>
public static class TrackPositionData
{
    private static Dictionary<Transform, Transform> currentPositions = new Dictionary<Transform, Transform>();

    /// <summary>
    /// Устанавливает текущий чекпоинт объекта
    /// </summary>
    /// <param name="gameObj">Объект</param>
    /// <param name="newPoint">Чекпоинт</param>
    public static void SetCurrentPointToObject(Transform gameObj, Transform newPoint)
    {
        if (currentPositions.ContainsKey(gameObj))
        {
            currentPositions.Remove(gameObj);
        }
        
        currentPositions.Add(gameObj, newPoint);

    }

    /// <summary>
    /// Возвращает текущий чекпоинт объекта
    /// </summary>
    /// <param name="gameObj"></param>
    /// <returns></returns>
    public static Transform GetCurrentPointToObject(Transform gameObj)
    {
        if (currentPositions.ContainsKey(gameObj))
        {
            return currentPositions[gameObj];
        }
        else
        {
            return default;
        }
    }

 

}
