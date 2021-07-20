using System;
using UnityEngine;

/// <summary>
/// Класс контролирует ввод данных от пользователя.
/// </summary>
public class InputControl : Singleton<InputControl>
{
    //private CinemachineModelView cinemachineModelView;

    const string changeCam = "ChangeCam";
    const string upFire    = "upFire";
    const string leftFire  = "leftFire";
    const string rightFire = "rightFire";
    const string downFire  = "downFire";
    const string speedUp   = "speedUp";
    const string speedDown = "speedDown";
    const string backView  = "backView";

    public event EventHandler<Vector3> OnActionInput = (sender, e) => { };

    //private void Start()
    //{
    //    cinemachineModelView = CinemachineModelView.Instance;
    //}

    void Update()
    {
        KeyInputUpdate();
        ProcedureCaller();
    }

    /// <summary>
    /// Опрашивает вводные данные от пользователя
    /// </summary>
    private void KeyInputUpdate()
    {

        InputParams.XAxis = Input.GetAxis("Horizontal");
        InputParams.ZAxis = Input.GetAxis("Vertical");


        if ((Input.GetButtonDown(changeCam)))
            InputParams.ChangeCamButtonDown = true;

        if ((Input.GetButtonDown(upFire)))
            InputParams.UpfireButtonDown = true;

        if ((Input.GetButtonDown(leftFire)))
            InputParams.LeftFireButtonDown = true;

        if ((Input.GetButtonDown(rightFire)))
            InputParams.RightFireButtonDown = true;

        if ((Input.GetButtonDown(downFire)))
            InputParams.DownFireButtonDown = true;

        if ((Input.GetButtonDown(speedUp)))
            TimeFollowController.Instance.SpeedUp();

        if ((Input.GetButtonDown(speedDown)))
            TimeFollowController.Instance.SpeedDown();

        if ((Input.GetButtonDown(backView)))
            CinemachineModelView.Instance.BackViewOn();

        if ((Input.GetButtonUp(backView)))
            CinemachineModelView.Instance.BackViewOff();
    }

    /// <summary>
    /// Вызов дополнительных процедур по факту нажатия
    /// </summary>
    private void ProcedureCaller()
    {
        ///проверка переключения камеры на следующую
        if (InputParams.ChangeCamButtonDown)
            CinemachineModelView.Instance.NextCam();

        Vector3 actionDirection = Vector3.zero;

        if (InputParams.UpfireButtonDown)
            OnActionInput(this, Vector3.forward);

        if (InputParams.LeftFireButtonDown)
            OnActionInput(this, Vector3.left);

        if (InputParams.RightFireButtonDown)
            OnActionInput(this, Vector3.right);

        if (InputParams.DownFireButtonDown)
            OnActionInput(this, Vector3.back);
    }

    /// <summary>
    /// процедура для вызова смены камера из других мест
    /// </summary>
    public void ChangeCamButtonDown()
    {
        InputParams.ChangeCamButtonDown = true;
    }
}

public static class InputParams
{
    public static float moveTimeScale;
    public static bool canPause;
    private static float zAxis;
    private static float xAxis;
    private static bool changeCamButtonDown;

    private static bool upFireButtonDown;
    private static bool leftFireButtonDown;
    private static bool rightFireButtonDown;
    private static bool downFireButtonDown;

    public static float ZAxis { get => zAxis; set => zAxis = value; }
    public static float XAxis { get => xAxis; set => xAxis = value; }

    /// <summary>
    /// Проверяет нажатие кноки, если она нажата, возвращает true, но состояние переходит в false
    /// </summary>
    public static bool ChangeCamButtonDown
    {
        get
        {
            if (changeCamButtonDown)
            {
                changeCamButtonDown = false;
                return true;
            }

            else
            {
                return false;
            }

        }

        set => changeCamButtonDown = value;
    }

    /// <summary>
    /// Проверяет нажатие кноки, если она нажата, возвращает true, но состояние переходит в false
    /// </summary>
    public static bool UpfireButtonDown
    {
        get
        {
            if (upFireButtonDown)
            {
                upFireButtonDown = false;
                return true;
            }

            else
            {
                return false;
            }
        }

        set => upFireButtonDown = value;
    }

    /// <summary>
    /// Проверяет нажатие кноки, если она нажата, возвращает true, но состояние переходит в false
    /// </summary>
    public static bool LeftFireButtonDown
    {
        get
        {
            if (leftFireButtonDown)
            {
                leftFireButtonDown = false;
                return true;
            }
            else
            {
                return false;
            }

        }

        set => leftFireButtonDown = value;
    }

    /// <summary>
    /// Проверяет нажатие кноки, если она нажата, возвращает true, но состояние переходит в false
    /// </summary>
    public static bool RightFireButtonDown
    {
        get
        {
            if (rightFireButtonDown)
            {
                rightFireButtonDown = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        set => rightFireButtonDown = value;
    }

    /// <summary>
    /// Проверяет нажатие кноки, если она нажата, возвращает true, но состояние переходит в false
    /// </summary>
    public static bool DownFireButtonDown
    {
        get
        {
            if (downFireButtonDown)
            {
                downFireButtonDown = false;
                return true;
            }

            else
            {
                return false;
            }
        }

        set => downFireButtonDown = value;
    }
}