using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс создает точки по треку, в которых будут генерироваться бусты и прочие активности.
/// </summary>
public class EntityOnTrackCreator : MonoBehaviour
{
    private TrackPath trackPath;
    
    /// <summary>
    /// префаб точки, которая будет генерировать бусты
    /// </summary>
    [SerializeField] private GameObject pointCreator;

    /// <summary>
    /// Связка точки спавна объектов и созданного объекта. 
    /// </summary>
    private Dictionary<GameObject, GameObject> pointsAndCreations;

    /// <summary>
    /// Создавать в точках генерации указанные префабы
    /// </summary>
    [SerializeField] private bool generateConstantly = true;

    [SerializeField] private float timeToCorutineCreator = 5f;

    /// <summary>
    /// Список создаваемых объектов со свойствами
    /// </summary>
    [SerializeField] private List<GameobjectsToCreate> creation;

    /// <summary>
    /// Список точек спавна объектов и соответствий префабов которые будут генерироваться в этой точке 
    /// </summary>
    private Dictionary<GameObject, GameobjectsToCreate> pointsAndPrefabs;

    /// <summary>
    /// список всех точек спавна объектов
    /// </summary>
    private List<GameObject> listOfPoints;

    /// <summary>
    /// класс содержащий перфаб и его свойства для спавна
    /// </summary>
    [Serializable]
    private class GameobjectsToCreate
    {
        public enum TypeOfCreation
        {
            simple,
            radius,
            oneSide
        }
        /// <summary>
        /// префаб спавна
        /// </summary>
        public GameObject gameObj;
        /// <summary>
        /// количество создаваемых префабов на трек
        /// </summary>
        public int count;
        /// <summary>
        /// расстояние от земли на котором создается объект
        /// </summary>
        public float distFromGround = 0.5f;
        
        public TypeOfCreation typeOfCreation = TypeOfCreation.simple;
        /// <summary>
        /// радиус рандомного спавна
        /// </summary>
        public float radius = 10f;
        
    }


    private void Start()
    {
        if (creation.Count==0)
        {
            return;
        }

        trackPath = GetComponent<TrackPath>();
        pointsAndPrefabs = new Dictionary<GameObject, GameobjectsToCreate>();
        pointsAndCreations = new Dictionary<GameObject, GameObject>();
        listOfPoints = new List<GameObject>();


        //определяем количество создаваемых объектов
        int sumPointToCreate = 0;
        foreach (GameobjectsToCreate gameobjectsToCreate in creation)
        {
            sumPointToCreate += gameobjectsToCreate.count;
        }

        //Длина интервала между точками спавна объектов
        float entitieDistance = trackPath.GetTrackDistance() / (sumPointToCreate + 1);
        float currentDistance = 0.5f;

        //создаём точки спавна объектов
        for (int i = 0; i < sumPointToCreate; i++)
        {
            currentDistance += entitieDistance;

            KeyValuePair<Transform, float>  cKVP = trackPath.GetChekpointThrouDistance(currentDistance);

            
            Vector3 curPoint = cKVP.Key.position;
            Vector3 nextPoint = trackPath.GetNextCheckPointPosition(cKVP.Key).position;
            float m1 = currentDistance - cKVP.Value;
            float m2 = Vector3.Distance(curPoint, nextPoint)- m1;

            float x = (m2 * curPoint.x + m1 * nextPoint.x) / (m1 + m2);
            float y = (m2 * curPoint.y + m1 * nextPoint.y) / (m1 + m2);
            float z = (m2 * curPoint.z + m1 * nextPoint.z) / (m1 + m2);

           

            Vector3 posToCreate = new Vector3(x, y+100f,z);

            GameObject pointOfCreation = Instantiate(pointCreator, posToCreate, Quaternion.identity, gameObject.transform);

            Vector3 posToLookAt = nextPoint;
            posToLookAt.y = pointOfCreation.transform.position.y;

            pointOfCreation.transform.LookAt(posToLookAt);

            pointsAndCreations.Add(pointOfCreation, default);
            listOfPoints.Add(pointOfCreation);

        }

        ///создаём Список всех создаваемых префабов по их количеству
        List<GameobjectsToCreate> creaturesCountList= new List<GameobjectsToCreate>();
        foreach (var item in creation)
        {
            for (int i = 0; i < item.count; i++)
            {
                creaturesCountList.Add( item);
            }
        }

        //раздаём точкам тип префаба
        foreach (GameObject pointOfSpawn in listOfPoints)
        {

            int curRnd = UnityEngine.Random.Range(0, creaturesCountList.Count);

            pointsAndPrefabs.Add(pointOfSpawn, creaturesCountList[curRnd]);
            creaturesCountList.RemoveAt(curRnd);
        }

        CheckAndCreateEntities();

        if (generateConstantly)
        {
            StartCoroutine(CreateEntitiesCorut());
        }


    }


    /// <summary>
    /// Создает объект в указанной точке на поверхности
    /// </summary>
    /// <param name="gObj">префаб</param>
    /// <param name="distFromGround">расстояние от земли на котором создается префаб</param>
    /// <param name="randomDistanceFromCenter">максимальная дистация от центра на котором создаются объекты</param>
    private GameObject CreateObjOnGround(GameobjectsToCreate gObjStruct, GameObject pointOfCreate)
    {

        
        Vector3 posOfPiu = new Vector3(pointOfCreate.transform.position.x, pointOfCreate.transform.position.y, pointOfCreate.transform.position.z);

        if (gObjStruct.typeOfCreation==GameobjectsToCreate.TypeOfCreation.radius)
        {
            float xrnd = UnityEngine.Random.Range(-gObjStruct.radius, gObjStruct.radius);
            posOfPiu += pointOfCreate.transform.right * xrnd;

        }
        else if (gObjStruct.typeOfCreation == GameobjectsToCreate.TypeOfCreation.oneSide)
        {
            posOfPiu += pointOfCreate.transform.right * gObjStruct.radius;
        }
        

        RaycastHit raycastHit = new RaycastHit();
        bool wasTouch = Physics.Raycast(posOfPiu, Vector3.down, out raycastHit);
        if (wasTouch)
        {
            Vector3 pointToSpawn = raycastHit.point;
            pointToSpawn.y += gObjStruct.distFromGround;

            return Instantiate(gObjStruct.gameObj, pointToSpawn, pointOfCreate.transform.rotation, gameObject.transform);

        }else
        {
            return default;
        }
    }

    IEnumerator CreateEntitiesCorut()
    {
        while (generateConstantly)
        {
            yield return new WaitForSecondsRealtime(timeToCorutineCreator);
            CheckAndCreateEntities();
        }
        
    }

    /// <summary>
    /// Метод проверяет и создает активности в точках генерации
    /// </summary>
    private void CheckAndCreateEntities()
    {
        List<GameObject> listOfPointWithoutPrefab = new List<GameObject>();
        foreach (KeyValuePair<GameObject, GameObject> point in pointsAndCreations)
        {
            if (point.Value == default)
            {
                listOfPointWithoutPrefab.Add(point.Key);
                
            }
        }
        foreach (GameObject item in listOfPointWithoutPrefab)
        {
            GameobjectsToCreate gobjStruct = pointsAndPrefabs[item];
            GameObject newEntity = CreateObjOnGround(gobjStruct, item);
            if (newEntity)
            {
                EntitiyDeleter entitiyDeleter = newEntity.AddComponent<EntitiyDeleter>();
                entitiyDeleter.SetParentCreator(this);
                pointsAndCreations[item] = newEntity;
            }
        }
            
    }



    /// <summary>
    /// Удаляет объект из списка существующих, после этого может создваться новый буст
    /// </summary>
    /// <param name="objToRemove">объект префаб</param>
    public void RemoveObjectFromSlot(GameObject objToRemove)
    {

        if (pointsAndCreations.ContainsValue(objToRemove))
        {
            GameObject myKey = pointsAndCreations.FirstOrDefault(x => x.Value == objToRemove).Key;
            pointsAndCreations[myKey] = default;
        }
        
    }

}


