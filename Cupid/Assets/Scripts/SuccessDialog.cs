using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessDialog : MonoBehaviour
{

    public Text stars;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowStar(int starNumber)
    {
        if (starNumber == 1)
        {
            stars.text = "✪";
        } else if (starNumber == 2)
        {
            stars.text = "✪ ✪";
        } else if (starNumber == 3)
        {
            stars.text = "✪ ✪ ✪";
        }
    }
}
