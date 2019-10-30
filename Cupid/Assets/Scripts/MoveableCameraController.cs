using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class MoveableCameraController : MonoBehaviour
{
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    private CinemachineVirtualCamera cVirtualCamera;
    private Vector3 touchStart;

    private float upperBound = float.NegativeInfinity;
    private float lowerBound = float.PositiveInfinity;
    private float leftBound = float.PositiveInfinity;
    private float rightBound = float.NegativeInfinity;

    private bool reseted = false;
    private bool pressedOnUI = false;

    void Start()
    {
        // Get reference of cinemanchine virtual camera sub game object
        cVirtualCamera = transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

        // Calculate screen size in unity game world unit
        Vector3 p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        float unit = Vector3.Distance(p1, p2);
        float screenWidthInWorld = Screen.width * unit;
        float screenHeightInWorld = Screen.height * unit;

        // Find Upper, lower, left and right bound of cinemachine confiner
        Transform cameraConfiner = gameObject.transform.Find("CameraConfiner");
        PolygonCollider2D vcamConfiner = cameraConfiner.GetComponent<PolygonCollider2D>();
        foreach (Vector2 point in vcamConfiner.points)
        {
            if (point.y > upperBound)
            {
                upperBound = point.y;
            }

            if (point.y < lowerBound)
            {
                lowerBound = point.y;
            }

            if (point.x > rightBound)
            {
                rightBound = point.x;
            }

            if (point.x < leftBound)
            {
                leftBound = point.x;
            }
        }

        // Calculate camera moveing and zooming bound.
        //upperBound -= (screenHeightInWorld / 2);
        //lowerBound += (screenHeightInWorld / 2);
        //leftBound += (screenWidthInWorld / 2);
        //rightBound -= (screenWidthInWorld / 2);

        zoomOutMax = screenHeightInWorld / 2;

        // Move camera into confined space if it is not inside
        float newX = Mathf.Clamp(cVirtualCamera.transform.position.x, leftBound + (screenWidthInWorld / 2), rightBound - (screenWidthInWorld / 2));
        float newY = Mathf.Clamp(cVirtualCamera.transform.position.y, lowerBound + (screenHeightInWorld / 2), upperBound - (screenHeightInWorld / 2));
        cVirtualCamera.transform.position = new Vector3(newX, newY, cVirtualCamera.transform.position.z);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            pressedOnUI = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    pressedOnUI = true;
                }
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                pressedOnUI = true;
            }
        }
        if (!pressedOnUI)
        {
            if (Input.touchCount == 2)
            {
                zoomByTouch();
            }
            else if (Input.GetMouseButton(0))
            {
                moveCameraByTouch();
            }
            zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    private void moveCameraByTouch()
    {
        // Calculate screen size in unity game world unit
        Vector3 p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        float unit = Vector3.Distance(p1, p2);
        float screenWidthInWorld = Screen.width * unit;
        float screenHeightInWorld = Screen.height * unit;

        if (!reseted && (
                Mathf.Approximately(cVirtualCamera.transform.position.x, leftBound + (screenWidthInWorld / 2)) ||
                Mathf.Approximately(cVirtualCamera.transform.position.x, rightBound - (screenWidthInWorld / 2)) ||
                Mathf.Approximately(cVirtualCamera.transform.position.y, upperBound - (screenHeightInWorld / 2)) ||
                Mathf.Approximately(cVirtualCamera.transform.position.y, lowerBound + (screenHeightInWorld / 2))))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            reseted = true;
        }
        else
        {
            reseted = false;
        }

        Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float newX = Mathf.Clamp(cVirtualCamera.transform.position.x + direction.x, leftBound + (screenWidthInWorld / 2), rightBound - (screenWidthInWorld / 2));
        float newY = Mathf.Clamp(cVirtualCamera.transform.position.y + direction.y, lowerBound + (screenHeightInWorld / 2), upperBound - (screenHeightInWorld / 2));
        cVirtualCamera.transform.position = new Vector3(newX, newY, cVirtualCamera.transform.position.z);

        Debug.Log(upperBound + "/" + lowerBound);
        Debug.Log(newY);

    }

    private void zoomByTouch()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        zoom(difference * 0.01f);
    }

    public void zoom(float increment)
    {
        cVirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(cVirtualCamera.m_Lens.OrthographicSize - increment, zoomOutMin, zoomOutMax);

        // Calculate screen size in unity game world unit
        Vector3 p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        float unit = Vector3.Distance(p1, p2);
        float screenWidthInWorld = Screen.width * unit;
        float screenHeightInWorld = Screen.height * unit;

        screenHeightInWorld = cVirtualCamera.m_Lens.OrthographicSize*2;
        screenWidthInWorld = screenHeightInWorld / Screen.height * Screen.width;

        // Move camera into confined space if it is not inside
        float newX = Mathf.Clamp(cVirtualCamera.transform.position.x, leftBound + (screenWidthInWorld / 2), rightBound - (screenWidthInWorld / 2));
        float newY = Mathf.Clamp(cVirtualCamera.transform.position.y, lowerBound + (screenHeightInWorld / 2), upperBound - (screenHeightInWorld / 2));
        cVirtualCamera.transform.position = new Vector3(newX, newY, cVirtualCamera.transform.position.z);
    }

    public void setFollow(Transform follow)
    {
        cVirtualCamera.m_Follow = follow;
    }

}