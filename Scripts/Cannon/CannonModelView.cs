using System.Collections;
using System.Linq;
using UnityEngine;

public class CannonModelView : MonoBehaviour, ICannonModelView
{
    [SerializeField] private Transform shotOrigin;

    [SerializeField] bool aimEnabled = false;

    [SerializeField] private LayerMask targetLayers;

    [SerializeField] private float aimDistance = 30f;
    [SerializeField] private float aimRadius = 0.5f;
    [SerializeField] private float aimDelay = 0.1f;

    private BoxCollider[] selfColliders;

    private void Start()
    {
        if (aimEnabled)
        {
            selfColliders = GetComponentsInParent<BoxCollider>();
            StartCoroutine(AimTarget());
        }
    }

    public void Fire(IAbility ability)
    {
        ability.Execute(shotOrigin);
    }

    private IEnumerator AimTarget()
    {
        bool targetFound;
        RaycastHit hit;

        Vector3 target;

        while (true)
        {
            targetFound = Physics.SphereCast(shotOrigin.position, 2f, transform.forward, out hit, aimDistance, targetLayers);

            if (targetFound && selfColliders.Contains(hit.collider) == false)
            {
                target = hit.collider.bounds.center - transform.position;

                if (transform.localRotation.y < aimRadius && transform.localRotation.y > -aimRadius)
                    transform.rotation = Quaternion.LookRotation(target);

                else
                    transform.localRotation = Quaternion.identity;
            }

            else
                transform.localRotation = Quaternion.identity;

            yield return new WaitForSeconds(aimDelay);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    float maxDistance = 30f;
    //    RaycastHit hit;

    //    bool isHit = Physics.SphereCast(shotOrigin.position, 2f, transform.forward, out hit, maxDistance, visibleLayers);

    //    if (isHit)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawRay(shotOrigin.position, transform.forward * hit.distance);
    //        Gizmos.DrawWireSphere(hit.collider.bounds.center, 2f);
    //    }

    //    else
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
    //    }
    //}
}