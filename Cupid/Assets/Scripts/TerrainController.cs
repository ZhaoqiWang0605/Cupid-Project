using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*ForceArrowController arrow = collision.gameObject.GetComponent<ForceArrowController>();
        if (arrow != null) {
            Debug.Log("TerrainController.OnCollisionEnter2D(): Got ForceArrowController reference");
            Destroy(arrow.gameObject);
        }*/
    }
}
