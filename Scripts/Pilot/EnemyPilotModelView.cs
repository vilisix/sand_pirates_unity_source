using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyPilotModelView : MonoBehaviour
{
    public event EventHandler OnMovingInput = (sender, e) => { };
    public event EventHandler<Vector3> OnActionInput = (sender, e) => { };
    public event EventHandler<Transform> OnTriggerCollision = (sender, checkPointTransform) => { };

    private float moveH, moveV;
    private Vector3 actionDirection;
    
    public Vector3 ChechpointTarget { get; set; }

    [SerializeField] private SkinnedMeshRenderer pirateRenderer;
    public SkinnedMeshRenderer PirateRenderer { get => pirateRenderer; }

    private void FixedUpdate()
    {
        OnMovingInput(this, EventArgs.Empty);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerCollision(this, other.transform);
    }
    
    /// <summary>
    /// Вызывает корутину с рандомным временем задержки
    /// </summary>
    /// <param name="minDelay"></param>
    /// <param name="maxDelay"></param>
    public void InvokeSecondaryAbilityAfterDelay(float minDelay, float maxDelay)
    {
        float delay = UnityEngine.Random.Range(minDelay, maxDelay);
        StartCoroutine(InvokeSecondaryAction(delay));
    }
    
    IEnumerator InvokeSecondaryAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnActionInput(this, Vector3.back);
    }

    #region Gizmos

    

    // Drawing gizmos for obstacle avoid system (test)
    void OnDrawGizmos()
    {
        float maxDistance = 100f;
        RaycastHit hit;
        LayerMask TrackEntityMask = LayerMask.GetMask("AICastedEntity");
        LayerMask groundMask = LayerMask.GetMask("Ground");
        Gizmos.color = Color.red;
        
        if (ChechpointTarget != null)
        {
            Vector3 checkpointDirection = new Vector3((ChechpointTarget - transform.position).normalized.x, 0,
                                              (ChechpointTarget - transform.position).normalized.z) * 2f;
            Vector3 rightDirection = new Vector3(transform.right.x, 0, transform.right.z) * 10;
            Vector3 leftDirection = new Vector3(-transform.right.x, 0, -transform.right.z) * 10;

            //forward cast
            bool isLandCast = Physics.Raycast(transform.position + (checkpointDirection + Vector3.up).normalized * 20, 
                Vector3.down, out hit, maxDistance, groundMask);
            if (isLandCast)
            {
                Gizmos.DrawLine(transform.position + (checkpointDirection + Vector3.up).normalized * 20, hit.point);
                Gizmos.DrawWireSphere(hit.point, transform.lossyScale.x);
                Gizmos.DrawLine(transform.position, hit.point);
                
                float pilotToCastPointDistance = Vector3.Distance(transform.position, hit.point);
                bool isHit = Physics.SphereCast(transform.position, transform.lossyScale.x / 2, hit.point, out hit, pilotToCastPointDistance, TrackEntityMask);
                if (isHit)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(hit.point, transform.lossyScale.x/2);
                    Gizmos.DrawLine(transform.position, hit.point);
                    Gizmos.color = Color.green;
                    
                    Gizmos.DrawWireCube(hit.point + rightDirection, transform.lossyScale/2);
                    Gizmos.DrawWireCube(hit.point + leftDirection, transform.lossyScale/2);

                }
            }
            

            /*//right cast
            isHit = Physics.BoxCast(
                transform.position + (forwardDirection + Vector3.up + rightDirection).normalized * 20,
                transform.lossyScale / 2, Vector3.down, out hit,
                transform.rotation, maxDistance, mask);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(hit.point, transform.lossyScale);
                Gizmos.DrawLine(transform.position,
                    transform.position + (forwardDirection + Vector3.up + rightDirection).normalized * 20);
                Gizmos.DrawLine(transform.position + (forwardDirection + Vector3.up + rightDirection).normalized * 20,
                    hit.point);
            }

            //left cast
            isHit = Physics.BoxCast(
                transform.position + (forwardDirection + Vector3.up + -rightDirection).normalized * 20,
                transform.lossyScale / 2, Vector3.down, out hit,
                transform.rotation, maxDistance, mask);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(hit.point, transform.lossyScale);
                Gizmos.DrawLine(transform.position,
                    transform.position + (forwardDirection + Vector3.up + -rightDirection).normalized * 20);
                Gizmos.DrawLine(transform.position + (forwardDirection + Vector3.up + -rightDirection).normalized * 20,
                    hit.point);
            }*/
        }
    }
    
    #endregion
}
