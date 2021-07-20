using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilityData", order = 1)]

public class AbilityData : ScriptableObject
{
    // Тип способности
    [SerializeField] private Ability abilityType;
    public Ability AbilityType { get => abilityType; set => abilityType = value; }

    public enum Ability
    {
        Shooting,
        Hazard,
        Shield,
        SpeedUp
    }

    // Общие параметры способностей
    #region General properties

    // Иконка способности
    [SerializeField] private Sprite icon;
    public Sprite Icon { get => icon; set => icon = value; }

    //Материал контейнера на сцене
    [SerializeField] private GameObject containerMesh;
    public GameObject ContainerMesh { get => containerMesh; set => containerMesh = value; }

    // Префаб способности
    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get => prefab; set => prefab = value; }

    // Куда может быть загружена способность
    //[SerializeField] private bool equippableFront, equippableBack, equippableLeft, equippableRight;
    //public bool EquippableFront { get => equippableFront; set => equippableFront = value; }
    //public bool EquippableBack { get => equippableBack; set => equippableBack = value; }
    //public bool EquippableLeft { get => equippableLeft; set => equippableLeft = value; }
    //public bool EquippableRight { get => equippableRight; set => equippableRight = value; }

    #endregion

    // Уникальные параметры для стреляющих способностей
    #region Shooting ability properties

    // Урон от снаряда
    [SerializeField] private float projectileDamage;
    public float ProjectileDamage { get => projectileDamage; set => projectileDamage = value; }

    // Начальная скорость движения снаряда
    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

    // Продолжительность существования ловушки
    [SerializeField] private float projectileLifetime;
    public float ProjectileLifetime { get => projectileLifetime; set => projectileLifetime = value; }

    // Разброс снарядов
    [SerializeField] private float projectileScatter;
    public float ProjectileScatter { get => projectileScatter; set => projectileScatter = value; }

    // Количество снарядов
    [SerializeField] private float projectilesCount;
    public float ProjectilesCount { get => projectilesCount; set => projectilesCount = value; }

    [SerializeField] private bool projectileIsExplosive;
    public bool ProjectileIsExplosive { get => projectileIsExplosive; set => projectileIsExplosive = value; }

    // Сила взрыва
    [SerializeField] private float projectileExplosionForce;
    public float ProjectileExplosionForce { get => projectileExplosionForce; set => projectileExplosionForce = value; }

    // Радиус взрыва
    [SerializeField] private float projectileExplosionRadius;
    public float ProjectileExplosionRadius { get => projectileExplosionRadius; set => projectileExplosionRadius = value; }

    #endregion

    // Уникальные параметры для ловушек
    #region Hazard ability properties

    // Урон от ловушки
    [SerializeField] private float hazardDamage;
    public float HazardDamage { get => hazardDamage; set => hazardDamage = value; }

    // Скорость полета объекта ловушки
    [SerializeField] private float hazardSpeed;
    public float HazardSpeed { get => hazardSpeed; set => hazardSpeed = value; }

    // Продолжительность существования ловушки
    [SerializeField] private float hazardLifetime;
    public float HazardLifetime { get => hazardLifetime; set => hazardLifetime = value; }

    // Разброс снарядов
    [SerializeField] private float hazardScatter;
    public float HazardScatter { get => hazardScatter; set => hazardScatter = value; }

    // Количество снарядов
    [SerializeField] private float hazardsCount;
    public float HazardsCount { get => hazardsCount; set => hazardsCount = value; }

    // Ловушка взрывается?
    [SerializeField] private bool hazardIsExplosive;
    public bool HazardIsExplosive { get => hazardIsExplosive; set => hazardIsExplosive = value; }

    // Сила взрыва
    [SerializeField] private float hazardExplosionForce;
    public float HazardExplosionForce { get => hazardExplosionForce; set => hazardExplosionForce = value; }

    // Радиус взрыва
    [SerializeField] private float hazardExplosionRadius;
    public float HazardExplosionRadius { get => hazardExplosionRadius; set => hazardExplosionRadius = value; }

    #endregion

    // Уникальные параметры для щитов
    #region Shield ability properties

    // Длительность работы щита
    [SerializeField] private float shieldDuration;
    public float ShieldDuration { get => shieldDuration; set => shieldDuration = value; }

    // Долговечность щита
    [SerializeField] private float shieldDurability;
    public float ShieldDurability { get => shieldDurability; set => shieldDurability = value; }

    #endregion

    // Уникальные параметры для бустов
    #region SpeedUp ability properties

    // Продолжительность работы буста
    [SerializeField] private float speedUpDuration;
    public float SpeedUpDuration { get => speedUpDuration; set => speedUpDuration = value; }

    // Эффект от буста
    [SerializeField] private float speedUpIntensity;
    public float SpeedUpIntensity { get => speedUpIntensity; set => speedUpIntensity = value; }

    // Максимальная скорость буста
    [SerializeField] private float speedUpMaxSpeed;
    public float SpeedUpMaxSpeed { get => speedUpMaxSpeed; set => speedUpMaxSpeed = value; }

    #endregion
}