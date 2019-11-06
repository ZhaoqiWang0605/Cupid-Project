using UnityEngine;

public class CoupleController : MonoBehaviour
{
    private bool inLove = false;

    public bool moveCupidAfterHit = true;
    private Transform cupidPosAfterHit;

    private StageController stageController;
    private UIForceArrowButtonController uIForceArrowButtonController;
    private MoveableCameraController moveableCameraController;

    private Animator animator;
    private ParticleSystem heartEffect;
    private ParticleSystem hitEffect;
    private Audio arrowHitCoupleAudio;

    void Start()
    {
        
        cupidPosAfterHit = transform.GetChild(3);
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        uIForceArrowButtonController = GameObject.Find("ArrowButton").GetComponent<UIForceArrowButtonController>();
        moveableCameraController = GameObject.Find("MoveableCamera").GetComponent<MoveableCameraController>();
        animator = GetComponent<Animator>();
        heartEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
        hitEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
        arrowHitCoupleAudio = transform.GetChild(2).GetComponent<Audio>();
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
            stageController.UpdateScore(1000);
            GetComponent<BoxCollider2D>().enabled = false;

            animator.SetBool("inLove", true);
            arrowHitCoupleAudio.PlayMusic();
            hitEffect.Play();
            heartEffect.Play();

            if (moveCupidAfterHit)
            {
                uIForceArrowButtonController.moveCupidTo(cupidPosAfterHit);
            }
            moveableCameraController.setFollow(transform);
        }

    }

    public bool isInLove()
    {
        return inLove;
    }

    public void EnableFreeze()
    {
        animator.SetBool("freeze", true);
    }

    public void DisableFreeze()
    {
        animator.SetBool("isFreezed", false);
    }
}
