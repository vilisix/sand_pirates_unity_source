using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFactory
{
    // Взрыв песка

    public static void CreateSandExplosion(Transform origin)
    {
        ParticleSystem particle = Resources.Load<ParticleSystem>("Particles/Explosions/SandExplosion");
        UnityEngine.Object.Instantiate(particle, origin.position, Quaternion.identity);
    }

    // Мусор от корабля при столкновении

    public static void CreateShipCollision(Transform origin)
    {
        ParticleSystem particle = Resources.Load<ParticleSystem>("Particles/ShipCollision");
        UnityEngine.Object.Instantiate(particle, origin.position, Quaternion.identity);
    }

    // Камушки от валуна при столкновении

    public static void CreateRockCollision(Transform origin)
    {
        ParticleSystem particle = Resources.Load<ParticleSystem>("Particles/RockCollision");
        UnityEngine.Object.Instantiate(particle, origin.position, Quaternion.identity);
    }

    //Дым из пушки

    public static void CreateShotSmoke(Transform origin)
    {
        ParticleSystem particle = Resources.Load<ParticleSystem>("Particles/ShotSmoke");
        UnityEngine.Object.Instantiate(particle, origin.position, origin.rotation);
    }

    // Малый взрыв
    public static GameObject CreateSmallExplosion(Transform origin)
    {
        GameObject explosion = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Particles/Explosions/SmallExplosion"), origin.position, Quaternion.identity);
        return explosion;
    }

    // Большой взрыв
    public static GameObject CreateBigExplosion(Transform origin)
    {
        GameObject explosion = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Particles/Explosions/BigExplosion"), origin.position, Quaternion.identity);
        return explosion;
    }

    public static GameObject CreateShipDestroyedSmoke(Transform origin)
    {
        GameObject smoke = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Particles/ShipDestroyedSmoke"), origin.position + origin.forward * -3f + origin.up * 3f, Quaternion.identity, origin);
        return smoke;

    }
}