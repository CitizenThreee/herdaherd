using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Composite")]
public class Composite : SheepBehaviour
{
    public SheepBehaviour[] behaviours;
    public float[] weights;
    public override Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manger)
    {
        if (behaviours.Length != weights.Length)
        {
            Debug.LogError("Composite doesn't have the same amount of weights");
            return Vector3.zero;
        }
        
        Vector3 move = Vector3.zero;
        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector3 partialMove = behaviours[i].calculateMove(sheep, context, manger) * weights[i];
            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i]*weights[i]) // @TODO 
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }
}
