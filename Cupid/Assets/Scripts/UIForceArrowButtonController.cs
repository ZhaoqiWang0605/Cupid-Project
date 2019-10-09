using UnityEngine;
using UnityEngine.EventSystems;

public class UIForceArrowButtonController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Transform cupidAnchor;
    public RectTransform arrowButtonPointer;
    public float maxDis;
    public GameObject forceArrowPrefab;

    private ForceArrowController currentArrow;
    private Vector3 centerPosition;

    private void Awake()
    {
        /*Debug.Log("transform.Position: " + transform.position.ToString());
        Debug.Log("RectTransform.localPosition: " + GetComponent<RectTransform>().localPosition.ToString());
        Debug.Log("RectTransform.anchoredPosition: " + GetComponent<RectTransform>().anchoredPosition.ToString());*/
        centerPosition = transform.position;
        nextArrow();
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

        // Rotate cupid, arrow and arrowButtonPointer as we rotate force arrow button
        float rotationAngle = Mathf.Atan2(-posDelta.y, -posDelta.x) * Mathf.Rad2Deg;
        cupidAnchor.rotation = Quaternion.Euler(0, 0, rotationAngle);
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        arrowButtonPointer.transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag()");
        Vector2 posDelta = new Vector2(centerPosition.x - transform.position.x, centerPosition.y - transform.position.y);
        currentArrow.launch(new Vector2(posDelta.x, posDelta.y)/4);
        transform.position = centerPosition;

        // Rotate cupid back to default position after arrow is shot
        cupidAnchor.rotation = Quaternion.Euler(0, 0, 0);
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
