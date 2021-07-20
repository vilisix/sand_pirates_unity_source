using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;
    [SerializeField] private bool destroyOnCollision;

    private enum ObjectType
    {
        Rock,
        Other
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ship"))
        {
            if(objectType == ObjectType.Rock)
            {
                ParticleFactory.CreateRockCollision(transform);
                AudioSourceFactory.CreateSmallRangeSource(gameObject.transform, Resources.Load<AudioClip>("SFX/Collision/Rock"));
            }

            else
            {
                ParticleFactory.CreateShipCollision(transform);
            }

            if (destroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}
