    using UnityEngine;

    public class CameraFactory
    {
        public static CinemachineModelView CreateCameraRig(Transform playerTransform)
        {
            ///Переделал на Cimemashine //Mark
            // Получаем GO рига
            GameObject cinemachinePrefab = Resources.Load<GameObject>("Prefabs/Camera/CineMachineSet");
            // Создаем инстанс и получаем модель-представление рига
            CinemachineModelView modelView = UnityEngine.Object.Instantiate(cinemachinePrefab).GetComponent<CinemachineModelView>();

            modelView.targetPosition = playerTransform;

            return modelView;
        }
    }
