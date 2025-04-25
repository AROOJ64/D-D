using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInZoomOut : MonoBehaviour
{
    [SerializeField] private float zoomModifierSpeed = 0.1f;
    [SerializeField] private float moveSpeed = 1.0f;

    private float touchesPrevPosDifference;
    private float touchesCurPosDifference;
    private float zoomModifier;

    private Vector2 firstTouchPrevPos;
    private Vector2 secondTouchPrevPos;

    private Vector3 touchStart;

    Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            // Zooming logic
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));

                Vector3 direction = touchStart - touchPosition;

                mainCamera.transform.position += direction * moveSpeed;

                touchStart = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));
            }
        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 10f);
    }
}
