using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBombModelView : MonoBehaviour
{
    #region Fields
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private float force;
    [SerializeField] private float radius;

    private float destroyTime;

    private Collider[] affectedColliders;
    private HashSet<GameObject> affectedObjects;

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

    public float Force
    {
        get => force;
        set
        {
            if (force != value)
            {
                force = value;
            }
        }
    }

    public float Radius
    {
        get => radius;
        set
        {
            if (radius != value)
            {
                radius = value;
            }
        }
    }

    #endregion

    private void Start()
    {
        affectedObjects = new HashSet<GameObject>();

        if (lifetime != 0)
            destroyTime = Time.time + lifetime;

        LaunchHazard();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void Update()
    {
        if (destroyTime != 0)
        {
            if (Time.time > destroyTime)
            {
                Explode();
            }

            else
            {
                affectedColliders = Physics.OverlapSphere(transform.position, radius);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                destroyTime = Time.time;
            }
        }
    }

    private void LaunchHazard()
    {
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void Explode()
    {
        if(affectedColliders != null)
        {
            foreach (Collider collider in affectedColliders)
            {
                if (!affectedObjects.Contains(collider.gameObject))
                {
                    affectedObjects.Add(collider.gameObject);
                }
            }

            foreach (GameObject obj in affectedObjects)
            {
                if (obj.TryGetComponent<MonoBehaviour>(out MonoBehaviour mb))
                {
                    if (mb is IDamageable)
                    {
                        mb.TryGetComponent<Rigidbody>(out Rigidbody rb);
                        rb.AddExplosionForce(force, transform.position, radius, 10f, ForceMode.Impulse);
                        rb.AddRelativeTorque(Vector3.up * Random.Range(-1000f, 1000f), ForceMode.Impulse);
                        rb.velocity /= 2f;

                        Vector3 closestPoint = rb.ClosestPointOnBounds(transform.position);
                        float distance = Vector3.Distance(closestPoint, transform.position);

                        float calculatedDamage = 1.0f - Mathf.Clamp01(distance / radius);
                        calculatedDamage *= damage;

                        ((IDamageable)mb).RecieveDamage(Mathf.Round(calculatedDamage));
                        Debug.Log($"{obj.name} takes {calculatedDamage} damage! from {name}");
                    }
                }
            }
        }
        Destroy(gameObject);
        ParticleFactory.CreateBigExplosion(transform);
        AudioSourceFactory.CreateLargeRangeSource(transform, Resources.Load<AudioClip>("SFX/Explosion/ExplosionMedium"));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Ship"))
        {
            destroyTime = Time.time;
        }

        else if (collision.collider.tag.Equals("Ground"))
        {
            ParticleFactory.CreateSandExplosion(transform);
        }
    }
}
