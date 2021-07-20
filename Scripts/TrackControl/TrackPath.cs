using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Data;
using System;

//[Serializable]
/// <summary>
/// Класс трек системы чекпоинтов.
/// Следит за контролем прохождения АИ объектов через навигационные чекпоинты.
/// Контроль прохождения объектов через видимые чекпоинты и регистрации на них, 
/// для составления таблицы лидеров
/// </summary>
public class TrackPath : MonoBehaviour
{
    [SerializeField] public List<Transform> trackPoints;
    public bool isLooped = false;
    public int countOfLaps = 1;

    private bool isActive = false;

    private List<KeyValuePair<Transform, float>> checkPointAndDistance;

    /// <summary>
    /// Список точек, в которых есть ворота, через них идёт регистрация прохождения
    /// </summary>
    [SerializeField] public List<Transform> gatePoints;

    /// <summary>
    /// Список точек, в которых есть ворота выстроенный по порядку для прохождения, через них идёт регистрация прохождения
    /// </summary>
    private List<Transform> gatePointsFollow;

    /// <summary>
    /// соответствие пилота и его текущей видимой точки, ворот
    /// </summary>
    private Dictionary<Transform, int> pilotsAndCurrentGatePoint;

    /// <summary>
    /// Ключ - номер чекпоинта с воротами из списка gatePointsFollow и структура для фиксации прохождения
    /// </summary>
    private List<KeyValuePair<int, TrackLeaderTableStruct>> trackLeaderBoard;

    /// <summary>
    /// Таблица содержит ключ значение для определения финишировал ли пилот
    /// </summary>
    private Dictionary<Transform, bool> hasPilotFinished;

    public event EventHandler OnFinish = (sender, e) => { };

    /// <summary>
    /// Победа в гонке
    /// </summary>
    [HideInInspector] public bool isWin = false;

    [SerializeField] private GameObject checkPointHightLight;
    private GameObject checkPointHightLightInstce;
    private float heightOfHighLight = 5f;

    Transform player;
    private ShipModelView playerShMV;
    List<ShipModelView> enemiesShipModelView;
    [SerializeField] private float distToAutoBoost = 10f;
    [SerializeField] private float aimHelperInterest = 2f;
    [SerializeField] private float aimOfsetCoef = 0.5f;
    [SerializeField] private float timerForBoostCorutine = 5f;

    private DateTime startTime;

    Dictionary<Transform, float> shipsAndDistance;

    [SerializeField] private ShipDriveParams basicShipDriveParams;
    [SerializeField] private ShipDriveParams boostDriveParams;
    

    private void Awake()
    {
        if (trackPoints.Count > 1)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }

        pilotsAndCurrentGatePoint = new Dictionary<Transform, int>();
        trackLeaderBoard = new List<KeyValuePair<int, TrackLeaderTableStruct>>();
        hasPilotFinished = new Dictionary<Transform, bool>();

        SetCheckpointDistance();
        MakeGateFollow();

        checkPointHightLightInstce = Instantiate(checkPointHightLight);

        checkPointHightLightInstce.transform.parent = this.transform;


        if (gatePointsFollow.Count>0)
        {
            checkPointHightLightInstce.transform.position = gatePointsFollow[0].position;
        }
        else
        {
            checkPointHightLightInstce.transform.position = trackPoints[0].position;
        }

        enemiesShipModelView = new List<ShipModelView>();
        startTime = DateTime.Now;

        shipsAndDistance = new Dictionary<Transform, float>();

    }

    private void Start()
    {
        StartCoroutine(TrackEnemyHelperCorut(timerForBoostCorutine));
    }

    /// <summary>
    /// Метод устанавливает связь чекпоинт - дистанция от начала прохождения в list checkPointAndDistance
    /// </summary>
    private void SetCheckpointDistance()
    {
        checkPointAndDistance = new List<KeyValuePair<Transform, float>>();
        checkPointAndDistance.Add(new KeyValuePair<Transform, float>(trackPoints[0], 0f));
        float trackDist = 0f;
        for (int i = 1; i < trackPoints.Count; i++)
        {
            trackDist += Vector3.Distance(trackPoints[i - 1].position, trackPoints[i].position);

            checkPointAndDistance.Add(new KeyValuePair<Transform, float>(trackPoints[i], trackDist));
        }

        //для кольцевой последнюю дистанцию добавляем в ручном режиме
        if (isLooped && trackPoints.Count > 1)
        {
            trackDist += Vector3.Distance(trackPoints[trackPoints.Count - 1].position, trackPoints[0].position);
            checkPointAndDistance.Add(new KeyValuePair<Transform, float>(trackPoints[0], trackDist));
        }

    }


    /// <summary>
    /// Визуальное отображение для редкатора сцены
    /// </summary>
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        if (trackPoints == null)
        {
            return;
        }

        Color color;
        if (trackPoints.Count > 0)
        {
            color = Color.blue;
            color.a = 0.2f;
            Gizmos.color = color;
            Gizmos.DrawSphere(trackPoints[0].position, 20f);
        }
        
        color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;
        for (int i = 1; i < trackPoints.Count; i++)
        {
            if (!trackPoints[i]|| !trackPoints[i-1])
            {
                continue;
            }

            if (i == trackPoints.Count - 1 && !isLooped)
            {
                color = Color.red;
                color.a = 0.2f;
                Gizmos.color = color;
                Gizmos.DrawSphere(trackPoints[i].position, 20f);
            }
            else
            {
                Gizmos.DrawSphere(trackPoints[i].position, 20f);
            }


            Gizmos.DrawLine(trackPoints[i - 1].position, trackPoints[i].position);

        }

        if (isLooped && trackPoints.Count > 1)
        {
            if (trackPoints[trackPoints.Count - 1] && trackPoints[0])
            {
                Gizmos.DrawLine(trackPoints[trackPoints.Count - 1].position, trackPoints[0].position);
            }
           
        }
    }

    /// <summary>
    /// Метод возвращает стартовую позицию трека
    /// </summary>
    /// <returns></returns>
    public Transform GetStartPosition()
    {
        if (isActive)
        {
            return trackPoints[0];
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Метод возвращает ближайший чекпоинт трассы
    /// </summary>
    /// <param name="objectPosition">Позиция объекта для определения ближайшей точки</param>
    /// <returns></returns>
    public Transform GetNearCheckPointPosition(Transform objectPosition)
    {
        if (isActive)
        {
            List<KeyValuePair<Transform, float>> flotDistList = new List<KeyValuePair<Transform, float>>();

            for (int i = 0; i < trackPoints.Count; i++)
            {
                flotDistList.Add(new KeyValuePair<Transform, float>(trackPoints[i], Vector3.Distance(objectPosition.position, trackPoints[i].position)));
            }
            flotDistList.Sort((x, y) => (y.Value.CompareTo(x.Value)));
            return flotDistList[flotDistList.Count - 1].Key;

        }
        else
        {
            return default;
        }

    }

    /// <summary>
    /// Класс выстраивает последовательность ворот для прохождения
    /// берёт все ворота из gatePoints
    /// порядок ворот выстраивает из trackPoints
    /// </summary>
    private void MakeGateFollow()
    {

        gatePointsFollow = new List<Transform>();
        for (int i = 1; i <= countOfLaps; i++)
        {
            foreach (Transform trackPoint in trackPoints)
            {
                if (gatePoints.Contains(trackPoint))
                {
                    gatePointsFollow.Add(trackPoint);
                }
            }
            if (!isLooped)
            {
                break;
            }
        }
        
    }

    /// <summary>
    /// Метод возвращает позицию следующей контрольной точки
    /// </summary>
    /// <param name="lastCheckPoint"> последняя пройденная контрольная точка</param>
    /// <returns></returns>
    private Transform GetNextPosition(Transform lastCheckPoint)
    {
        if (isActive)
        {
            int findInd = trackPoints.FindIndex(x => x == lastCheckPoint);
            if (findInd < trackPoints.Count - 1)
            {
                return trackPoints[findInd + 1];
            }
            else if (findInd == trackPoints.Count - 1 && isLooped)
            {
                return trackPoints[0];
            }
            else
            {
                return trackPoints[findInd];
            }
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Метод возвращает следующий чекпоинт к которому надо двигаться для указанного объекта
    /// и регистрирует в текущем чекпоинте как в пройденном
    /// в случае если чекпоинт является видимым (воротами), 
    /// </summary>
    /// <param name="pilot">Трансформ объекта который следует треку</param>
    /// <returns></returns>
    public Transform GetNextCheckPointAndCheckIn(Transform pilot, Transform trigerTransform)
    {
        Transform curPoint = TrackPositionData.GetCurrentPointToObject(pilot);

        if (isActive && curPoint)
        {
            TrySetPilotNextGatePointAndCheckInTable(pilot, trigerTransform);
            if (curPoint == trigerTransform)
            {
                Transform newPoint = GetNextPosition(curPoint);
                TrackPositionData.SetCurrentPointToObject(pilot, newPoint);
                
                

                return newPoint;
            }
            else
            {
                return curPoint;
            }
            
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// проверяет входит ли текущий чекпоинт в состав gatepoint
    /// если входит, то смещает следующий чекпоинт для пилота
    /// и делает запись в таблицу результатов
    /// </summary>
    /// <param name="pilot"></param>
    /// <param name="trackPoint"></param>
    private void TrySetPilotNextGatePointAndCheckInTable(Transform pilot, Transform trackPoint)
    {
        if (gatePoints.Contains(trackPoint))
        {
            int nextGatePointID = pilotsAndCurrentGatePoint[pilot]+1;
            
            if ((nextGatePointID >= gatePointsFollow.Count && isLooped)||
                (!isLooped&& nextGatePointID >= gatePointsFollow.Count-1) )
            {
                
                AddRowToLeaderTable(pilot, nextGatePointID, true);
                
            }else if (trackPoint == gatePointsFollow[nextGatePointID])
            {
                pilotsAndCurrentGatePoint[pilot] = nextGatePointID;
                AddRowToLeaderTable(pilot, nextGatePointID);
                MakeHighLightNextGate(pilot);
            }
              
        }
        
    }

    /// <summary>
    /// Регистрирует пилота в системи учет видимых чекпоинтов и определяет начало старта гонки
    /// </summary>
    /// <param name="pilot"></param>
    private void StartRegisterPilotInGate(Transform pilot)
    {
        if (gatePointsFollow.Count==0)
        {
            return;
        }

        AddRowToLeaderTable(pilot, 0);


        pilotsAndCurrentGatePoint.Add(pilot, 0);
        hasPilotFinished.Add(pilot, false);
        shipsAndDistance.Add(pilot, 0f);

        MakeHighLightNextGate(pilot);

        


    }

    /// <summary>
    /// Добавляет строку в словарь рейтинга лидеров прохождения гонки
    /// </summary>
    /// <param name="pilot"></param>
    /// <param name="gateNumber"></param>
    private void AddRowToLeaderTable(Transform pilot,  int gateNumber, bool isFinish = false)
    {
        if (hasPilotFinished.ContainsKey(pilot) && hasPilotFinished[pilot])
        {
            return;
        }

        if (isFinish)
        {
            hasPilotFinished[pilot] = true;
            if (pilot==player)
            {
                //Finish
                SetWin();
                OnFinish(this, EventArgs.Empty);
            }
            pilot.GetComponent<ShipModelView>().IsAlive = false;
        }
        
        TrackLeaderTableStruct trackLeaderTableStruct = new TrackLeaderTableStruct
        {
            pilot = pilot,
            gateNumber = gateNumber,
            dateTime = System.DateTime.Now,
            isFinish = isFinish
            
        };

        if (gateNumber< gatePointsFollow.Count)
        {
            trackLeaderTableStruct.gateTransform = gatePointsFollow[gateNumber];
        }
        
        trackLeaderBoard.Add(new KeyValuePair<int, TrackLeaderTableStruct>(gateNumber, trackLeaderTableStruct));

    }

    /// <summary>
    /// Определяет победителя гонки, запускать один раз
    /// победа, если на момет чека проехавших финиш только игрок
    /// </summary>
    /// <returns></returns>
    public void SetWin()
    {
        List<KeyValuePair<string, DateTime>> leaderList = new List<KeyValuePair<string, DateTime>>();
        foreach (KeyValuePair<int, TrackLeaderTableStruct> item in trackLeaderBoard)
        {
            if (item.Key == gatePointsFollow.Count)
            {
                leaderList.Add(new KeyValuePair<string, DateTime>(item.Value.pilot.name, item.Value.dateTime));
            }
        }

        if (leaderList.Count==0)
        {
            isWin = true;
        }
    }

    /// <summary>
    ///  Метод возвращает следующий чекпоинт относительно указанного
    ///  если трасса кольцевая и чекпоинт последний, возвращает первый чекпоинт
    ///  если чекпоинт последний и трасса не кольцевая возвращает последний чекпоинт
    /// </summary>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public Transform GetNextCheckPointPosition(Transform curCheckPoint)
    {
        int curIndex = trackPoints.IndexOf(curCheckPoint) + 1;

        if (curIndex < (trackPoints.Count))
        {
            return trackPoints[curIndex];
        }
        else if (isLooped)
        {
            return trackPoints[0];
        }
        else
        {
            return trackPoints[trackPoints.Count - 1];
        }
    }

    /// <summary>
    /// Добавляет объект на трек, для слежения за его прохождением
    /// </summary>
    /// <param name="pilotTransform">объект для слежения</param>
    public void SetObjPosition(Transform pilotTransform, ShipModelView shipModelView, bool setStartPoint = false, bool isPlayer = false)
    {
        if (isPlayer)
        {
            player = pilotTransform;
            playerShMV = shipModelView;
        }
        else
        {
            enemiesShipModelView.Add(shipModelView);
        }

        if (setStartPoint)
        {
            Transform startPosition = GetStartPosition();
            TrackPositionData.SetCurrentPointToObject(pilotTransform, startPosition);
            StartRegisterPilotInGate(pilotTransform);
        }
        else
        {
            TrackPositionData.SetCurrentPointToObject(pilotTransform, GetNearCheckPointPosition(pilotTransform));
        }


        
    }

    /// <summary>
    /// Возрващеает чекпоинта точки со старта
    /// </summary>
    /// <param name="curPoint"></param>
    /// <returns></returns>
    public float GetDistanceFromStart(Transform curPoint)
    {
        KeyValuePair<Transform, float> rv = checkPointAndDistance.FirstOrDefault(d => d.Key == curPoint);
        return rv.Value;
        
    }

    /// <summary>
    /// Возвращаяет длину трассы
    /// </summary>
    /// <returns></returns>
    public float GetTrackDistance()
    {
        return checkPointAndDistance[checkPointAndDistance.Count - 1].Value;
    }

    /// <summary>
    /// Возвращает пару - чекпоинт, который последним вошёл в указанную дистанцию с начала трассы
    /// и расстояние от начала трассы
    /// </summary>
    /// <param name="dist"></param>
    /// <returns></returns>
    public KeyValuePair<Transform, float> GetChekpointThrouDistance(float dist)
    {
        List<KeyValuePair<Transform, float>> ll = checkPointAndDistance.Where(d => d.Value <= dist).ToList();

        if (ll.Count > 0)
        {
            return ll[ll.Count - 1];
        }
        else
        {
            return default;
        }

    }

   /// <summary>
   /// Возвращает список "строк" кораблей прошедших финишную черту и время прохождения 
   /// </summary>
   /// <returns></returns>
    public List<string> GetFinishTrackLeaderBoard()
    {
        List<KeyValuePair<string, DateTime>> leaderList = new List<KeyValuePair<string, DateTime>>();
        foreach (KeyValuePair<int, TrackLeaderTableStruct> item in trackLeaderBoard)
        {
            if (item.Key == gatePointsFollow.Count)
            {
                leaderList.Add(new KeyValuePair<string, DateTime>(item.Value.pilot.name, item.Value.dateTime));
            }
        }
        //leaderList.OrderBy(o => o.Value);
        List<string> leaderListString = new List<string>();
        foreach (KeyValuePair<string, DateTime> item in leaderList)
        {
            leaderListString.Add(item.Key +": " + (item.Value- startTime).ToString(@"mm\:ss\:fff"));
        }
        


        return leaderListString;
    }

    /// <summary>
    /// Возвращает трансформ следующей по порядку точки с воротами,  если следующей точки нет, возвращает первую.
    /// </summary>
    /// <param name="pilot"></param>
    /// <returns></returns>
    public Transform GetNextGatePoint(Transform pilot)
    {
        if (gatePointsFollow.Count == 0)
        {
            return default;
        }
        
        int numberGate;
        if (pilotsAndCurrentGatePoint.ContainsKey(pilot))
        {
            numberGate = pilotsAndCurrentGatePoint[pilot];
        }
        else
        {
            numberGate = 0;
        }
        numberGate++;
        if (gatePointsFollow.Count> numberGate)
        {
            return gatePointsFollow[numberGate];
        }
        else
        {
            return gatePointsFollow[0];
        }
    }

    /// <summary>
    /// подсветка следующего чекпоинта для игрока
    /// </summary>
    /// <param name="pilot"></param>
    public void MakeHighLightNextGate(Transform pilot)
    {
        if (pilot == player)
        {
            if (checkPointHightLightInstce==null)
            {
                checkPointHightLightInstce = Instantiate(checkPointHightLight);
            }
            checkPointHightLightInstce.transform.position = GetNextGatePoint(pilot).position;

            playerShMV.CheckInSound();

            //StartCoroutine(TranslateGateHighLight(GetNextGatePoint(pilot).position));
        }
    }
    
    /// <summary>
    /// перемещает светлячок над воротами к следующей точке
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private IEnumerator TranslateGateHighLight(Vector3 position)
    {
        Vector3 startPosition = checkPointHightLightInstce.transform.position;
        float startTime = Time.time;
        // Calculate the journey length.
        float journeyLength = Vector3.Distance(checkPointHightLightInstce.transform.position, position);

        

        position.y +=heightOfHighLight;
        while (checkPointHightLightInstce.transform.position!= position)
        {

            float distCovered = (Time.time - startTime) * 100f;
            float fractionOfJourney = distCovered / journeyLength;

            checkPointHightLightInstce.transform.position = Vector3.Lerp(startPosition, position, fractionOfJourney);
            yield return null;
        }
    }

    /// <summary>
    /// корутина добавляет буст отстающим противникам
    /// </summary>
    /// <param name="timerToBoost"></param>
    /// <returns></returns>
    IEnumerator TrackEnemyHelperCorut(float timerToBoost = 5f)
    {
        while (true)
        {
            yield return new WaitForSeconds(timerToBoost);
            TrackEnemyHelper();
        }
        
    }

    /// <summary>
    /// добавляет буст для отстающих врагов
    /// </summary>
    private void TrackEnemyHelper()
    {
        int curPointPlayer = pilotsAndCurrentGatePoint[player];
        float playerNextGatePosDistance = Vector3.Distance(GetNextGatePoint(player).position, player.position);
        
        foreach (ShipModelView enemy in enemiesShipModelView)
        {
            float enemyNextGatePosDistance = Vector3.Distance(GetNextGatePoint(enemy.transform).position, enemy.transform.position);

            float playerEnemyDistance = Vector3.Distance(player.position, enemy.transform.position);                                                                                      //добавляем ускороение если:
            if ((pilotsAndCurrentGatePoint[enemy.transform] < curPointPlayer                        // последний зачекиненый поинт меньше чем у игрока
                && playerEnemyDistance > distToAutoBoost)                                           // и дистация между игроком и врагом больше чем указанная;
                || ( pilotsAndCurrentGatePoint[enemy.transform] == curPointPlayer                   // или одинаковый чекпоинт, но
                && enemyNextGatePosDistance > playerNextGatePosDistance                             // расстояние до сл. чекпоинта больше чем у игрока
                && playerEnemyDistance > distToAutoBoost ))                                         // и дистация между игроком и врагом больше чем указанная
            {
                
                enemy.enemyPilotController.aimInterestCoef = aimHelperInterest;
                enemy.autoBoostHelper = true;
                Debug.Log(enemy.transform.name + "Set enemy coef interest " + aimHelperInterest.ToString());
            }
            else
            {
                enemy.enemyPilotController.aimInterestCoef = 1f;
                enemy.autoBoostHelper = false;
                Debug.Log(enemy.transform.name + "Set enemy coef interest X1.0");
            }
        }
        
    }

    /// <summary>
    /// установка прохождения расстояния пилотом с начала гонки в словарь соответствий
    /// </summary>
    private void SetShipPositionTable()
    {
        foreach (KeyValuePair<Transform, int> item in pilotsAndCurrentGatePoint)
        {
            float pos = GetDistanceFromStart(gatePointsFollow[item.Value]); 
            pos += Vector3.Distance(gatePointsFollow[item.Value].position, item.Key.position);
            shipsAndDistance[item.Key] = pos;
            Debug.Log(item.Key.ToString() + ": " + pos.ToString());
        }
                      
    }
    
    /// <summary>
    /// Возвращает список гоночных кораблей в порядке лидирования на трассе.
    /// </summary>
    /// <returns></returns>
    public List<string> GetShipPositionTable(out int position)
    {
        List<string> leaderListString = new List<string>();
        List<KeyValuePair<Transform, float>> posList = new List<KeyValuePair<Transform, float>>();
        
        foreach (KeyValuePair<Transform, int> item in pilotsAndCurrentGatePoint)
        {
            float pos = GetDistanceFromStart(gatePointsFollow[item.Value]);
            pos += Vector3.Distance(gatePointsFollow[item.Value].position, item.Key.position);
            posList.Add(new KeyValuePair<Transform, float>(item.Key, pos));
        }

        posList.Sort((x, y) => (y.Value.CompareTo(x.Value)));

        position = 0;

        if (posList.Count <= 3)
        {
            for (int j = 0; j < posList.Count; j++)
            {
                
                leaderListString.Add((j+1).ToString() + ": " + posList[j].Key.name);
                position = j + 1;
            }
            
        }
        else
        {
            int i;
            for (i = 0; i < posList.Count; i++)
            {
                if (posList[i].Key == player)
                {
                    break;
                }
            }
            position = i + 1;

            if (i == 0)
            {
                leaderListString.Add("1: " + posList[i].Key.name);
                leaderListString.Add("2: " + posList[i + 1].Key.name);
                leaderListString.Add("3: " + posList[i + 2].Key.name);
            }
            else if (i == posList.Count-1)
            {
                leaderListString.Add((i-1).ToString() + ": " + posList[i-2].Key.name);
                leaderListString.Add((i).ToString() + ": " + posList[i-1].Key.name);
                leaderListString.Add((i+1).ToString() + ": " + posList[i].Key.name);
            }
            else
            {
                leaderListString.Add((i).ToString() + ": " + posList[i-1].Key.name);
                leaderListString.Add((i+1).ToString() + ": " + posList[i].Key.name);
                leaderListString.Add((i+2).ToString() + ": " + posList[i+1].Key.name);
            }

        }

        return leaderListString;
    }


}
