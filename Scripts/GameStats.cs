using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStats", menuName = "ScriptableObjects/GameStats", order = 1)]
public class GameStats: ScriptableObject
{
    public string testMainMenuTrackScene = "Prefabs/Track/MainMenuTrackScene";
    public string testTrackPrefab = "Prefabs/Track/desert_race_track";
    public string testTrackPathPrefab = "Prefabs/Track/DesertRaceCheckpointsPath";
    public string containerPrefab = "Prefabs/Track/AbilityContainer";
    public string placerPrefab = "Prefabs/Track/StartPlacer";
    public string sandStormPrefab = "Prefabs/Effects/SandStorm";
}
