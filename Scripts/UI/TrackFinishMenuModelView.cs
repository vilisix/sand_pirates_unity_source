using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Завершение гонки
/// </summary>
public class TrackFinishMenuModelView : MonoBehaviour
{
    public TrackPath trackPath;
    public Text text;
    public Button exitButton;

    public event EventHandler OnExitToMainMenu = (sender, e) => { };

    private void Start()
    {
        exitButton.onClick.AddListener(delegate
            {
                OnExitToMainMenu(this, EventArgs.Empty);
            });
    }
    private void Update()
    {
        System.Text.StringBuilder strb = new System.Text.StringBuilder();
        if (trackPath.isWin)
        {
            strb.AppendLine("Sink me! Ye finished first! \n");
        }

        else
        {
            strb.AppendLine("Better luck next time, ye landlubber! \n");
        }

        foreach (string rawLeader in trackPath.GetFinishTrackLeaderBoard())
        {
            strb.AppendLine(rawLeader);
        }
        text.text = strb.ToString();
    }
}
