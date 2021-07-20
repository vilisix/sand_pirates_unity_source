using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Выскакивающие на экране оповещения
/// </summary>
public class AlertsModelView : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    /// <summary>
    /// показывает текст на экране
    /// </summary>
    /// <param name="textToShow">текст</param>
    /// <param name="timeToShow">время показа</param>
    public void ShowText(string textToShow, float timeToShow = 0)
    {
        if (timeToShow>0)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowTextCorut(textToShow, timeToShow));
        }
        else
        {
            textMeshPro.SetText(textToShow);
        }
    }

    private IEnumerator ShowTextCorut(string textToShow, float timeToShow)
    {
        gameObject.SetActive(true);
        textMeshPro.SetText(textToShow);
        yield return new WaitForSeconds(timeToShow);
        textMeshPro.SetText("");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Показывает обратный отсчет
    /// </summary>
    /// <param name="timeInSec"></param>
    public void ShowCountDown (int timeInSec = 5)
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowCountDownCorut(timeInSec));
    }

    private IEnumerator ShowCountDownCorut(int timeInSec)
    {
        InputParams.canPause = false;

        for (int i = timeInSec; i > 0; i--)
        {
            textMeshPro.SetText(i.ToString());
            yield return new WaitForSecondsRealtime(1);
        }

        textMeshPro.SetText("GO!");

        yield return new WaitForSecondsRealtime(1);

        InputParams.canPause = true;

        gameObject.SetActive(false);
    }
}
