using System;
using UnityEngine;

public class PlayerPilotController
{
    private readonly PlayerPilotModelView playerModelView;
    private readonly ShipModelView shipModelView;
    private readonly TrackPath checkpointsPath;
    private readonly DirectionArrowModelView directionArrow;



    public PlayerPilotController(PlayerPilotModelView player, ShipModelView ship, TrackPath checkpoints, DirectionArrowModelView dirHUDArrow)
    {
        playerModelView = player;
        shipModelView = ship;
        checkpointsPath = checkpoints;
        directionArrow = dirHUDArrow;

        checkpointsPath.SetObjPosition(shipModelView.transform, ship, true, true);
        directionArrow.CheckpointDirection = checkpointsPath.GetNextGatePoint(shipModelView.transform);
        directionArrow.ShipDirection = shipModelView.transform;

        playerModelView.OnMovingInput += HandleMovingInput;
        InputControl.Instance.OnActionInput += HandleActionInput;

        playerModelView.OnTriggerCollision += HandleTriggerCollision;
    }

    private void HandleActionInput(object sender, Vector3 direction)
    {
        shipModelView.ActionInput(direction);
    }

    private void HandleMovingInput(object sender, Vector3 direction)
    {
        shipModelView.SteeringInput(direction);

        directionArrow.ShipDirection = shipModelView.transform;
    }

    private void HandleTriggerCollision(object sender, Transform checkpointTransform)
    {
        if (checkpointTransform.tag.Equals("TrackPoint"))
        {
            checkpointsPath.GetNextCheckPointAndCheckIn(shipModelView.transform, checkpointTransform);
            directionArrow.CheckpointDirection = checkpointsPath.GetNextGatePoint(shipModelView.transform);
        }
    }
}
