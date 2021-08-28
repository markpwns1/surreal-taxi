using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickHandler : MonoBehaviour
{
    enum TrickMove
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    private List<TrickMove> tricks;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void GenerateTrickMoves()
    {
        for (int i = 0; i < 7; i++)
        {
            tricks.Add((TrickMove) Random.Range(0, 4));
        }
    }
}
