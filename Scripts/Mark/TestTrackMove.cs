using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Тестовы класс для проверки трек системы
/// </summary>
public class TestTrackMove : MonoBehaviour
{
    public TrackPath trackPath;
    private Rigidbody rb;
    [SerializeField] private float moveForce=5f;

    private Transform currentAim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentAim = trackPath.GetStartPosition();
        //trackPath.SetObjPosition(transform, true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(currentAim.position.x, transform.position.y, currentAim.position.z));
        //rb.AddForce(transform.forward * Time.deltaTime, ForceMode.VelocityChange);
        rb.velocity = transform.forward * Time.deltaTime * moveForce + Vector3.Project(rb.velocity, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrackPoint"))
        {

            currentAim = trackPath.GetNextCheckPointAndCheckIn(transform, other.transform);
            //currentAim = trackPath.GetNextCheckPointPosition(transform);

            Debug.Log( trackPath.GetNextGatePoint(transform).ToString());
        }
    }
}
