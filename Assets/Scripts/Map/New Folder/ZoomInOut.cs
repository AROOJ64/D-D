using Cinemachine;
using UnityEngine;

public class ZoomInOut : MonoBehaviour
{
    public CinemachineBrain cineBrain;
    private ICinemachineCamera activeCam;

    //zooming
    public float zoomSpeed = 0.1f;
    public float minFOV = 15f;
    public float maxFOV = 90f;

    //movement
    public float moveSpeed = 0.07f;

    private Vector2 touchStart;

    void Start()
    {
        if (cineBrain == null)
        {
            cineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }
    }

    void Update()
    {
        if (cineBrain != null)
        {
            activeCam = cineBrain.ActiveVirtualCamera;
            if (activeCam != null)
            {
                Debug.Log("Active Virtual Camera: " + activeCam.Name);

                if (IsTouchingPlane())
                {
                    //zoom in/out using touch
                    HandleZoom();

                    //movement using touch
                    HandleMovement();
                }
            }
        }
    }

    private void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            // Get the touches
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            CinemachineVirtualCamera virtualCam = activeCam as CinemachineVirtualCamera;

            if (virtualCam != null)
            {
                float currentFOV = virtualCam.m_Lens.FieldOfView;

                currentFOV += deltaMagnitudeDiff * zoomSpeed;
                currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);

                virtualCam.m_Lens.FieldOfView = currentFOV;

                Debug.Log($"Current FOV: {currentFOV}");
            }
        }
    }

    private void HandleMovement()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                Debug.Log($"Touch Start Position: {touchStart}");
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the movement based on the touch delta
                Vector2 touchDelta = touch.deltaPosition;

                Debug.Log($"Touch Delta Position: {touchDelta}");

                if (activeCam.VirtualCameraGameObject != null)
                {
                    Vector3 move = new Vector3(-touchDelta.x, -touchDelta.y, 0) * moveSpeed * Time.deltaTime;
                    activeCam.VirtualCameraGameObject.transform.Translate(move);

                    Debug.Log($"Camera Position: {activeCam.VirtualCameraGameObject.transform.position}");
                }
            }
        }
    }

    private bool IsTouchingPlane()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Plane"))
                {
                    Debug.Log("Touching Plane");
                    return true;
                }
            }
        }
        return false;
    }
}
