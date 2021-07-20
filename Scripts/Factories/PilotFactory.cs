using System;
using UnityEngine;
using System.Collections.Generic;

public class PilotFactory
{
    static List<Color> colorList = Resources.Load<PilotColors>("Pilot/Pilot Color Table").colorList;

    public static PlayerPilotModelView CreatePlayerPilotModelView(Transform parentShip)
    {
        GameObject testPlayerPilotPrefab = Resources.Load<GameObject>("Prefabs/Pilot/TestPlayer");
        parentShip.TryGetComponent<ShipModelView>(out ShipModelView shipMV);

        PlayerPilotModelView modelView = UnityEngine.Object.Instantiate(testPlayerPilotPrefab, shipMV.PirateSlot.transform)
            .GetComponent<PlayerPilotModelView>();

        //Material[] m = modelView.PirateRenderer.materials;
        //m[0].color = colorList[UnityEngine.Random.Range(0, colorList.Count)];
        //m[2].color = colorList[UnityEngine.Random.Range(0, colorList.Count)];

        return modelView;
    }

    public static PlayerPilotController CreatePlayerPilotController(PlayerPilotModelView playerMV, ShipModelView shipMV, TrackPath checkpoints, DirectionArrowModelView dirArrowHUD)
    {
        return new PlayerPilotController(playerMV, shipMV, checkpoints, dirArrowHUD);
    }

    public static EnemyPilotModelView CreateEnemyPilotModelView(Transform parentShip)
    {
        GameObject testEnemyPilotPrefab = Resources.Load<GameObject>("Prefabs/Pilot/TestEnemy");
        parentShip.TryGetComponent<ShipModelView>(out ShipModelView shipMV);

        EnemyPilotModelView modelView = UnityEngine.Object.Instantiate(testEnemyPilotPrefab, shipMV.PirateSlot.transform)
            .GetComponent<EnemyPilotModelView>();

        Material[] m = modelView.PirateRenderer.materials;
        m[0].color = colorList[UnityEngine.Random.Range(0, colorList.Count)];
        m[2].color = colorList[UnityEngine.Random.Range(0, colorList.Count)];

        return modelView;
    }
    
    public static EnemyPilotController CreateEnemyPilotController(EnemyPilotModelView enemyMV, ShipModelView shipMV, TrackPath checkpoints)
    {
        return new EnemyPilotController(enemyMV, shipMV, checkpoints);
    }
    
}
