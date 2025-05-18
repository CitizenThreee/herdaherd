using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Alignment")]
public class Alignment : SheepBehaviour
{
    public override Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manger, DogController dog)
    {
        if (context.Count == 0)
            return sheep.transform.forward;
        
        Vector3 move = Vector3.zero;
        foreach (Transform item in context)
        {
            move += item.transform.forward;
        }
        move /= context.Count;
        return move.normalized * manger.maxSpeed;
    }
}
