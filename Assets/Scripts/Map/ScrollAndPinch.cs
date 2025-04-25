using UnityEngine;
using Cinemachine;

class ScrollAndPinch : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine virtual camera
    public bool Rotate; // Allows rotation during pinch
    protected Plane Plane;

    private Camera _camera; // Reference to the main camera

    private void Awake()
    {
        // If virtualCamera is not assigned, find it in the scene
        if (virtualCamera == null)
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        // Get the camera component attached to the virtual camera
        _camera = virtualCamera != null ? virtualCamera.GetComponent<CinemachineVirtualCamera>().VirtualCameraGameObject.GetComponent<Camera>() : null;

        // Initialize the Plane with the upward direction and the object's position
        Plane.SetNormalAndPosition(transform.up, transform.position);
    }

    private void Update()
    {
        // Check for touch input
        if (Input.touchCount >= 1)
        {
            Debug.Log("Touch detected: " + Input.GetTouch(0).phase);
        }

        // Ensure _camera is valid
        if (_camera == null) return;

        // Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        // Scroll (Move)
        if (Input.touchCount >= 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Debug.Log("Camera Move Delta: " + Delta1); // Debug the movement delta
                _camera.transform.Translate(Delta1, Space.World); // Move the camera
            }
        }

        // Pinch (Zoom)
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            // Calculate zoom
            var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);
            Debug.Log("Zoom: " + zoom); // Debug the zoom ratio

            // Edge case (Prevent excessive zoom)
            if (zoom == 0 || zoom > 10)
                return;

            // Move the camera along the mid ray
            _camera.transform.position = Vector3.LerpUnclamped(pos1, _camera.transform.position, 1 / zoom);

            // Rotation logic
            if (Rotate && pos2b != pos2)
            {
                Debug.Log("Rotating camera");
                _camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
            }
        }
    }



    // Calculate the change in position for a single touch
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        if (touch.phase != TouchPhase.Moved) return Vector3.zero;

        var rayBefore = _camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = _camera.ScreenPointToRay(touch.position);

        // Check if the rays intersect with the plane
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        return Vector3.zero; // No movement detected
    }

    // Get the position on the plane from screen coordinates
    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = _camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero; // No intersection with the plane
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}
