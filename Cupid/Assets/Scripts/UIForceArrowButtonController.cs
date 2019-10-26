using System.Collections.Generic;
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

    public GameObject projectArcPoint;
    public int projectArcPointCount = 30;
    private List<GameObject> arcPoints;

	public Audio audio;

    // Members used for moving Cupid
    public Vector2 cupidTargetPos;
    public float smoothTime = 0.3F;
    private Vector2 velocity = Vector3.zero;
    private MoveableCameraController moveableCameraController;

    private void Awake()
    {
        centerPosition = transform.position;
        nextArrow();

        arcPoints = new List<GameObject>();
        //   points object are instatiated
        for (int i = 0; i < projectArcPointCount; i++)
        {
            GameObject point = (GameObject)Instantiate(projectArcPoint, cupidAnchor);
            point.GetComponent<SpriteRenderer>().sortingOrder = 2;
            point.GetComponent<Renderer>().enabled = false;
            arcPoints.Insert(i, point);
        }

        cupidTargetPos = cupidAnchor.position;
        moveableCameraController = GameObject.Find("MoveableCamera").GetComponent<MoveableCameraController>();
    }

    void Update()
    {
        if (Vector3.Distance(cupidAnchor.position, currentArrow.mGameObject.transform.position) > 20.0f)
        {
            Destroy(currentArrow.mGameObject);
            //currentArrow.Destroy();
            nextArrow();
        }
        cupidAnchor.position = Vector2.SmoothDamp(cupidAnchor.position, cupidTargetPos, ref velocity, smoothTime);
    }

    public void nextArrow()
    {
        Debug.Log("UIForceArrowButtonController.nextArrow()");
        /*
        currentArrow = Instantiate(arrowPrefabs[currentArrowType], cupidAnchor);
        currentArrow.GetComponent<ILaunchable>().uIForceArrowButtonController = this;
        */
        currentArrow = Instantiate(arrowPrefabs[currentArrowType], cupidAnchor).GetComponent<ILaunchable>();
        currentArrow.mGameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
        currentArrow.uIForceArrowButtonController = this;
    }

    public void moveCupidXto(float xPos)
    {
        cupidTargetPos = new Vector2(xPos, cupidTargetPos.y);
        moveableCameraController.setFollow(cupidAnchor);
    }

    public void changeArrow(int type)
    {
        currentArrowType = type;
        Destroy(currentArrow.mGameObject);
        //currentArrow.Destroy();
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
        //transform.eulerAngles = new Vector3(0, 0, angle);
        setTrajectoryPoints(currentArrow.getArrowPosition(), force / currentArrow.getArrowMass());
        

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
        RemoveProjectileArc();
        currentArrow.launch(force);
        transform.position = centerPosition;

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

    //---------------------------------------    
    // Following method displays projectile trajectory path. It takes two arguments, start position of object(ball) and initial velocity of object(ball).
    //---------------------------------------    
    private void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < projectArcPointCount; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
            arcPoints[i].transform.position = pos;
            arcPoints[i].GetComponent<Renderer>().enabled = true;
            arcPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
        }
    }

    private void RemoveProjectileArc()
    {
        for (int i = 0; i < projectArcPointCount; i++)
        {
            arcPoints[i].GetComponent<Renderer>().enabled = false;
        }
    }
}
