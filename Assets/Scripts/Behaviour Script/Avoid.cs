using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Movement/Avoid")]
public class Avoid : SheepBehaviour
{
    public override Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manger)
    {
        if (context.Count == 0)
            return Vector3.zero;
        
        Vector3 move = Vector3.zero;
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            if (Vector3.SqrMagnitude(item.position - sheep.transform.position) < manger.squareAvoidanceMultiplier)
            {
                nAvoid++;
                move += (sheep.transform.position - item.position);

            }
        }

        if (nAvoid > 0)
        {
            move /= nAvoid;
        }
        return move;
    }
}
