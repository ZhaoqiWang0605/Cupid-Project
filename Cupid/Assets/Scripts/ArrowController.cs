using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform cupidAnchor;
    public float maxDis = 2;

    private bool isClicked = false;
    private SpringJoint2D sp;
    private Rigidbody2D rg;


    // Start is called before the first frame update
    void Awake()
    {
        sp = GetComponent<SpringJoint2D>();
        rg = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        isClicked = true;
        rg.isKinematic = true;
    }

    private void OnMouseUp()
    {
        isClicked = false;
        rg.isKinematic = false;
        Invoke("Fly", 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cupidAnchor.position.x);
        if (isClicked) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            if (Vector3.Distance(transform.position, cupidAnchor.position) > maxDis) {
                Vector3 pos = (transform.position - cupidAnchor.position).normalized;
                pos *= maxDis;
                transform.position = pos + cupidAnchor.position;
            }
        }
    }

    void Fly()
    {
        sp.enabled = false;
    }

    

}
