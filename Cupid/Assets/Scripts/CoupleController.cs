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

    private bool inLove = false;
    // Start is called before the first frame update
    void Start()
    {
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInLove() {
        if (!inLove)
        {
            inLove = true;
            hitEffect.Play();
            StartCoroutine(SleepAWhile());
        }
        Debug.Log("CoupleController.inLove(): Couple already in love");
    }

    private void ChangeInLoveCoupleImage()
    {
        heartEffect.Play();
        Debug.Log("CoupleController.inLove(): Set couple in love");
        GetComponent<SpriteRenderer>().sprite = couple_inLove_image;
        transform.localScale = new Vector3(inLove_image_size, inLove_image_size, 1.0f);
        stageController.UpdateScore(1000);
        //audio.PlayMusic();
    }

    IEnumerator SleepAWhile() {
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
