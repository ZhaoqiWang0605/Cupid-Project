using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeItem : MonoBehaviour
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
        ForceArrowController arrow = collision.gameObject.GetComponent<ForceArrowController>();
        arrow.uIForceArrowButtonController.nextArrow();
        Destroy(arrow.gameObject);
        Destroy(gameObject);

        foreach (CoupleController cp in stageController.coupleControllers)
        {
            SpriteRenderer sr = cp.gameObject.GetComponent<SpriteRenderer>();
            //sr.color = new Color(93, 200, 255);
            print(sr.color);
            sr.color = new Color(0.3647f, 0.7843f, 1.0f, 1.0f);
            //sr.material = Resources.Load<Material>("Default-Line");
        }
    }
}
