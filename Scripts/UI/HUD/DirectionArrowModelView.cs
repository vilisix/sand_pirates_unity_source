using System;
using UnityEngine;


    public class DirectionArrowModelView : MonoBehaviour
    {
        public RectTransform ArrowTransform;
        public RectTransform ArrowOutlineTransform;
        public Transform CheckpointDirection { get; set; }
        public Transform ShipDirection { get; set; }

        private void Update()
        {
            if (CheckpointDirection != null && ShipDirection != null)
            {
                Vector3 direction = CheckpointDirection.position - 
                CinemachineModelView.Instance.CineCamera.ActiveVirtualCamera.VirtualCameraGameObject.transform.position;
                float arrowAngle = AngleSigned(ShipDirection.forward, direction, Vector3.up);
                ArrowTransform.rotation = Quaternion.Euler(0, 0, -arrowAngle);
				ArrowOutlineTransform.rotation = Quaternion.Euler(0, 0, -arrowAngle);
            }
        }
        
        private float AngleSigned(Vector3 v1, Vector3 v2, Vector3 normal)
        {
            return Mathf.Atan2(Vector3.Dot(normal, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }
    }
