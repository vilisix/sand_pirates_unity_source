using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoFactory
{
    // Обычное ядро

    public static void CreateCannonballShot(Transform origin, AbilityData data)
    {
        for (int i = 0; i < data.ProjectilesCount; i++)
        {
            GameObject shot = data.Prefab;
            CannonballShotModelView modelView = UnityEngine.Object.Instantiate(shot, origin.position, origin.rotation * GetRandomInsideCone(data.ProjectileScatter)).GetComponent<CannonballShotModelView>();
            modelView.Damage = data.ProjectileDamage;
            modelView.Speed = data.ProjectileSpeed;
            modelView.Lifetime = data.ProjectileLifetime;
        }
    }

    // Крюк-кошка

    public static void CreateHookShot(Transform origin, AbilityData data)
    {
        for (int i = 0; i < data.ProjectilesCount; i++)
        {
            GameObject shot = data.Prefab;
            HookShotModelView modelView = UnityEngine.Object.Instantiate(shot, origin.position, origin.rotation * GetRandomInsideCone(data.ProjectileScatter)).GetComponent<HookShotModelView>();
            modelView.Damage = data.ProjectileDamage;
            modelView.Speed = data.ProjectileSpeed;
        }
    }

    // Цепное ядро

    public static void CreateSeaMineShot(Transform origin, AbilityData data)
    {
        for (int i = 0; i < data.ProjectilesCount; i++)
        {
            GameObject shot = data.Prefab;
            SeaMineShotModelView modelView = UnityEngine.Object.Instantiate(shot, origin.position, origin.rotation * GetRandomInsideCone(data.ProjectileScatter)).GetComponent<SeaMineShotModelView>();
            modelView.Damage = data.ProjectileDamage;
            modelView.Speed = data.ProjectileSpeed;
            modelView.Lifetime = data.ProjectileLifetime;
            modelView.Force = data.ProjectileExplosionForce;
            modelView.Radius = data.ProjectileExplosionRadius;
        }
    }

    // Картечь

    public static void CreateGatlingShot(Transform origin, AbilityData data)
    {
        float step = data.ProjectileScatter / (data.ProjectilesCount - 1);
        float currentSpawnRotation;

        if (data.ProjectilesCount > 1)
            currentSpawnRotation = (origin.rotation.y - data.ProjectileScatter) / 2;

        else
            currentSpawnRotation = 0f;

        for (int i = 0; i < data.ProjectilesCount; i++)
        {
            GameObject shot = data.Prefab;
            GatlingShotModelView modelView = UnityEngine.Object.Instantiate
                (shot, origin.position, origin.rotation * Quaternion.Euler(0f, currentSpawnRotation, 0f)).GetComponent<GatlingShotModelView>();

            modelView.Damage = data.ProjectileDamage;
            modelView.Speed = data.ProjectileSpeed;
            modelView.Lifetime = data.ProjectileLifetime;

            currentSpawnRotation += step;
        }
    }

    private static Quaternion GetRandomInsideCone(float conicAngle)
    {
        Quaternion randomTilt = Quaternion.AngleAxis(Random.Range(0f, conicAngle), Vector3.up);
        Quaternion randomSpin = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

        return (randomSpin * randomTilt);
    }
}
