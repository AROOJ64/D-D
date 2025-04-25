using UnityEngine;
using Cinemachine;

class ScrollAndPinchPlane : MonoBehaviour
{
    public CinemachineBrain CinemachineBrain; // Reference to the Cinemachine Brain
    public bool Rotate;
    public LayerMask PlaneLayerMask; // Define a layer mask for the plane
    protected Plane Plane;

    private Camera mainCamera; // Reference to the Camera controlled by the CinemachineBrain

    private void Awake()
    {
        // Ensure that CinemachineBrain is attached and get the main camera from it
        if (CinemachineBrain == null)
            CinemachineBrain = Camera.main.GetComponent<CinemachineBrain>(); // Fallback to find the brain in the scene

        if (CinemachineBrain != null)
        {
            mainCamera = CinemachineBrain.GetComponent<Camera>(); // Get the actual camera
            if (mainCamera == null)
            {
                Debug.LogError("No Camera found with the CinemachineBrain. Please ensure a Camera is attached.");
            }
        }
        else
        {
            Debug.LogError("CinemachineBrain is missing. Please attach it to the main camera.");
        }
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // Update the plane when there's at least one touch
        if (Input.touchCount >= 1)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position); // Use the main camera
            RaycastHit hit;

            // Ensure the touch is on the plane (using the layer mask)
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlaneLayerMask))
            {
                Plane.SetNormalAndPosition(hit.transform.up, hit.transform.position);

                var Delta1 = Vector3.zero;

                // Scroll (single touch)
                if (Input.touchCount == 1)
                {
                    Delta1 = PlanePositionDelta(Input.GetTouch(0));
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                        mainCamera.transform.Translate(Delta1, Space.World);
                }

                // Pinch (two touches)
                if (Input.touchCount == 2)
                {
                    var pos1 = PlanePosition(Input.GetTouch(0).position);
                    var pos2 = PlanePosition(Input.GetTouch(1).position);
                    var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                    var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                    // Calculate zoom (pinch to zoom)
                    var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

                    // Edge case for zoom
                    if (zoom == 0 || zoom > 10)
                        return;

                    // Move the camera based on the zoom
                    mainCamera.transform.position = Vector3.LerpUnclamped(pos1, mainCamera.transform.position, 1 / zoom);

                    // Rotate the camera if required
                    if (Rotate && pos2b != pos2)
                        mainCamera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
                }
            }
        }
    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        var rayBefore = mainCamera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = mainCamera.ScreenPointToRay(touch.position);

        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = mainCamera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}