using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaunchable
{
    UIForceArrowButtonController uIForceArrowButtonController {get; set; }
    GameObject mGameObject { get; }
    void launch(Vector2 force);
    float getArrowMass();
    Vector3 getArrowPosition();
    void Destroy();
}
