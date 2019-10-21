using UnityEngine;

public class CoupleController : MonoBehaviour
{
    public ParticleSystem heartEffect;
    public StageController stageController;
    public Sprite couple_inLove_image;
    public float inLove_image_size;
    public Audio audio;

    private bool inLove = false;
    // Start is called before the first frame update
    void Start()
    {
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInLove() {
        if (!inLove)
        {
            inLove = true;
            heartEffect.Play();
            Debug.Log("CoupleController.inLove(): Set couple in love");
            GetComponent<SpriteRenderer>().sprite = couple_inLove_image;
            transform.localScale = new Vector3(inLove_image_size, inLove_image_size, 1.0f);
            stageController.UpdateScore(1000);
            //audio.PlayMusic();
        }
        Debug.Log("CoupleController.inLove(): Couple already in love");
    }

    public bool isInLove() {
        return inLove;
    }
}
