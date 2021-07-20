using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CinemachineModelView : Singleton<CinemachineModelView>
{
    [SerializeField] private List<CinemachineVirtualCamera> virtCameras;
    [SerializeField] private CinemachineBrain cinemachineBrain;

    // Camera accessor for hp bars
    public CinemachineBrain CineCamera { get => cinemachineBrain; }

    public Transform targetPosition = null;

    [SerializeField] private GameObject trackDolly;
    [SerializeField] private CinemachineVirtualCamera trackDollyCamera;
    [SerializeField] private PlayableDirector timeline;

    [SerializeField] private CinemachineVirtualCamera backCam;

    private int currCam = 0;

    float defaultBlendTime = 0f;

    private void Start()
    {
        if (trackDolly)
        {
            trackDolly.transform.parent = targetPosition;
            trackDolly.transform.position = targetPosition.position;
            trackDolly.transform.rotation = targetPosition.rotation;
        }

        if (virtCameras.Count==0 || !targetPosition)
        {
            return;
        }

        foreach (CinemachineVirtualCamera cam in virtCameras)
        {
            cam.Follow = targetPosition;
            cam.LookAt = targetPosition;
        }

        trackDollyCamera.LookAt = targetPosition;

        backCam.LookAt = targetPosition;
        backCam.Follow = targetPosition;
        defaultBlendTime = cinemachineBrain.m_DefaultBlend.m_Time;
    }

    public void NextCam()
    {
        currCam++;
        if (currCam> virtCameras.Count-1)
        {
            currCam = 0;
        }
        SetCurrentCamera(currCam);


    }

    public void SetCurrentCamera(int camNum = 0)
    {
        for (int i = 0; i < virtCameras.Count; i++)
        {

            if (i == camNum)
            {
                virtCameras[i].Priority = 1;
            }
            else
            {
                virtCameras[i].Priority = 0;
            }
        }
    }

    public void CountdownPause(int timeInSec = 5)
    {
        StartCoroutine(StartTrackCorutine(timeInSec));
    }

    private IEnumerator StartTrackCorutine(int timeInSec)
    {
        timeline.Play();
        TimeFollowController.Instance.PauseMove();
        yield return new WaitForSecondsRealtime(timeInSec);
        SetCurrentCamera();
        TimeFollowController.Instance.ResumeMove();

    }

    public void BackViewOn()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 0f;
        backCam.Priority = 10;
    }
    public void BackViewOff()
    {
        StartCoroutine(BackCamOfCorut());
    }

    IEnumerator BackCamOfCorut()
    {
        backCam.Priority = -1;
        yield return null;
        cinemachineBrain.m_DefaultBlend.m_Time = defaultBlendTime;
    }
}
