using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Cohesion")]
public class Cohesion : SheepBehaviour
{
    /**
     * Find the middle point between all the neighbours and move there
     */
    public override Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manger)
    {
        if (context.Count == 0)
            return Vector3.zero;
        
        Vector3 move = Vector3.zero;
        foreach (Transform item in context)
        {
            move += item.position;
        }
        move /= context.Count;
        
        // create offset from agent position
        move -= sheep.transform.position;
        return move;
    }
}
