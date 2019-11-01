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
    private bool startedCurrentDragOnUI = false;

    void Start()
    {
        // Get reference of cinemanchine virtual camera sub game object
        cVirtualCamera = transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

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

        // Calculate screen height in unity game world unit and camera zooming bound.
        float screenHeightInWorld = cVirtualCamera.m_Lens.OrthographicSize * 2;
        zoomOutMax = screenHeightInWorld / 2;

        // Move camera into confined space if it is not inside
        ClampCmaera();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            startedCurrentDragOnUI = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startedCurrentDragOnUI = IsPointerOverGameObject();
        }
        if (!startedCurrentDragOnUI)
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
        ClampCmaera();
    }

    private void moveCameraByTouch()
    {
        // Calculate screen size in unity game world unit
        float screenHeightInWorld = cVirtualCamera.m_Lens.OrthographicSize * 2;
        float screenWidthInWorld = screenHeightInWorld / Screen.height * Screen.width;

        // Reset TouchStart point if the camera has touched boundary
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
        cVirtualCamera.transform.position += direction;
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

    private void zoom(float increment)
    {
        cVirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(cVirtualCamera.m_Lens.OrthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    private void ClampCmaera()
    {
        // Calculate screen size in unity game world unit
        float screenHeightInWorld = cVirtualCamera.m_Lens.OrthographicSize * 2;
        float screenWidthInWorld = screenHeightInWorld / Screen.height * Screen.width;

        // Move camera into confined space if it is not inside
        float newX = Mathf.Clamp(cVirtualCamera.transform.position.x, leftBound + (screenWidthInWorld / 2), rightBound - (screenWidthInWorld / 2));
        float newY = Mathf.Clamp(cVirtualCamera.transform.position.y, lowerBound + (screenHeightInWorld / 2), upperBound - (screenHeightInWorld / 2));
        cVirtualCamera.transform.position = new Vector3(newX, newY, cVirtualCamera.transform.position.z);
    }

    private bool IsPointerOverGameObject()
    {
        // Check if any touch is on UI
        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                return true;
            }
        }

        // Check if mouse pointer is on UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }

    public void setFollow(Transform follow)
    {
        cVirtualCamera.m_Follow = follow;
    }

}