using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Структура для записи в таблицу прохождения трассы
/// </summary>
public struct TrackLeaderTableStruct
{
    public Transform pilot;
    public Transform gateTransform;
    public int gateNumber;
    public System.DateTime dateTime;
    public bool isFinish;
    
}
