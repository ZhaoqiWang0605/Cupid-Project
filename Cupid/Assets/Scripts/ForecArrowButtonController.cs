using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForecArrowButtonController : MonoBehaviour
{
    public Transform cupidAnchor;
    public float maxDis = 1.5f;
    public GameObject forceArrowPrefab;

    private ForceArrowController currentArrow;
    //private bool launched = false;

    // Start is called before the first frame update
    void Start()
    {  
    }

    private void Awake()
    {
        nextArrow();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, currentArrow.gameObject.transform.position) > 20.0f) {
            Destroy(currentArrow.gameObject);
            nextArrow();
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        Vector3 pos = (transform.position - cupidAnchor.position).normalized;
        // TODO: rotate arrow and cupid as we rotate force arrow button
        //forceArrow.rotateClockWise(Mathf.Atan2(-pos.y, -pos.x)*Mathf.Rad2Deg);

        if (Vector3.Distance(transform.position, cupidAnchor.position) > maxDis)
        {    
            pos *= maxDis;
            transform.position = pos + cupidAnchor.position;
        }

        
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        /*
        if (!launched) {
            Vector3 pos = (cupidAnchor.position - transform.position);
            currentArrow.launch(new Vector2(pos.x, pos.y) * 10);
            launched = true;
        }
        transform.position = cupidAnchor.position;
        */

        Vector3 pos = (cupidAnchor.position - transform.position);
        currentArrow.launch(new Vector2(pos.x, pos.y) * 10);
        transform.position = cupidAnchor.position;
    }

    public void nextArrow() {
        currentArrow = Instantiate(forceArrowPrefab, cupidAnchor.position, Quaternion.identity).GetComponent<ForceArrowController>();
        //launched = false;
        if (currentArrow != null)
        {
            currentArrow.GetComponent<ForceArrowController>().forceArrowButtonController = this;
        }
        else {
            Debug.Log("ForceArrowButton.nextArrow(): ForceArrow prefab instantiated, but couldn't get prefab's ForceArrowController.");
        }

    }
}
