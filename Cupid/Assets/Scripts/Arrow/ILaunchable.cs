using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaunchable
{
    UIForceArrowButtonController uIForceArrowButtonController {get; set; }
    GameObject mGameObject { get; }
    void Launch(Vector2 force);
    void SetTrajectoryPoints(Vector3 force);
    void RemoveProjectileArc();
    void Destroy();
}
