using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyPilotController
{
    private readonly EnemyPilotModelView pilotModelView;
    private readonly ShipModelView shipModelView;
    private readonly TrackPath checkpointsPath;

    private Transform currentAim;

    // Точка финальной цели для пилота корабля с уже добавленным разбросом
    private Vector3 aimOffset;

    // "Шум" скорости передвижения
    public float aimInterestCoef = 1.0f;
    private float aimInterest = 1.0f;
    
     private int runOutDirection = 2; // TODO переделать этот костыль

    public EnemyPilotController(EnemyPilotModelView enemyPilot, ShipModelView ship, TrackPath checkpoints)
    {
        pilotModelView = enemyPilot;
        shipModelView = ship;
        checkpointsPath = checkpoints;

        
        
        checkpointsPath.SetObjPosition(ship.transform, shipModelView, true);
        currentAim = checkpointsPath.GetStartPosition();

        pilotModelView.ChechpointTarget = currentAim.position;
        pilotModelView.OnMovingInput += HandleMovingInput;
        pilotModelView.OnActionInput += HandleActionInput;
        pilotModelView.OnTriggerCollision += HandleTriggerCollision;

        shipModelView.OnSecondaryAbilityChanged += HandleShipSecondaryAbilityChanged;
        SetAimAndOffset();
    }

    private void HandleShipSecondaryAbilityChanged(object sender, Sprite e)
    {
        pilotModelView.InvokeSecondaryAbilityAfterDelay(1f,6f);
    }

    private void HandleTriggerCollision(object sender, Transform checkpointTransform)
    {
        if (checkpointTransform.tag.Equals("TrackPoint"))
        {
            currentAim = checkpointsPath.GetNextCheckPointAndCheckIn(shipModelView.transform, checkpointTransform); //?????

            SetAimAndOffset();
        }
    }

    private void SetAimAndOffset()
    {
        // Получаем компонент коллайдера, описывающего расстояние между столбами ворот
        BoxCollider collider = currentAim.GetComponentInChildren<BoxCollider>();

        // Получаем случайную точку исходя из размеров коллайдера
        if (shipModelView.autoBoostHelper)
        {
            aimOffset = collider.transform.position;
        }
        else
        {
            aimOffset = new Vector3(
              x: UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x),
              y: collider.bounds.max.y,
              z: UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z));

        }
       

        // и размещаем эту точку внутри коллайдера (т.е. возвращаем ближайшую соответствующую полученным координатам точку в пространстве коллайдера)
        aimOffset = collider.ClosestPoint(aimOffset);

        pilotModelView.ChechpointTarget = aimOffset;

        // Получаем множитель скорости движения к следующей цели
        aimInterest = UnityEngine.Random.Range(0.9f, 1.18f) * aimInterestCoef;
    }


    private void HandleActionInput(object sender, Vector3 direction)
    { 
        shipModelView.ActionInput(direction);
    }
    
    private void HandleMovingInput(object sender, EventArgs e)
    {
        // AI obstacles avoiding (nikita)
        
        float maxDistance = 100f;
        RaycastHit hit;
        bool isLeftHit = false;
        bool isRightHit = false;
        bool isObstacleOnMyWay = false;
        bool isEnemyOnForward = false;
        bool isEnemyOnLeft = false;
        bool isEnemyOnRight = false;
        
        //вводим слои для обнаружения земли и для обнаружения подбираемых\уклоняемых сущностей на трассе
        LayerMask TrackEntityMask = LayerMask.GetMask("AICastedEntity");
        LayerMask groundMask = LayerMask.GetMask("Ground");
        LayerMask enemyMask = LayerMask.GetMask("Ship");
        
        if (pilotModelView.ChechpointTarget != null)
        {
            // нормализуем вектор направления до чекпоинта, отсекаем ему Y составляющую
            Vector3 checkpointDirection = new Vector3((pilotModelView.ChechpointTarget - pilotModelView.transform.position).normalized.x, 0,
                (pilotModelView.ChechpointTarget - pilotModelView.transform.position).normalized.z) * 2f;
            Vector3 rightDirection = new Vector3(pilotModelView.transform.right.x, 0, pilotModelView.transform.right.z) * 10;
            Vector3 leftDirection = new Vector3(-pilotModelView.transform.right.x, 0, -pilotModelView.transform.right.z) * 10;

            //forward cast
            bool isLandCast = Physics.Raycast(pilotModelView.transform.position + (checkpointDirection + Vector3.up).normalized * 20,
                Vector3.down, out hit, maxDistance, groundMask);
            isLandCast = true;
            if (isLandCast)
            {
                // находим расстояние до рейкаста до земли
                float pilotToCastPointDistance = Vector3.Distance(pilotModelView.transform.position, hit.point)*2;
                
                    // смотрим есть ли противник спереди или по бокам
                isEnemyOnForward = Physics.BoxCast(pilotModelView.transform.position, pilotModelView.transform.lossyScale, hit.point,
                    Quaternion.identity, pilotToCastPointDistance, enemyMask);
                //isEnemyOnLeft = Physics.BoxCast(pilotModelView.transform.position, pilotModelView.transform.lossyScale, leftDirection,
                //        Quaternion.identity, pilotToCastPointDistance, enemyMask);
                //isEnemyOnRight = Physics.BoxCast(pilotModelView.transform.position, pilotModelView.transform.lossyScale, rightDirection,
                //        Quaternion.identity, pilotToCastPointDistance, enemyMask);
                
                    // смотрим есть ли препятствие на пути
                isObstacleOnMyWay = Physics.BoxCast(pilotModelView.transform.position, pilotModelView.transform.lossyScale / 2, hit.point, out hit,
                    Quaternion.identity, pilotToCastPointDistance, TrackEntityMask);
                if (isObstacleOnMyWay)
                {
                    if (hit.collider.tag.Equals("SlowPoint"))
                    {
                        isRightHit = Physics.BoxCast(pilotModelView.transform.position, pilotModelView.transform.lossyScale / 2, (hit.point + rightDirection).normalized ,
                            Quaternion.identity, pilotToCastPointDistance, TrackEntityMask);
                        
                        isLeftHit = Physics.BoxCast(pilotModelView.transform.position, pilotModelView.transform.lossyScale / 2, (hit.point + leftDirection).normalized ,
                            Quaternion.identity, pilotToCastPointDistance, TrackEntityMask);
                        
                    }
                }
            }

            if(isEnemyOnForward && shipModelView.PrimaryAbility != null)
                HandleActionInput(this, Vector3.forward);
            //if(isEnemyOnLeft && shipModelView.PrimaryAbility != null)
            //    HandleActionInput(this, Vector3.left);
            //if(isEnemyOnRight && shipModelView.PrimaryAbility != null)
            //    HandleActionInput(this, Vector3.right);
            
            // создание вектора движения и поворота
            float moveH;
            if (isObstacleOnMyWay && shipModelView.autoBoostHelper == false)
            {
                if(isRightHit == false)
                {
                    moveH = Vector3.SignedAngle(shipModelView.transform.forward,
                        shipModelView.transform.forward + rightDirection, Vector3.up);
                    
                }

                 else if (isLeftHit == false)
                {
                    moveH = Vector3.SignedAngle(shipModelView.transform.forward,
                        shipModelView.transform.forward + leftDirection, Vector3.up);
                    
                }
                else
                {
                    if (runOutDirection == 2)
                        runOutDirection = UnityEngine.Random.Range(0, 2);
                    
                    if(runOutDirection == 0)
                        moveH = Vector3.SignedAngle(shipModelView.transform.forward,
                            shipModelView.transform.forward + leftDirection * 4, Vector3.up);
                    else
                    {
                        moveH = Vector3.SignedAngle(shipModelView.transform.forward,
                            shipModelView.transform.forward + rightDirection * 4, Vector3.up);
                    }
                        
                }
            }
            else 
                moveH = Vector3.SignedAngle(shipModelView.transform.forward,
                aimOffset - shipModelView.transform.position, Vector3.up);

            Vector3 direction = new Vector3(moveH / 30, 0, aimInterest);
            shipModelView.SteeringInput(direction);

            Debug.DrawLine(shipModelView.transform.position, aimOffset);
        }
    }
}