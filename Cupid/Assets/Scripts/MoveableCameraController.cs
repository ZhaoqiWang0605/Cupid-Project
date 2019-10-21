using UnityEngine;
using Cinemachine;

public class MoveableCameraController : MonoBehaviour
{
    
    private Rigidbody2D vcamFollow;
    private CinemachineVirtualCamera cVirtualCamera;

    void Start()
    {
        // Get reference of cinemanchine virtual camera sub game object
        cVirtualCamera = transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

        // Get reference of CameraFollow sub game object
        vcamFollow = transform.Find("CameraFollw").GetComponent<Rigidbody2D>();

        // Calculate screen size in unity game world unit
        Vector3 p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        float unit = Vector3.Distance(p1, p2);
        float screenWidthInWorld = Screen.width * unit;
        float screenHeightInWorld = Screen.height * unit;

        // Find left and right bound of cinemachine confiner
        Transform cameraConfiner = gameObject.transform.Find("CameraConfiner");
        float leftBound = float.PositiveInfinity;
        float rightBound = float.NegativeInfinity;
        PolygonCollider2D vcamConfiner = cameraConfiner.GetComponent<PolygonCollider2D>();
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

        // Instanciate left and right bound for CameraFollow object using bounds of cinamachine confiner
        GameObject leftConfiner = new GameObject("CameraLeftConfiner");
        BoxCollider2D leftBoxCollider = leftConfiner.AddComponent<BoxCollider2D>();
        leftConfiner.transform.parent = cameraConfiner;
        leftConfiner.layer = 9;
        leftConfiner.transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        leftBoxCollider.size = new Vector2(screenWidthInWorld, screenHeightInWorld);
        leftBoxCollider.offset = new Vector2(0, 0);


        GameObject rightConfiner = new GameObject("CameraRightConfiner");
        BoxCollider2D rightBoxCollider = rightConfiner.AddComponent<BoxCollider2D>();
        rightConfiner.transform.parent = cameraConfiner;
        rightConfiner.layer = 9;
        rightConfiner.transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
        rightBoxCollider.size = new Vector2(screenWidthInWorld, screenHeightInWorld);
        rightBoxCollider.offset = new Vector2(0, 0);
    }

    public void moveCamera(float deltaX)
    {
        Vector2 position = vcamFollow.position;
        position.x = position.x + deltaX;
        vcamFollow.MovePosition(position);
    }

    public void setFollowPosition(Vector2 position)
    {
        vcamFollow.MovePosition(position);
    }
}

