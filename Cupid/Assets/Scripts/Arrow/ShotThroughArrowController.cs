﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotThroughArrowController : MonoBehaviour, ILaunchable
{
    public UIForceArrowButtonController uIForceArrowButtonController { get; set; }
    public GameObject mGameObject { get; set; }
    public GameObject dummyColliderPrefab;

    private Vector2 originalPos;
    private Rigidbody2D rg;
    private bool launched = false;
    private HashSet<int> collidedID;

    void Awake()
    {
        mGameObject = gameObject;
        originalPos = transform.position;
        rg = GetComponent<Rigidbody2D>();
        collidedID = new HashSet<int>();
    }

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
        if (!launched)
        {
            launched = true;
            rg.bodyType = RigidbodyType2D.Dynamic;
            rg.AddForce(force, ForceMode2D.Impulse);
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
