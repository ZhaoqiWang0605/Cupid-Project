using UnityEngine;
using System.Collections;

public class CoupleController : MonoBehaviour
{
    public ParticleSystem heartEffect;
    public ParticleSystem hitEffect;
    public StageController stageController;
    public SpriteRenderer sr;
    public Sprite couple_inLove_image;
    public float inLove_image_size;
    public Audio audio;
    public bool moveCupidAfterHit = true;

    private bool inLove = false;
    private UIForceArrowButtonController uIForceArrowButtonController;

    void Start()
    {
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        uIForceArrowButtonController = GameObject.Find("ArrowButton").GetComponent<UIForceArrowButtonController>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        setInLove();
    }

    public void setInLove() {
        if (!inLove)
        {
            inLove = true;
            stageController.UpdateScore(1000);
            hitEffect.Play();
            if (moveCupidAfterHit)
            {
                uIForceArrowButtonController.moveCupidXto(transform.position.x);
            }
            StartCoroutine(Sleep());
        }
        
    }

    private void ChangeInLoveCoupleImage()
    {
        heartEffect.Play();
        Debug.Log("CoupleController.inLove(): Set couple in love");
        GetComponent<SpriteRenderer>().sprite = couple_inLove_image;
        transform.localScale = new Vector3(inLove_image_size, inLove_image_size, 1.0f);
        //audio.PlayMusic();
    }

    IEnumerator Sleep() {
        yield return new WaitForSeconds(2);
        ChangeInLoveCoupleImage();
    }

    public bool isInLove() {
        return inLove;
    }

    public void EnableFreeze()
    {

        sr.color = new Color(0.3647f, 0.7843f, 1.0f, 1.0f);
    }

    public void DisableFreeze()
    {
        sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
