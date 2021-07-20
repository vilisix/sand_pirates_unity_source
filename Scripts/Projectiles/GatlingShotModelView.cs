using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingShotModelView : MonoBehaviour
{
    #region Fields

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    [SerializeField] private bool isHarmful;

    private bool isFloating;

    private RaycastHit startHeight;
    private RaycastHit currentHeight;

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
        LaunchProjectile();
    }

    private void Update()
    {
        if (isFloating)
        {
            Physics.Raycast(transform.position, Vector3.down, out currentHeight, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground"));

            if (currentHeight.distance != startHeight.distance)
                rb.MovePosition(new Vector3(rb.transform.position.x, rb.transform.position.y - currentHeight.distance + startHeight.distance, rb.transform.position.z));
        }
    }

    private void LaunchProjectile()
    {
        isFloating = true;
        rb.useGravity = false;

        Physics.Raycast(transform.position, Vector3.down, out startHeight);

        isHarmful = true;
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);

        StartCoroutine(DelayedDestroy(lifetime));
    }

    private IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        isFloating = false;
        rb.useGravity = true;

        yield return new WaitForSeconds(delay);

        UnityEngine.Object.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<MonoBehaviour>(out MonoBehaviour mb))
        {
            if (mb is IDamageable && isHarmful)
            {
                ((IDamageable)mb).RecieveDamage(damage);
                Debug.Log($"{collision.collider.name} takes {damage} damage! from {name}");

                mb.TryGetComponent<Rigidbody>(out Rigidbody rb);
                rb.AddExplosionForce(75f, transform.position, 3f, 10f, ForceMode.Impulse);
                rb.AddRelativeTorque(Vector3.up * Random.Range(-1000f, 1000f), ForceMode.Impulse);

                if (mb.tag.Equals("Ship"))
                {
                    Destroy(gameObject);
                    ParticleFactory.CreateShipCollision(transform);
                }
            }
        }

        else if (collision.collider.tag.Equals("Ground"))
            ParticleFactory.CreateSandExplosion(transform);

        isHarmful = false;

        isFloating = false;
        rb.useGravity = true;
        rb.velocity /= 2f;
    }
}
