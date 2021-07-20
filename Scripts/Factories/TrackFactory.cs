using System;
using Track;
using UnityEngine;

// фабрика трасс, создает заранее подготовленные трассы из папки Resources, либо процедурно сгенерированные
    public class TrackFactory
    {
        // public static TrackModelView CreateTestTrackModelView()
        // {
        //     GameObject testTrackPrefab = Resources.Load<GameObject>("Prefabs/Track/MarkTestTrack");
        //     TrackModelView modelView = UnityEngine.Object.Instantiate(testTrackPrefab)
        //         .GetComponent<TrackModelView>();
        //     return modelView;
        // }
        //
        // public static TrackPath CreateTestTrackPath()
        // {
        //     GameObject testTrackPathPrefab = Resources.Load<GameObject>("Prefabs/Track/CheckpointsPath");
        //     TrackPath path = UnityEngine.Object.Instantiate(testTrackPathPrefab)
        //         .GetComponent<TrackPath>();
        //     return path;
        // }
        
        public static TrackModelView CreateBigTrackModelView(string resourceString)
        {
            GameObject testTrackPrefab = Resources.Load<GameObject>(resourceString);
            TrackModelView modelView = UnityEngine.Object.Instantiate(testTrackPrefab)
                .GetComponent<TrackModelView>();
            return modelView;
        }

        public static TrackPath CreateBigTrackPath(string resourceString, int lapsCount)
        {
            GameObject testTrackPathPrefab = Resources.Load<GameObject>(resourceString);
            testTrackPathPrefab.GetComponent<TrackPath>().countOfLaps = lapsCount;
            TrackPath path = UnityEngine.Object.Instantiate(testTrackPathPrefab)
                .GetComponent<TrackPath>();
            return path;
        }

        public static AbilityContainerModelView CreateAbilityContainer(Vector3 position, string resourceString)
        {
            GameObject containerPrefab = Resources.Load<GameObject>(resourceString);
            AbilityContainerModelView modelView = UnityEngine.Object.Instantiate(containerPrefab,position,Quaternion.identity)
                .GetComponent<AbilityContainerModelView>();
            return modelView;
        }

        public static StartPlacerModelView CreateStartPlacer(Transform startGate, string resourceString)
        {
            GameObject placerPrefab = Resources.Load<GameObject>(resourceString);
            StartPlacerModelView modelView = UnityEngine.Object.Instantiate(placerPrefab, startGate.position,startGate.rotation)
                .GetComponent<StartPlacerModelView>();
            return modelView;
        }

        public static GameObject CreateMainMenuTrack(string resourceString)
        {
            GameObject trackPrefab = Resources.Load<GameObject>(resourceString);
            return UnityEngine.Object.Instantiate(trackPrefab);
        }
    }