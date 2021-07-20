using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerPointer : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Start()
    {
        cam = CinemachineModelView.Instance.CineCamera.OutputCamera;

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
    }
}
