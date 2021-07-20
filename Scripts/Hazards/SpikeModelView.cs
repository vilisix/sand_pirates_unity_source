using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeModelView : MonoBehaviour
{
    #region Fields
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private bool isHarmful;
    private float destroyTime;

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
        if (lifetime != 0)
            destroyTime = Time.time + lifetime;

        isHarmful = false;
        LaunchHazard();
    }

    private void Update()
    {
        if (destroyTime != 0)
        {
            if (Time.time > destroyTime)
                Destroy(gameObject);
        }
    }

    private void LaunchHazard()
    {
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<MonoBehaviour>(out MonoBehaviour mb))
        {
            if (mb is IDamageable && isHarmful)
            {
                ((IDamageable)mb).RecieveDamage(damage);

                if (mb.tag.Equals("Ship"))
                {
                    mb.GetComponent<Rigidbody>().velocity *= 0.5f;
                    ParticleFactory.CreateShipCollision(transform);
                }

                Destroy(gameObject);
            }
        }

        else if (collision.collider.tag.Equals("Ground"))
        {
            isHarmful = true;
        }
    }
}
