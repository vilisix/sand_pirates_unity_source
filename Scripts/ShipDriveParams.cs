using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// параметры управляемости корабля
/// </summary>
[CreateAssetMenu(fileName = "ShipDriveParams", menuName = "ScriptableObjects/ShipDriveParams", order = 1)]
public class ShipDriveParams : ScriptableObject
{
    /// <summary>
    /// скорость поворота
    /// </summary>
    public float RotateCoef = 100f;
    /// <summary>
    /// коэффициент ускорения корабля
    /// </summary>
    public float acceleration = 50f;
    /// <summary>
    /// дистанция луча бьющего вниз для определения на земле объект или в воздухе
    /// </summary>
    public float distance = 3f;
    /// <summary>
    /// инерция: коэффициент сохранения скорости с предыдущего фрейма
    /// (экспоненциальный)
    ///  значения больше 1.1 не устанавливать.
    ///  при изменении коффициента надо вносить обратную поправку в коэффециент максимальной скорости
    /// </summary>
    public float inertialCoef = 0.97f;

    /// <summary>
    /// Здоровье корабля
    /// </summary>
    public float health = 50;

    /// <summary>
    /// Время восстановления корабля
    /// </summary>
    public float respawnTime = 7f;
}
