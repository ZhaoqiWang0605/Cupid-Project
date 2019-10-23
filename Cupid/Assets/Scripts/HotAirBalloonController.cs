using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBalloonController : MonoBehaviour
{
    public float climbSpeed = 0.1f;
    public bool climbOnCollide = true;

    private bool isClimbing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isClimbing)
        {
            float deltaY = climbSpeed * Time.deltaTime;
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + deltaY, transform.position.z);
            transform.position = newPos;
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HotBalloon OnCollisionEnter2D");
        if (climbOnCollide)
        {
            StartCoroutine(Sleep());
        }
    }

    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(2);
        StartClimbing();
    }

    public void StartClimbing()
    {
        isClimbing = true;
    }

    public void StopClimbing()
    {
        isClimbing = false;
    }

    public bool IsClimbing()
    {
        return isClimbing;
    }
}
