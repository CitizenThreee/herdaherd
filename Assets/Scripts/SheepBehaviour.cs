using System.Collections.Generic;
using UnityEngine;

public abstract class SheepBehaviour : ScriptableObject
{
    public abstract Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manger, DogController dog);
}
