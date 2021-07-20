using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilModelView : MonoBehaviour
{
    #region Fields

    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody rb;
    
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private float destroyTime;

    private bool isSplattered;
    private GameObject splatter;

    #endregion

    #region Accessors

    public Rigidbody Rigidbody { get => rb; }

    public float Damage
    {
        get => damage;
        set
        {
            if (damage != value)
            {
                damage = value;
            }
        }
    }

    public float Speed
    {
        get => speed;
        set
        {
            if (speed != value)
            {
                speed = value;
            }
        }
    }

    public float Lifetime
    {
        get => lifetime;
        set
        {
            if (lifetime != value)
            {
                lifetime = value;
            }
        }
    }

    #endregion

    private void Start()
    {
        LaunchHazard();
    }

    private void Update()
    {
        if (destroyTime != 0)
            if (Time.time > destroyTime)
            {
                Destroy(splatter);
                Destroy(gameObject);
            }
    }

    private void LaunchHazard()
    {
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isSplattered == false)
            if (collision.collider.tag.Equals("Ground"))
            {
                isSplattered = true;
                destroyTime = Time.time + lifetime;

                meshRenderer.enabled = false;
                sphereCollider.enabled = false;
                rb.isKinematic = true;
                
                splatter = HazardFactory.CreateOilSplatter(transform);

                int sound = Random.Range(0, 2);

                switch (sound)
                {
                    case 0: AudioSourceFactory.CreateSmallRangeSource(transform, Resources.Load<AudioClip>("SFX/Explosion/OilBombDrop_01")); break;
                    case 1: AudioSourceFactory.CreateSmallRangeSource(transform, Resources.Load<AudioClip>("SFX/Explosion/OilBombDrop_02")); break;
                }
            }
    }
}
