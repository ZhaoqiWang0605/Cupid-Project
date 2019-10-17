using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceArrowController : MonoBehaviour
{
    public UIForceArrowButtonController uIForceArrowButtonController;

    private Rigidbody2D rg;
    private bool launched = false;
    private bool collided = false;

    // Start is called before the first frame update
    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (launched && !collided) {
            rotate();
        }
    }

    void rotate() {
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
        if (!collided)
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

    }

    public void collideWithTerrain()
    {
        Debug.Log("collideWithTerrain");
        rg.bodyType = RigidbodyType2D.Static;
        uIForceArrowButtonController.nextArrow();
        //Destroy(gameObject, 0.5f);
        Destroy(gameObject);
    }

    public void collideWithCouple(CoupleController couple)
    {
        Debug.Log("collideWithCouple");
        rg.bodyType = RigidbodyType2D.Static;
        couple.setInLove();
        uIForceArrowButtonController.nextArrow();
        //Destroy(gameObject, 0.5f);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Debug.Log("ForceArrowController.OnMouseDown()");
    }

    public float getArrowMass()
    {
        return rg.mass;
    }

    public Vector3 getArrowPosition()
    {
        return rg.transform.position;
    }
}
