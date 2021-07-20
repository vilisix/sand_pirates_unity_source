using System.Collections;
using UnityEngine;


/// <summary>
/// Управление временем
/// </summary>
public class TimeFollowController: Singleton<TimeFollowController>
{
    /// <summary>
    /// пауза коэф для движущихся объектов.
    /// </summary>
    public void PauseMove()
    {
        InputParams.moveTimeScale = 0f;
    }
    /// <summary>
    /// пауза с обратным отсчетом
    /// </summary>
    /// <param name="timer"></param>
    public void Pause(float timer)
    {
        StartCoroutine(TimerPause(timer));
    }

    /// <summary>
    /// возобновление игры
    /// </summary>
    public void ResumeMove()
    {
        InputParams.moveTimeScale = 1f;
    }

    /// <summary>
    /// установить множитель скорости игрового времени
    /// </summary>
    /// <param name="coeff"></param>
    public void SetTimeScale(float coeff = 2f)
    {
        Time.timeScale = coeff;
    }

    /// <summary>
    /// умножить игровое время на два
    /// </summary>
    public void DoubleTimeScale()
    {
        Time.timeScale *= 2;
    }

    private IEnumerator TimerPause(float timer)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(timer);
        Time.timeScale = 1f;
    }


    private void Awake()
    {
        InputParams.moveTimeScale = 1f;
    }

    public void SpeedUp()
    {
        Time.timeScale += 0.5f;
    }

    public void SpeedDown()
    {
        Time.timeScale -= 0.5f;
    }
}


