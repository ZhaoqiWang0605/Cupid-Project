using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingLine : MonoBehaviour
{
    public GameObject projectArcPoint;
    public int projectArcPointCount = 30;
    public List<GameObject> arcPoints;

    // Start is called before the first frame update
    void Start()
    {
        arcPoints = new List<GameObject>();
        //   points object are instatiated
        for (int i = 0; i < projectArcPointCount; i++)
        {
            GameObject point = (GameObject)Instantiate(projectArcPoint, transform);
            point.GetComponent<SpriteRenderer>().sortingOrder = 80;
            arcPoints.Insert(i, point);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //---------------------------------------    
    // Following method displays projectile trajectory path. It takes two arguments, start position of object(ball) and initial velocity of object(ball).
    // This base class method would return a straight line, please override this method if other kinds of aiming line is needed
    //---------------------------------------    
    public virtual void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < arcPoints.Count; i++)
        {
            Vector3 pos = pStartPosition + pVelocity * fTime;
            this.arcPoints[i].transform.position = pos;
            this.arcPoints[i].GetComponent<Renderer>().enabled = true;
            this.arcPoints[i].transform.eulerAngles = new Vector3(0, 0, angle);
            fTime += 0.1f;
        }
    }

    public void RemoveProjectileArc()
    {
        for (int i = 0; i < arcPoints.Count; i++)
        {
            arcPoints[i].GetComponent<Renderer>().enabled = false;
        }
    }
}
