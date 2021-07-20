using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsFactory
{
    public static EffectModelView CreateSandstormEffect(string resourceString)
    {
        GameObject testTrackPrefab = Resources.Load<GameObject>(resourceString);
        EffectModelView modelView = UnityEngine.Object.Instantiate(testTrackPrefab)
            .GetComponent<EffectModelView>();
        return modelView;
    }
}
