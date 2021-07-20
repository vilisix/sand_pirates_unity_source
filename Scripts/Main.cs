using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// скрипт висит на пустом объекте Main
public class Main : MonoBehaviour
{
    public Canvas canvas;
    public Camera mainCamera;
    public GameStats gameStats;

    private GameObject menuTrackScene = null;

    // точка входа в приложение
    void Start()
    {
        CreateMainMenu();

        // подписываемся на кнопки из меню
    }

    private void CreateMainMenu()
    {
        menuTrackScene = TrackFactory.CreateMainMenuTrack(gameStats.testMainMenuTrackScene);
       MainMenuModelView menuModelView = UIFactory.CreateMainMenuModelView(canvas);
       menuModelView.OnStart += HandleGameCreation;
       menuModelView.OnExit += HandleExitGame;
    }

    private void HandleGameCreation(object sender, NewGameEventArgs e)
    {
        if(menuTrackScene != null) Destroy(menuTrackScene);
        Destroy(((MonoBehaviour)sender).gameObject);  // удаляем sender, т.е в нашем случае это объект со скриптом menuModelView ( но можно было его кэшировать и не приводить тип sender'а)
        StartGame(e.LapsCount, e.ShipCount);
    }

    private void HandleExitGame(object sender, EventArgs e)
    {
        Application.Quit();
    }

    private void StartGame(int lapsCount, int shipCount)
    {
        GameController game = new GameController(canvas, mainCamera, gameStats, lapsCount, shipCount);
        game.CountdownPause();
        game.OnDestroyGame += HandleOnGameDestroy;
    }

    private void HandleOnGameDestroy(object sender, EventArgs e)
    {
        CreateMainMenu();
    }
}
