using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIForceArrowButtonController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool moveCameraAfterLaunch = false;
    public Transform cupidAnchor;
    public RectTransform arrowButtonPointer;
    public float maxDis;
    public List<GameObject> arrowPrefabs;
    public List<double> arrowNums;
    public List<Text> arrowNumText;
    public List<Toggle> arrowSwitchToggles;

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
        for (int i=0; i<arrowPrefabs.Count; i++)
        {
            if (i > arrowNums.Count - 1)
            {
                arrowNums.Add(double.PositiveInfinity);
            }
            else if (arrowNums[i] < 0)
            {
                arrowNums[i] = double.PositiveInfinity;
            }
        }

        moveableCameraController = GameObject.Find("MoveableCamera").GetComponent<MoveableCameraController>();

        centerPosition = transform.position;
        NextArrow();
    }

    void OnEnable()
    {
        cupidTargetPos = cupidAnchor.position;
    }

    void Update()
    {
        cupidAnchor.position = Vector2.SmoothDamp(cupidAnchor.position, cupidTargetPos, ref velocity, smoothTime);
        for (int i=0;i<arrowNumText.Count;i++)
        {
            arrowNumText[i].text = arrowNums[i].ToString();
            if (arrowNums[i] == double.PositiveInfinity)
            {
                arrowNumText[i].text = "∞";
            }
            else if (arrowNums[i] <= 0)
            {
                arrowNumText[i].text = "0";
            }
        }
    }

    public void NextArrow()
    {
        int switchCount = 0;
        while (arrowNums[currentArrowType] <= 0)
        {
            currentArrowType = (currentArrowType + 1) % arrowPrefabs.Count;
            switchCount++;
            if (arrowNums[currentArrowType] > 0)
            {
                arrowSwitchToggles[currentArrowType].isOn = true;
                return;
            }
            if (switchCount > arrowPrefabs.Count - 1)
            {
                return;
            }
        }

        Debug.Log("UIForceArrowButtonController.nextArrow()");
        currentArrow = Instantiate(arrowPrefabs[currentArrowType], cupidAnchor).GetComponent<ILaunchable>();
        currentArrow.mGameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
        currentArrow.uIForceArrowButtonController = this;
    }

    public void MoveCupidTo(Transform pos)
    {
        cupidTargetPos = pos.position;
    }

    public void ChangeArrow(int type)
    {
        currentArrowType = type;
        if (currentArrow != null)
        {
            Destroy(currentArrow.mGameObject);
        }
        NextArrow();
    }

    public bool IsOutOfArrow()
    {
        foreach (double num in arrowNums)
        {
            if (num > 0)
            {
                Debug.Log("Has Arrow");
                return false;
            }
        }
        Debug.Log("Out Of Arrow");
        return true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(currentArrow == null)
        {
            return;
        }
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
        currentArrow.SetTrajectoryPoints(force);


        // Rotate cupid and arrowButtonPointer as we rotate force arrow button
        float rotationAngle = Mathf.Atan2(-posDelta.y, -posDelta.x) * Mathf.Rad2Deg;
        cupidAnchor.rotation = Quaternion.Euler(0, 0, rotationAngle);
        currentArrow.mGameObject.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        arrowButtonPointer.transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentArrow == null)
        {
            return;
        }
        //Debug.Log("OnEndDrag()");
        Vector2 force = GetForceFromTwoPoint(transform.position, centerPosition);
        currentArrow.RemoveProjectileArc();
        currentArrow.Launch(force);
        transform.position = centerPosition;
        if (moveCameraAfterLaunch)
        {
            moveableCameraController.SetFollowTarget(currentArrow.mGameObject.transform);
            moveableCameraController.SwitchToFollow();
        }
        arrowNums[currentArrowType] = arrowNums[currentArrowType] - 1;

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
