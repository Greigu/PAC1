using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;
[Condition("Bees/Is Runner Near?")] // information for the BB editor
[Help("Checks whether Runner is near the Hive.")]
public class IsThiefClose : ConditionBase
{
    [InParam("Police game object")] // input parameters
    [Help("Game object of the Police")]
    public GameObject police;
    [InParam("Thief game object")]
    [Help("Game object of the Thief")]
    public GameObject thief;
    [InParam("distance")]
    [Help("Distance between police and runner")]
    public float d;
    public override bool Check()
    {
        return Vector3.Distance(police.transform.position,
        thief.transform.position) < d;
    }
}