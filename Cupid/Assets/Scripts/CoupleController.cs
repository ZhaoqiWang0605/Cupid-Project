using UnityEngine;

public class CoupleController : MonoBehaviour
{
    public ParticleSystem heartEffect;
    public StageController stageController;

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
        inLove = true;
        heartEffect.Play();
        Debug.Log("CoupleController.inLove(): Couple inLoved");
        stageController.UpdateScore(0);
        audio.PlayMusic();
    }

    public bool isInLove() {
        return inLove;
    }
}
