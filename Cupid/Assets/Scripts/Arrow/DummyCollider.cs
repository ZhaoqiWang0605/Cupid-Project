using UnityEngine;

public class DummyCollider : MonoBehaviour
{
    Rigidbody2D rg;
    
    void Start()
    {
        rg = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
