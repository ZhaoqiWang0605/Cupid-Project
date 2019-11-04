using UnityEngine;
using System.Collections;

public class AnimatedCoupleController : MonoBehaviour
{
    public ParticleSystem heartEffect;
    public ParticleSystem hitEffect;
    public StageController stageController;
    private SpriteRenderer sr;
    private Audio audio;
    public bool moveCupidAfterHit = true;
    public Transform cupidPosAfterHit;

    private bool inLove = false;
    private UIForceArrowButtonController uIForceArrowButtonController;
    private MoveableCameraController moveableCameraController;
    private Animator animator;

    void Start()
    {
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        uIForceArrowButtonController = GameObject.Find("ArrowButton").GetComponent<UIForceArrowButtonController>();
        moveableCameraController = GameObject.Find("MoveableCamera").GetComponent<MoveableCameraController>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        setInLove();
    }

    public void setInLove()
    {
        if (!inLove)
        {
            inLove = true;
            animator.SetBool("inLove", true);
            stageController.UpdateScore(1000);
            //audio.PlayMusic();
            hitEffect.Play();
            if (moveCupidAfterHit)
            {
                //uIForceArrowButtonController.moveCupidXto(transform.position.x);
                uIForceArrowButtonController.moveCupidTo(cupidPosAfterHit);
            }
            moveableCameraController.setFollow(transform);
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Sleep());
        }

    }

    private void ChangeInLoveCoupleImage()
    {
        heartEffect.Play();
        Debug.Log("CoupleController.inLove(): Set couple in love");
    }

    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(2);
        ChangeInLoveCoupleImage();
    }

    public bool isInLove()
    {
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
