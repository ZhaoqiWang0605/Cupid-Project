using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIForceArrowButtonController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Transform cupidAnchor;
    public RectTransform arrowButtonPointer;
    public float maxDis;
    public GameObject forceArrowPrefab;

    private ForceArrowController currentArrow;
    private Vector3 centerPosition;
    private float power = 0.25f;

    public GameObject projectArcPoint;
    public int projectArcPointCount = 30;
    private List<GameObject> arcPoints;

	public Audio audio;

	private void start()
    {
        arcPoints = new List<GameObject>();
        //   points object are instatiated
        for (int i = 0; i < projectArcPointCount; i++)
        {
            GameObject point = (GameObject)Instantiate(projectArcPoint);
            point.GetComponent<Renderer>().enabled = false;
            arcPoints.Insert(i, point);
        }
    }

    private void Awake()
    {
        /*Debug.Log("transform.Position: " + transform.position.ToString());
        Debug.Log("RectTransform.localPosition: " + GetComponent<RectTransform>().localPosition.ToString());
        Debug.Log("RectTransform.anchoredPosition: " + GetComponent<RectTransform>().anchoredPosition.ToString());*/
        centerPosition = transform.position;
        nextArrow();

        arcPoints = new List<GameObject>();
        //   points object are instatiated
        for (int i = 0; i < projectArcPointCount; i++)
        {
            GameObject point = (GameObject)Instantiate(projectArcPoint);
            point.GetComponent<Renderer>().enabled = false;
            arcPoints.Insert(i, point);
        }
    }

    void Update()
    {
        if (Vector3.Distance(cupidAnchor.position, currentArrow.gameObject.transform.position) > 20.0f)
        {
            Destroy(currentArrow.gameObject);
            nextArrow();
        }
    }

    public void nextArrow()
    {
        Debug.Log("UIForceArrowButtonController.nextArrow()");
        Vector3 newArrowPosition = new Vector3(cupidAnchor.position.x + 0.1f, cupidAnchor.position.y, cupidAnchor.position.z);
        currentArrow = Instantiate(forceArrowPrefab, newArrowPosition, Quaternion.identity).GetComponent<ForceArrowController>();
        if (currentArrow != null)
        {
            currentArrow.GetComponent<ForceArrowController>().uIForceArrowButtonController = this;
        }
        else
        {
            Debug.Log("ForceArrowButton.nextArrow(): ForceArrow prefab instantiated, but couldn't get prefab's ForceArrowController.");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag() eventData.position: " + eventData.position.ToString());
        
        transform.position = eventData.position;
        Vector3 posDelta = (transform.position - centerPosition).normalized;
        Debug.Log("posDelta normalized: " + posDelta.ToString());
        if (Vector3.Distance(transform.position, centerPosition) > maxDis)
        {
            posDelta *= maxDis;
            Debug.Log("maxDis: " + maxDis);
            Debug.Log("posDelta multiplied: " + posDelta.ToString());
            transform.position = centerPosition + posDelta;
        }
        //draw projectile arc
        Vector3 force = GetForceFromTwoPoint(transform.position, centerPosition);
        float angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        setTrajectoryPoints(currentArrow.getArrowPosition(), force / currentArrow.getArrowMass());

        // Rotate cupid, arrow and arrowButtonPointer as we rotate force arrow button
        float rotationAngle = Mathf.Atan2(-posDelta.y, -posDelta.x) * Mathf.Rad2Deg;
        cupidAnchor.rotation = Quaternion.Euler(0, 0, rotationAngle);
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        arrowButtonPointer.transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag()");
        Vector2 force = GetForceFromTwoPoint(transform.position, centerPosition);
        RemooveProjectileArc();
        currentArrow.launch(force);
        transform.position = centerPosition;

        // Rotate cupid back to default position after arrow is shot
        cupidAnchor.rotation = Quaternion.Euler(0, 0, 0);
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
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

    private void RemooveProjectileArc()
    {
        for (int i = 0; i < projectArcPointCount; i++)
        {
            arcPoints[i].GetComponent<Renderer>().enabled = false;
        }
    }
}
