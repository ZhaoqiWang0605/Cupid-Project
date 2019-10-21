using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotThroughArrowController : MonoBehaviour, ILaunchable
{
    public UIForceArrowButtonController uIForceArrowButtonController { get; set; }
    public GameObject mGameObject { get; set; }

    private Vector2 originalPos;
    private Rigidbody2D rg;
    private bool launched = false;
    private bool collided = false;

    // Start is called before the first frame update
    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        mGameObject = gameObject;
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (launched)
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
        rg.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        TerrainController terrain = col.gameObject.GetComponent<TerrainController>();
        if (terrain != null)
        {
            collided = true;
            collideWithTerrain();
        }

        CoupleController couple = col.gameObject.GetComponent<CoupleController>();
        if (couple != null)
        {
            collided = true;
            collideWithCouple(couple);
        }
    }

    public void collideWithTerrain()
    {
        Debug.Log("collideWithTerrain");
    }

    public void collideWithCouple(CoupleController couple)
    {
        Debug.Log("collideWithCouple");
        couple.setInLove();
        uIForceArrowButtonController.moveCupidXto(transform.position.x);
    }

    public float getArrowMass()
    {
        return rg.mass;
    }

    public Vector3 getArrowPosition()
    {
        return rg.transform.position;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

