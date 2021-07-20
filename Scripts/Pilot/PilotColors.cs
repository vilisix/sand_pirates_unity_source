using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Pilot Color Table", menuName = "ScriptableObjects/Pilot Color Table", order = 3)]

public class PilotColors : ScriptableObject
{
    public List<Color> colorList;
}