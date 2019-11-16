using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotThroughArrowController : MonoBehaviour, ILaunchable
{
    public UIForceArrowButtonController uIForceArrowButtonController { get; set; }
    public GameObject mGameObject { get; set; }
    public GameObject dummyColliderPrefab;
    public GameObject aimingLinePrefab;

    private Vector2 originalPos;
    private Rigidbody2D rg;
    private bool launched = false;
    private HashSet<int> collidedID;
    private AimingLine aimingLine;

    void Awake()
    {
        mGameObject = gameObject;
        originalPos = transform.position;
        rg = GetComponent<Rigidbody2D>();
        collidedID = new HashSet<int>();
        aimingLine = Instantiate(aimingLinePrefab, transform).GetComponent<AimingLine>();
    }

    void Update()
    {
        if (launched)
        {
            Rotate();
        }

        if (Vector2.Distance(originalPos, transform.position) > 50.0f)
        {
            uIForceArrowButtonController.NextArrow();
            Destroy(gameObject);
        }
    }

    void Rotate()
    {
        float rotationZ = Mathf.Atan2(rg.velocity.y, rg.velocity.x) * Mathf.Rad2Deg;
        rg.rotation = rotationZ;
    }

    public void Launch(Vector2 force)
    {
        if (!launched)
        {
            launched = true;
            rg.bodyType = RigidbodyType2D.Dynamic;
            rg.AddForce(force, ForceMode2D.Impulse);
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("ShotThroughArrow OnTriggerEnter");
        if (!collidedID.Contains(collision.gameObject.GetInstanceID()))
        {
            collidedID.Add(collision.gameObject.GetInstanceID());
            GameObject dummy = Instantiate(dummyColliderPrefab, collision.gameObject.transform.position, Quaternion.identity);
            dummy.GetComponent<Rigidbody2D>().velocity = rg.velocity;
        }
    }

    public void SetTrajectoryPoints(Vector3 force)
    {
        aimingLine.setTrajectoryPoints(transform.position, force / rg.mass);
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

