using System;
using UnityEngine;

// контроллер корабля
public class ShipController
{
    private readonly ShipModelView shipMV;
    private readonly HitpointsCanvasModelView shipHPMV;

    private float rbStartDrag;
    // Скалярная величина наклона корабля
    private float dotZ;
    private float dotX;

    // Таймер до флипа
    private float flipDelay = 0;
    private float flipTime = 0;

    private float previousAngularDrag;

    private bool isMoving = false;

    /// <summary>
    /// Конструктор корабля
    /// </summary>
    /// <param name="shipModelView">Модель-представление этого корабля</param>
    /// <param name="shipHPModelView">Полоска ХП этого корабля</param>
    public ShipController(ShipModelView shipModelView, HitpointsCanvasModelView shipHPModelView)
    {
        shipHPMV = shipHPModelView;
        shipMV = shipModelView;

        rbStartDrag = shipMV.Rigidbody.drag;

        //EventHandlers
        shipMV.OnInput += HandleInput;
        shipMV.OnAction += HandleAction;
        shipMV.OnTriggerIN += HandleTriggerIN;
        shipMV.OnTriggerOUT += HandleTriggerOUT;
        shipMV.OnDamageRecieved += HandleRecieveDamage;
        shipMV.OnFixedUpdate += HandleFixedUpdate;
        shipMV.OnRespawn += HandleRespawn;

        previousAngularDrag = shipMV.Rigidbody.angularDrag;

        shipMV.Health = shipMV.shipDriveParams.health;
    }

    /// <summary>
    /// Обработка входа в зону триггера
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="tag"></param>
    private void HandleTriggerIN(object sender, string tag)
    {
        if (shipMV.ShieldSlot.childCount == 0 && shipMV.autoBoostHelper == false)
        {
            if (tag.Equals("SlowPoint"))
            {
                shipMV.Rigidbody.velocity = Vector3.zero;
                shipMV.Rigidbody.AddRelativeForce(2000f * Vector3.back, ForceMode.Impulse);
            }

            if (tag.Equals("SlipperyPoint"))
            {
                shipMV.SlipperySound();
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0: shipMV.Rigidbody.AddRelativeTorque(new Vector3(0, -1000f, 0), ForceMode.Impulse); break;
                    case 1: shipMV.Rigidbody.AddRelativeTorque(new Vector3(0, 1000f, 0), ForceMode.Impulse); break;
                }
            }

            if (tag.Equals("Tornado"))
            {
                shipMV.Rigidbody.AddForce(Vector3.up * 500f, ForceMode.Impulse);

                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0: shipMV.Rigidbody.AddRelativeTorque(new Vector3(0, -1000f, 0), ForceMode.Impulse); break;
                    case 1: shipMV.Rigidbody.AddRelativeTorque(new Vector3(0, 1000f, 0), ForceMode.Impulse); break;
                }
            }
        }
    }

    /// <summary>
    /// Обработка выхода из зоны триггера
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="tag"></param>
    private void HandleTriggerOUT(object sender, string tag)
    {
        //if (tag.Equals("SlowPoint"))
        //{
        //    shipMV.Rigidbody.drag = rbStartDrag;
        //}
    }

    /// <summary>
    /// Обработка ввода действия
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="direction"></param>
    private void HandleAction(object sender, Vector3 direction)
    {
        if(Time.timeScale != 0)
        {
            //if (direction != Vector3.back)
            if (direction == Vector3.forward)
                shipMV.PrimaryAction(direction);

            else
                shipMV.SecondaryAction();
        }
    }

    /// <summary>
    /// Обработка ввода управления
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="input"></param>
    private void HandleInput(object sender, Vector3 input)
    {
        Vector3 down = Vector3.Project(shipMV.Rigidbody.velocity, shipMV.transform.up);
        Vector3 right = Vector3.Project(shipMV.Rigidbody.velocity, -shipMV.transform.right);
        Vector3 forward = Vector3.Project(shipMV.Rigidbody.velocity, -shipMV.transform.forward);

        shipMV.transform.Rotate(0f, input.x * shipMV.shipDriveParams.RotateCoef * InputParams.moveTimeScale * Time.fixedDeltaTime, 0f, Space.Self);

        Ray ray = new Ray(shipMV.transform.position, Vector3.down);
        bool wasHit = Physics.Raycast(ray, out RaycastHit raycastHit, shipMV.shipDriveParams.distance * 10f);

        if (wasHit && raycastHit.distance <= shipMV.shipDriveParams.distance)
        {
            shipMV.isOnGround = true;
        }

        else
        {
            shipMV.isOnGround = false;
        }

        if (shipMV.isOnGround)
        {
            //if (shipMV.DustTrail.isPlaying == false)
            //    shipMV.DustTrail.Play(true);

            shipMV.Rigidbody.angularDrag = previousAngularDrag;

            Vector3 direction = Vector3.ProjectOnPlane(shipMV.transform.forward, Vector3.up);
            shipMV.Rigidbody.AddForce(direction * input.z * shipMV.shipDriveParams.acceleration * InputParams.moveTimeScale, ForceMode.Acceleration);
            forward *= shipMV.shipDriveParams.inertialCoef;
            right *= shipMV.shipDriveParams.inertialCoef;


        }
        else if (shipMV.autoBoostHelper)
        {
            shipMV.Rigidbody.angularDrag = 3f;
            if (wasHit && !shipMV.isOnGround)
            {
                shipMV.Rigidbody.AddForce(Vector3.down, ForceMode.Acceleration);
            }
        }
        else
        {
            //if (shipMV.DustTrail.isPlaying)
            //    shipMV.DustTrail.Stop(true);

            shipMV.Rigidbody.angularDrag = 3f;
        }

        shipMV.Rigidbody.velocity = down + forward + right;

        if (isMoving == false && input.z != 0)
        {
            shipMV.AccecelerationSoundOn();
            isMoving = true;
        }

        else if (input.z == 0)
        {
            shipMV.AccecelerationSoundOff();
            isMoving = false;
        }

        shipMV.AnimatorVentBlade.SetFloat("RotationSpeed", shipMV.Rigidbody.velocity.z);
    }

    /// <summary>
    /// Обработка получения урона
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="amount"></param>
    private void HandleRecieveDamage(object sender, float amount)
    {
        if (shipHPMV != null)
        {
            if (shipMV.ShieldSlot.childCount == 0)
            {
                shipMV.HitSound();

                shipMV.Health -= amount;

                float percentageConst = 100 / shipMV.StartHealth;

                shipHPMV.GreenBarFill = shipMV.Health / shipMV.StartHealth;
                shipHPMV.HPAmount.text = $"{shipMV.Health * percentageConst}%";

                if (shipMV.Health <= 0)
                {
                    shipMV.IsAlive = false;
                    shipHPMV.HPAmount.text = $"X_X";
                    Debug.Log($"{shipMV.name} was destroyed!");

                    //TODO переделать
                    //shipMV.DetachPilot();

                    shipMV.StartCoroutine(shipMV.RespawnShip(shipMV.shipDriveParams.respawnTime));
                }

                if(shipMV.Health > shipMV.StartHealth * 0.33f)
                {
                    shipHPMV.StartCoroutine(shipHPMV.ShowHideHp(5f));
                }

                else
                {
                    shipHPMV.StopAllCoroutines();
                    shipHPMV.ShowHp();
                }
            }
        }
    }

    private void HandleRespawn(object sender, float startHealth)
    {
        shipMV.Health = startHealth;
        shipMV.IsAlive = true;

        shipMV.PrimaryAbility = null;
        shipMV.SecondaryAbility = null;

        float percentageConst = 100 / shipMV.StartHealth;

        shipHPMV.GreenBarFill = shipMV.Health / shipMV.StartHealth;
        shipHPMV.HPAmount.text = $"{shipMV.Health * percentageConst}%";

        shipHPMV.StartCoroutine(shipHPMV.ShowHideHp(3f));
    }

    /// <summary>
    /// Обработка FixedUpdate, проверка заваливания корабля TODO переделать иначе?
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleFixedUpdate(object sender, EventArgs e)
    {
        // Получаем скалярное произведение y+ и y- векторов корабля для того, чтобы получить уровень его наклона
        dotZ = Vector3.Dot(shipMV.transform.up, Vector3.down);
        dotX = Vector3.Dot(shipMV.transform.forward, Vector3.down);

        if (shipMV.IsAlive)
        {
            if (Physics.Raycast(shipMV.transform.position, Vector3.down, 5f, 1 << LayerMask.NameToLayer("Ground")))
            {
                if (dotZ > -0.1f)
                    FlipShip(shipMV.transform.forward);

                else if (dotX > 0.7f)
                    FlipShip(shipMV.transform.up);

                else if (dotX < -0.7f)
                    FlipShip(-shipMV.transform.up);

                else
                    flipDelay = 0;
            }
        }
    }

    private void FlipShip(Vector3 direction)
    {
        if(flipDelay == 0)
        {
            flipDelay = Time.time + 3.0f;
            flipTime = Time.time + 3.0f;
        }

        if(Time.time > flipTime)
        {
            shipMV.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            shipMV.transform.position += Vector3.up * 0.5f;
            shipMV.Rigidbody.velocity = Vector3.zero;
            shipMV.Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}