using UnityEngine;

public class CameraConfinerController : MonoBehaviour
{
    public Rigidbody2D vcamFollow;

    void Start()
    {
        Vector3 p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        float unit = Vector3.Distance(p1, p2);
        float screenWidthInWorld = Screen.width * unit;
        float screenHeightInWorld = Screen.height * unit;

        float leftBound = float.PositiveInfinity;
        float rightBound = float.NegativeInfinity;
        PolygonCollider2D vcamConfiner = GetComponent<PolygonCollider2D>();
        foreach (Vector2 point in vcamConfiner.points)
        {
            if (point.x > rightBound)
            {
                rightBound = point.x;
            }

            if (point.x < leftBound)
            {
                leftBound = point.x;
            }
        }
        
        GameObject leftConfiner = transform.GetChild(0).gameObject;
        leftConfiner.transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        leftConfiner.GetComponent<BoxCollider2D>().size = new Vector2(screenWidthInWorld, screenHeightInWorld);
        leftConfiner.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);

        GameObject rightConfiner = transform.GetChild(1).gameObject;
        rightConfiner.transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
        rightConfiner.GetComponent<BoxCollider2D>().size = new Vector2(screenWidthInWorld, screenHeightInWorld);
        rightConfiner.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
    }

    public void moveCamera(float deltaX)
    {
        Debug.Log("MoveCamera");
        Vector2 position = vcamFollow.position;
        position.x = position.x + deltaX;
        vcamFollow.MovePosition(position);
    }
}
