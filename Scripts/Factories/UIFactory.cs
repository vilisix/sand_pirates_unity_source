using UnityEngine;

// фабрика UI, создает заранее подготовленные UI элементы из папки Recources
public class UIFactory
{
    public static MainMenuModelView CreateMainMenuModelView(Canvas canvas)
    {
        GameObject mainMenuPrefab = Resources.Load<GameObject>("Prefabs/UI/MainMenuPanel");
        MainMenuModelView modelView = UnityEngine.Object.Instantiate(mainMenuPrefab, canvas.transform)
            .GetComponent<MainMenuModelView>();
        return modelView;
    }
        
    public static PauseMenuModelView CreatePauseMenuModelView(Canvas canvas)
    {
        GameObject pauseMenuPrefab = Resources.Load<GameObject>("Prefabs/UI/PauseMenuPanel");
        PauseMenuModelView modelView = UnityEngine.Object.Instantiate(pauseMenuPrefab, canvas.transform)
            .GetComponent<PauseMenuModelView>();
        return modelView;
    }

    public static AbilityHUDModelView CreatePlayerAbilityUI(Canvas canvas)
    {
        GameObject abilityHUDPrefab = Resources.Load<GameObject>("Prefabs/UI/PlayerAbilityUI");
        AbilityHUDModelView modelView = UnityEngine.Object.Instantiate(abilityHUDPrefab, canvas.transform)
            .GetComponent<AbilityHUDModelView>();
        return modelView;
    }
    
    public static DirectionArrowModelView CreateDirectionArrow(Canvas canvas)
    {
        GameObject arrowPrefab = Resources.Load<GameObject>("Prefabs/UI/DirectionArrow");
        DirectionArrowModelView modelView = UnityEngine.Object.Instantiate(arrowPrefab, canvas.transform)
            .GetComponent<DirectionArrowModelView>();
        return modelView;
    }

    public static HitpointsCanvasModelView CreateShipHealthBar(Transform shipTransform)
    {
        GameObject hpCanvasPrefab = Resources.Load<GameObject>("Prefabs/UI/HitpointsCanvas");
        HitpointsCanvasModelView modelView = UnityEngine.Object.Instantiate(hpCanvasPrefab, shipTransform)
            .GetComponent<HitpointsCanvasModelView>();
        return modelView;
    }

    public static void CreatePlayerPointer(Transform shipTransform)
    {
        GameObject playerPointerPrefab = Resources.Load<GameObject>("Prefabs/UI/PlayerPointerCanvas");
        UnityEngine.Object.Instantiate(playerPointerPrefab, shipTransform);
    }

    public static HitpointsCanvasModelView CreatePlayerShipHealthBar(Transform shipTransform)
    {
        GameObject hpCanvasPrefab = Resources.Load<GameObject>("Prefabs/UI/PlayerHitpointsCanvas");
        HitpointsCanvasModelView modelView = UnityEngine.Object.Instantiate(hpCanvasPrefab, shipTransform)
            .GetComponent<HitpointsCanvasModelView>();
        return modelView;
    }

    public static TrackFinishMenuModelView CreateTrackFinishMenuModelView(Canvas canvas)
    {
        GameObject trackFinishMenuPrefab = Resources.Load<GameObject>("Prefabs/UI/TrackFinishMenu");
        TrackFinishMenuModelView modelView = UnityEngine.Object.Instantiate(trackFinishMenuPrefab, canvas.transform)
            .GetComponent<TrackFinishMenuModelView>();
        return modelView;
    }
    public static TrackPositionModelView CreateTrackPositionModelView(Canvas canvas)
    {
        GameObject trackPosMenuPrefab = Resources.Load<GameObject>("Prefabs/UI/TrackPositionHUD");
        TrackPositionModelView modelView = UnityEngine.Object.Instantiate(trackPosMenuPrefab, canvas.transform)
            .GetComponent<TrackPositionModelView>();
        return modelView;
    }



    public static AlertsModelView CreateAlertsModelView(Canvas canvas)
    {
        GameObject alertsModelViewPrefab = Resources.Load<GameObject>("Prefabs/UI/AlertsCanvas");
        AlertsModelView modelView = UnityEngine.Object.Instantiate(alertsModelViewPrefab, canvas.transform)
            .GetComponent<AlertsModelView>();
        return modelView;
    }


    public static GameObject CreateMinimapObj(Canvas canvas)
    {
        GameObject minimapPref = Resources.Load<GameObject>("Prefabs/UI/Minimap/MinimapHUD");
        GameObject minimap = UnityEngine.Object.Instantiate(minimapPref, canvas.transform);
        return minimap;
    }

    public static void AddMinimapPointToPlayer(Transform parenTransform)
    {
        GameObject minimapPref = Resources.Load<GameObject>("Prefabs/UI/Minimap/PlayerIconMinimap");
        GameObject minimapPoint = UnityEngine.Object.Instantiate(minimapPref);
        minimapPoint.transform.SetParent(parenTransform);
        minimapPoint.transform.position = parenTransform.position;
    }

    public static void AddMinimapPointToEnemy(Transform parenTransform)
    {
        GameObject minimapPref = Resources.Load<GameObject>("Prefabs/UI/Minimap/EnemyIconMinimap");
        GameObject minimapPoint = UnityEngine.Object.Instantiate(minimapPref);
        minimapPoint.transform.SetParent(parenTransform);
        minimapPoint.transform.position = parenTransform.position;
    }

    

}
