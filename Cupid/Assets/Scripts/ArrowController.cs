﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour, ILaunchable
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
        originalPos = transform.position;
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
        rg.AddForce(force, ForceMode2D.Impulse);
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

