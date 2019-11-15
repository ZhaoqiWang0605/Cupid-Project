using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaunchable
{
    UIForceArrowButtonController uIForceArrowButtonController {get; set; }
    GameObject mGameObject { get; }
    void launch(Vector2 force);
    void setTrajectoryPoints(Vector3 force);
    void RemoveProjectileArc();
    void Destroy();
}
