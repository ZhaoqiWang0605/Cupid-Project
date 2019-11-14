using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceArrowController : MonoBehaviour, ILaunchable
{
    public UIForceArrowButtonController uIForceArrowButtonController { get; set; }
    public GameObject mGameObject { get; set; }
    public GameObject aimingLinePrefab;

    private Vector2 originalPos;
    private Rigidbody2D rg;
    private bool launched = false;
    private bool collided = false;
    private int collisionCnt = 0;
    private AimingLine aimingLine;

    //configuration
    public int maxBounceNum;

    // Start is called before the first frame update
    void Awake()
    {
        mGameObject = gameObject;
        originalPos = transform.position;
        rg = GetComponent<Rigidbody2D>();
        aimingLine = Instantiate(aimingLinePrefab, transform).GetComponent<AimingLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (launched && !collided)
        {
            rotate();
        }

        if (Vector2.Distance(originalPos, transform.position) > 50.0f)
        {
            uIForceArrowButtonController.nextArrow();
            Destroy(gameObject);
        }
    }

    void rotate()
    {
        
        float rotationZ = Mathf.Atan2(rg.velocity.y, rg.velocity.x) * Mathf.Rad2Deg;
        rg.rotation = rotationZ;
    }

    public void launch(Vector2 force)
    {
        Debug.Log("launch");
        launched = true;
        rg.bodyType = RigidbodyType2D.Dynamic;
        rg.gravityScale = 0;
        rg.AddForce(force, ForceMode2D.Impulse);
        Debug.Log("gravity: " + rg.gravityScale);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Unbouncable" || collisionCnt >= maxBounceNum)
        {
            rg.bodyType = RigidbodyType2D.Static;
            uIForceArrowButtonController.nextArrow();
            Destroy(gameObject);
            Debug.Log("bounce arrow destory");
        }
        else
        {
            collisionCnt++;
            rotate();
            Debug.Log("bounce arrow bounce");
        }
    }


    public float getArrowMass()
    {
        return rg.mass;
    }

    public Vector3 getArrowPosition()
    {
        return rg.transform.position;
    }

    public void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        aimingLine.setTrajectoryPoints(pStartPosition, pVelocity);
    }

    public void RemoveProjectileArc()
    {
        aimingLine.RemoveProjectileArc();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
