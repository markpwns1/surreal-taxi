using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public ProceduralGenerator map;

    public enum Phase
    {
        Idle,
        Delivering
    }

    public Phase currentPhase = Phase.Idle;
    public GameObject customerPrefab;
    public GameObject destinationPrefab;

    void Start()
    {
        PlaceCustomer();
    }

    void PlaceCustomer()
    {
        var placement = map.ForcePlaceObject(customerPrefab, true, true);
        placement.instance.transform.position = placement.hit.point;
    }

    void PlaceDestination()
    {
        var placement = map.ForcePlaceObject(destinationPrefab, true, true);
        placement.instance.transform.position = placement.hit.point;
    }

    public void CustomerPickedUp()
    {
        if (currentPhase != Phase.Idle) return;

        currentPhase = Phase.Delivering;
        PlaceDestination();
    }

    public void CustomerDroppedOff()
    {
        if (currentPhase != Phase.Delivering) return;

        currentPhase = Phase.Idle;
        PlaceCustomer();
    }
}
