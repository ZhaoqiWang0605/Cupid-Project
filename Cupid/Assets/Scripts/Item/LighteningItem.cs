using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighteningItem : MonoBehaviour
{
    StageController stageController;
    // Start is called before the first frame update
    void Start()
    {
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(CoupleController cp in stageController.coupleControllers)
        {
            if (!cp.IsInLove())
            {
                cp.SetInLove();
                break;
            }
        }

        Destroy(gameObject);
    }
}
