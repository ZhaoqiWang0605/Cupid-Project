using UnityEngine;

public class CoupleController : MonoBehaviour
{
    private bool inLove = false;

    public bool moveCameraAfterHit = false;
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

        cupidPosAfterHit = transform.Find("CupidAfterHitPos");
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        uIForceArrowButtonController = GameObject.Find("ArrowButton").GetComponent<UIForceArrowButtonController>();
        moveableCameraController = GameObject.Find("MoveableCamera").GetComponent<MoveableCameraController>();
        animator = GetComponent<Animator>();
        heartEffect = transform.Find("HeartEffect").GetComponent<ParticleSystem>();
        hitEffect = transform.Find("HitEffect").GetComponent<ParticleSystem>();
        arrowHitCoupleAudio = transform.Find("ArrowHitCoupleAudio").GetComponent<Audio>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetInLove();
    }

    public void SetInLove()
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
                uIForceArrowButtonController.MoveCupidTo(cupidPosAfterHit);
            }
            if (moveCameraAfterHit)
            {
                moveableCameraController.SetFollowTarget(transform);
                moveableCameraController.SwitchToFollow();
            }
        }
    }

    public bool IsInLove()
    {
        return inLove;
    }

    public void EnableFreeze()
    {
        animator.SetBool("isFreezed", true);
    }

    public void DisableFreeze()
    {
        animator.SetBool("isFreezed", false);
    }
}
