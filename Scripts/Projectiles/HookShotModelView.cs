using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShotModelView : MonoBehaviour
{
    #region Fields

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float damage;
    [SerializeField] private float speed;

    [SerializeField] private bool isHarmful;

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

    #endregion

    private void Start()
    {
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        rb.useGravity = false;

        isHarmful = true;
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);

        StartCoroutine(DelayedDestroy(2f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<MonoBehaviour>(out MonoBehaviour mb))
        {
            if (mb is IDamageable && isHarmful)
            {
                ((IDamageable)mb).RecieveDamage(damage);
                Debug.Log($"{collision.collider.name} takes {damage} damage! from {name}");

                if (mb.tag.Equals("Ship"))
                    ParticleFactory.CreateShipCollision(transform);
            }
        }

        else if (collision.collider.tag.Equals("Ground"))
            ParticleFactory.CreateSandExplosion(transform);

        isHarmful = false;
        rb.useGravity = true;
        rb.velocity /= 2f;
    }

    private IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.useGravity = true;

        yield return new WaitForSeconds(delay);

        UnityEngine.Object.Destroy(gameObject);
    }
}
