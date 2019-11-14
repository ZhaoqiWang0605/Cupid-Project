﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIForceArrowButtonController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Transform cupidAnchor;
    public RectTransform arrowButtonPointer;
    public float maxDis;
    public List<GameObject> arrowPrefabs;

    private int currentArrowType = 0;
    private ILaunchable currentArrow;

    private Vector3 centerPosition;
    private float power = 0.25f;

	public Audio audio;

    // Members used for moving Cupid
    public Vector2 cupidTargetPos;
    public float smoothTime = 0.3F;
    private Vector2 velocity = Vector3.zero;
    private MoveableCameraController moveableCameraController;

    private void Start()
    {
        moveableCameraController = GameObject.Find("MoveableCamera").GetComponent<MoveableCameraController>();

        centerPosition = transform.position;
        nextArrow();

        cupidTargetPos = cupidAnchor.position;
    }

    void OnEnable()
    {
        cupidTargetPos = cupidAnchor.position;
    }

    void Update()
    {
        if (Vector3.Distance(cupidAnchor.position, currentArrow.mGameObject.transform.position) > 20.0f)
        {
            Destroy(currentArrow.mGameObject);
            nextArrow();
        }
        cupidAnchor.position = Vector2.SmoothDamp(cupidAnchor.position, cupidTargetPos, ref velocity, smoothTime);
    }

    public void nextArrow()
    {
        Debug.Log("UIForceArrowButtonController.nextArrow()");
        currentArrow = Instantiate(arrowPrefabs[currentArrowType], cupidAnchor).GetComponent<ILaunchable>();
        currentArrow.mGameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
        currentArrow.uIForceArrowButtonController = this;
    }

    public void moveCupidTo(Transform pos)
    {
        cupidTargetPos = pos.position;
    }

    public void changeArrow(int type)
    {
        currentArrowType = type;
        Destroy(currentArrow.mGameObject);
        nextArrow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag() eventData.position: " + eventData.position.ToString());
        
        transform.position = eventData.position;
        Vector3 posDelta = (transform.position - centerPosition).normalized;
        //Debug.Log("posDelta normalized: " + posDelta.ToString());
        if (Vector3.Distance(transform.position, centerPosition) > maxDis)
        {
            posDelta *= maxDis;
            //Debug.Log("maxDis: " + maxDis);
            //Debug.Log("posDelta multiplied: " + posDelta.ToString());
            transform.position = centerPosition + posDelta;
        }
        //draw projectile arc
        Vector3 force = GetForceFromTwoPoint(transform.position, centerPosition);
        float angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        currentArrow.setTrajectoryPoints(currentArrow.getArrowPosition(), force / currentArrow.getArrowMass());


        // Rotate cupid and arrowButtonPointer as we rotate force arrow button
        float rotationAngle = Mathf.Atan2(-posDelta.y, -posDelta.x) * Mathf.Rad2Deg;
        cupidAnchor.rotation = Quaternion.Euler(0, 0, rotationAngle);
        currentArrow.mGameObject.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        arrowButtonPointer.transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag()");
        Vector2 force = GetForceFromTwoPoint(transform.position, centerPosition);
        currentArrow.RemoveProjectileArc();
        currentArrow.launch(force);
        transform.position = centerPosition;
        moveableCameraController.setFollow(currentArrow.mGameObject.transform);
        moveableCameraController.followArrow();

        // Rotate cupid back to default position after arrow is shot
        cupidAnchor.rotation = Quaternion.Euler(0, 0, 0);
        currentArrow.mGameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        arrowButtonPointer.transform.rotation = Quaternion.Euler(0, 0, -90.0f);

		audio.PlayMusic();
	}

    // Following method returns force by calculating distance between given two points
    //---------------------------------------    
    private Vector2 GetForceFromTwoPoint(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
    }
    
}
