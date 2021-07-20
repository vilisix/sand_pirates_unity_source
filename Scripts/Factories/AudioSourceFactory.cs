using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceFactory
{
    public static void CreateSmallRangeSource(Transform origin, AudioClip clip)
    {
        GameObject obj = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Audio/SmallRangeSource"), origin.position, Quaternion.identity);
        AudioSource src = obj.GetComponent<AudioSource>();

        src.PlayOneShot(clip);
        float clipLength = clip.length;

        Object.Destroy(obj, clipLength);
    }

    public static void CreateMediumRangeSource(Transform origin, AudioClip clip)
    {
        GameObject obj = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Audio/MediumRangeSource"), origin.position, Quaternion.identity);
        AudioSource src = obj.GetComponent<AudioSource>();

        src.PlayOneShot(clip);
        float clipLength = clip.length;

        Object.Destroy(obj, clipLength);
    }

    public static void CreateLargeRangeSource(Transform origin, AudioClip clip)
    {
        GameObject obj = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Audio/LargeRangeSource"), origin.position, Quaternion.identity);
        AudioSource src = obj.GetComponent<AudioSource>();

        src.PlayOneShot(clip);
        float clipLength = clip.length;

        Object.Destroy(obj, clipLength);
    }
}