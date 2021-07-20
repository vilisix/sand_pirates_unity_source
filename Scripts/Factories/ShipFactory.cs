using System;
using UnityEngine;

// Ship factory, creates different ships and their controllers
public class ShipFactory
{
    public static ShipModelView CreateShipModelView(Transform transform)
    {
            GameObject testShipPrefab = Resources.Load<GameObject>("Prefabs/Ship/BoozeBoat");
            ShipModelView modelView = UnityEngine.Object.Instantiate(testShipPrefab, transform.position, transform.rotation)
                .GetComponent<ShipModelView>();
            return modelView;
    }

    public static ShipController CreateShipController(ShipModelView shipMV, HitpointsCanvasModelView shipHP)
    {
        return new ShipController(shipMV, shipHP);
    }
}
