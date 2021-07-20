using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class ShipModelView : MonoBehaviour, IDamageable
{
    #region events

    //Events
    public event EventHandler<Vector3> OnInput = (sender, e) => { };
    public event EventHandler<Vector3> OnAction = (sender, e) => { };
    public event EventHandler<Sprite> OnPrimaryAbilityChanged = (sender, e) => { };
    public event EventHandler<Sprite> OnSecondaryAbilityChanged = (sender, e) => { };
    public event EventHandler<float> OnDamageRecieved = (sender, e) => { };
    public event EventHandler<float> OnRespawn = (sender, e) => { };
    public event EventHandler<string> OnTriggerIN = (sender, tag) => { };
    public event EventHandler<string> OnTriggerOUT = (sender, tag) => { };
    public event EventHandler OnFixedUpdate = (sender, e) => { };


    #endregion

    #region fields
    //Ship data
    [SerializeField] private Rigidbody rb;
    //Cannon spots on the ship sides
    [SerializeField] private List<Transform> frontSlots, backSlots, leftSlots, rightSlots;
    //Cannon arrays of Cannon types
    private ICannonModelView[] frontCannons, backCannons, leftCannons, rightCannons;

    // Shield spot on center of the ship
    [SerializeField] private Transform shieldSlot;

    //[SerializeField] private ParticleSystem dustTrail;

    [SerializeField] private Animator animatorVentBlade;

    //ShipAbility
    private IAbility primaryAbilitySlot;
    private IAbility secondaryAbilitySlot;

    private float health;
    private float startHealth;
    private bool isAlive = true;

    [SerializeField] AudioSource accelerationSound;
    [SerializeField] AudioSource engineSound;
    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource hazardDropSound;
    [SerializeField] AudioSource shieldSound;
    [SerializeField] AudioSource speedBoostSound;
    [SerializeField] AudioSource pickUpSound;
    [SerializeField] AudioSource slipperySound;
    [SerializeField] AudioSource checkInSound;
    [SerializeField] AudioSource hitSound;


    ////TODO переделать
    [SerializeField] private Transform pilotSlot;

    //Collider[] pirateColliders;
    //Rigidbody[] pirateRigidbodies;

    [SerializeField] Vector3 centerOfMass;

    #endregion

    #region Accessors
    //IsAlive Accessor
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    //Health Accessor
    public float Health
    {
        get => health; set => health = value;
    }

    public float StartHealth
    {
        get => startHealth;
    }

    //Ability Accessor
    public IAbility PrimaryAbility
    {
        get => primaryAbilitySlot;
        set
        {
            if (value == null)
                primaryAbilitySlot = null;
            else if (primaryAbilitySlot != null)
                primaryAbilitySlot = primaryAbilitySlot.Add(value);
            else primaryAbilitySlot = value;

            if (primaryAbilitySlot != null)
            {
                OnPrimaryAbilityChanged(this, primaryAbilitySlot.Data.Icon);
                PickUpSound();
            }
            else OnPrimaryAbilityChanged(this, null);

        }
    }
    public IAbility SecondaryAbility
    {
        get => secondaryAbilitySlot;
        set
        {
            if (value == null)
                secondaryAbilitySlot = null;
            else if (secondaryAbilitySlot != null)
                secondaryAbilitySlot = secondaryAbilitySlot.Add(value);
            else secondaryAbilitySlot = value;

            if (secondaryAbilitySlot != null)
            {
                OnSecondaryAbilityChanged(this, secondaryAbilitySlot.Data.Icon);
                PickUpSound();
            }
            else OnSecondaryAbilityChanged(this, null);
        }
    }

    //Shield slot accessor
    public Transform ShieldSlot { get => shieldSlot; }

    // Pirate slot accessor
    public Transform PirateSlot { get => pilotSlot; }

    //Cannons accessor
    public ICannonModelView[] FrontCannons { get => frontCannons; }
    public ICannonModelView[] BackCannons { get => backCannons; }
    public ICannonModelView[] LeftCannons { get => leftCannons; }
    public ICannonModelView[] RightCannons { get => rightCannons; }

    //Rotation Accessor
    public Quaternion Rotation
    {
        get => rb.rotation;
        set
        {
            if (rb.rotation != value)
            {
                rb.rotation = value;
            }
        }
    }

    //public ParticleSystem DustTrail
    //{
    //    get => dustTrail;
    //}

    public Animator AnimatorVentBlade
    {
        get => animatorVentBlade;
    }

    public Rigidbody Rigidbody { get => rb; }

    public ShipDriveParams shipDriveParams;
    public bool isOnGround;

    public bool autoBoostHelper = false;


    #endregion

    public EnemyPilotController enemyPilotController;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 1f);
    }

    private void Awake()
    {
        //TODO переделать

        //pirateColliders = pirate.GetComponentsInChildren<Collider>();

        //foreach (Collider col in pirateColliders)
        //    col.enabled = false;

        //pirateRigidbodies = pirate.GetComponentsInChildren<Rigidbody>();

        //foreach (Rigidbody rb in pirateRigidbodies)
        //    rb.isKinematic = true;


        // Создаем пушки
        CreateCannons();

        // Устанавливаем центр тяжести корабля
        Rigidbody.centerOfMass = centerOfMass;

        StartCoroutine(StartEngine());

        health = shipDriveParams.health;
        startHealth = health;
    }

    /// <summary>
    /// Метод ввода управления. Вызывается пилотом этого корабля. При вызове активирует событие OnInput
    /// </summary>
    /// <param name="input"></param>
    public void SteeringInput(Vector3 input)
    {
        if (isAlive)
            OnInput(this, input);
    }

    /// <summary>
    /// Метод ввода действия. Вызывается пилотом этого корабля. При вызове активирует событие OnAction
    /// </summary>
    /// <param name="input"></param>
    public void ActionInput(Vector3 input)
    {
        if (isAlive)
            OnAction(this, input);
    }

    /// <summary>
    /// Метод заполнения пушками слотов корабля. Сейчас вызывается в Awake
    /// </summary>
    public void CreateCannons()
    {
        frontCannons = new ICannonModelView[frontSlots.Count];
        for (int i = 0; i < frontSlots.Count; i++)
            frontCannons[i] = CannonFactory.CreateCannonModelView(frontSlots[i]);

        //backCannons = new ICannonModelView[backSlots.Count];
        //for (int i = 0; i < backSlots.Count; i++)
        //    backCannons[i] = CannonFactory.CreateCannonModelView(backSlots[i]);

        leftCannons = new ICannonModelView[leftSlots.Count];
        for (int i = 0; i < leftSlots.Count; i++)
            leftCannons[i] = CannonFactory.CreateCannonModelView(leftSlots[i]);

        rightCannons = new ICannonModelView[rightSlots.Count];
        for (int i = 0; i < rightSlots.Count; i++)
            rightCannons[i] = CannonFactory.CreateCannonModelView(rightSlots[i]);

    }

    //TODO переделать
    //public void DetachPilot()
    //{
    //    pirate.GetComponent<Animator>().enabled = false;

    //    foreach (Collider col in pirateColliders)
    //        col.enabled = true;

    //    foreach (Rigidbody rb in pirateRigidbodies)
    //        rb.isKinematic = false;

    //    pirate.transform.parent = null;
    //}

    /// <summary>
    /// Действие ModelView при активации первичной способности. Вызывается из контроллера!
    /// </summary>
    public void PrimaryAction(Vector3 direction)
    {
        if (primaryAbilitySlot != null)
        {
            if (direction == Vector3.forward)
            {
                foreach (ICannonModelView cannon in frontCannons)
                    cannon.Fire(primaryAbilitySlot);
            }

            if (direction == Vector3.left)
            {
                foreach (ICannonModelView cannon in leftCannons)
                    cannon.Fire(primaryAbilitySlot);
            }

            if (direction == Vector3.right)
            {
                foreach (ICannonModelView cannon in rightCannons)
                    cannon.Fire(primaryAbilitySlot);
            }

            PrimaryAbility = null; // TODO: включить
            ShootSound();
        }
    }

    /// <summary>
    /// Действие ModelView при активации второстепенной способности. Вызывается из контроллера!
    /// </summary>
    public void SecondaryAction()
    {
        if (secondaryAbilitySlot != null)    // TODO: реализовать через Input system
        {
            // Обработка абилок щитов
            if (SecondaryAbility is IShield)
            {
                // Удаляем щит из слота под щиты, если там что-то присутствует
                if (shieldSlot.childCount != 0)
                {
                    Destroy(shieldSlot.GetChild(0).gameObject);
                }

                SecondaryAbility.Execute(shieldSlot);
                ShieldSound();
            }

            if (SecondaryAbility is ISpeedUp)
            {
                // Удаляем турбину из заднего слота, если там что-то присутствует
                foreach (Transform slot in backSlots)
                {
                    if (slot.childCount != 0)
                    {
                        Destroy(slot.GetChild(0).gameObject);
                    }

                    SecondaryAbility.Execute(slot);
                    SpeedBoostSound();
                }
            }

            if (SecondaryAbility is IHazard)
            {
                foreach (Transform slot in backSlots)
                {
                    SecondaryAbility.Execute(slot);
                }

                HazardDropSound();
            }

            SecondaryAbility = null; // TODO: включить


        }
    }

    /// <summary>
    /// Получение урона - метод вызывается чем-то извне(например снарядом), вызывает событие получения урона в контроллере
    /// </summary>
    /// <param name="amount"></param>
    public void RecieveDamage(float amount)
    {
        if (isAlive)
            OnDamageRecieved(this, amount);
    }

    /// <summary>
    /// Вызывает событие обработки FixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        OnFixedUpdate(this, EventArgs.Empty);
    }

    /// <summary>
    /// Вызывает событие обработки входного триггера(которое передает тег в качестве параметра)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerIN(this, other.tag);
    }

    /// <summary>
    /// Вызывает событие обработки выходного триггера(которое передает тег в качестве параметра)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        OnTriggerOUT(this, other.tag);
    }

    #region Audio
    public void AccecelerationSoundOn()
    {
        accelerationSound.Play();
    }

    public void AccecelerationSoundOff()
    {
        accelerationSound.Stop();
    }

    public void ShootSound()
    {
        shootSound.Play();
    }

    public void HazardDropSound()
    {
        hazardDropSound.Play();
    }

    public void ShieldSound()
    {
        shieldSound.Play();
    }

    public void SpeedBoostSound()
    {
        speedBoostSound.Play();
    }

    public void PickUpSound()
    {
        pickUpSound.Play();
    }

    public void SlipperySound()
    {
        if(slipperySound.isPlaying == false)
            slipperySound.Play();
    }

    public void EngineSoundOn()
    {
        engineSound.volume = 1f;
    }

    public void EngineSoundOff()
    {
        engineSound.volume = 0.4f;
    }

    System.Collections.IEnumerator StartEngine()
    {
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(3f, 5f));
        engineSound.Play();
    }

    public void CheckInSound()
    {
        checkInSound.Play();
    }
    public void HitSound()
    {
        hitSound.Play();
    }

    public System.Collections.IEnumerator RespawnShip(float delay)
    {
        GameObject smoke = ParticleFactory.CreateShipDestroyedSmoke(transform);

        yield return new WaitForSeconds(delay);
        OnRespawn(this, startHealth);
        smoke.GetComponent<ParticleSystem>().Stop();

        Destroy(smoke, 3f);
    }

    #endregion
}
