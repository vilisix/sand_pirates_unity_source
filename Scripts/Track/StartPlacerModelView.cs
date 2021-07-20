using System;
using System.Collections.Generic;
using UnityEngine;

//класс расстановщик стартовых мест
    public class StartPlacerModelView : MonoBehaviour
    {
        [SerializeField] private List<Transform> placePoints;
        
        public Transform GetSpawnPoint(int index)
        {
            return placePoints[index];
        }
    }
