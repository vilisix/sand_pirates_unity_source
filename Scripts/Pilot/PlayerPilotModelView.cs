using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerPilotModelView : MonoBehaviour
{
    public event EventHandler<Vector3> OnMovingInput = (sender, e) => { };
    public event EventHandler<Vector3> OnActionInput = (sender, e) => { };
    public event EventHandler<Transform> OnTriggerCollision = (sender, checkPointTransform) => { };

    [SerializeField] private SkinnedMeshRenderer pirateRenderer;
    public SkinnedMeshRenderer PirateRenderer { get => pirateRenderer; }

    private void FixedUpdate()
    {
        OnMovingInput(this, new Vector3(InputParams.XAxis, 0, InputParams.ZAxis));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerCollision(this, other.transform);
    }
}
