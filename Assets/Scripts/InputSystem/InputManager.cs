using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;

public class InputManager : MonoBehaviour
{
    [SerializeField] private DiceSelection dropdownManager;

    private TouchController touchControls;
    private GameObject selectedDicePrefab;
    private Camera camera;

    public DiceManager diceManager;

    private void Awake()
    {
        touchControls = new TouchController();
        camera = Camera.main;
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        if (!IsTouchOverUI())
        {
            Vector2 touchPosition = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            Ray ray = camera.ScreenPointToRay(touchPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has the "Dice Ground" tag
                if (hit.collider.CompareTag("DiceGround"))
                {
                    selectedDicePrefab = dropdownManager.GetSelectedDicePrefab();

                    if (selectedDicePrefab != null)
                    {
                        InstantDice(hit.point);
                    }
                }
                else
                {
                    Debug.Log("Touch was not on the Dice Ground.");
                }
            }
        }
    }

    private void InstantDice(Vector3 position)
    {
        Profiler.BeginSample("Instantiate Dice");
        position.y += 1f;

        GameObject dice = Instantiate(selectedDicePrefab, position, Quaternion.identity);

        Dice diceScript = dice.GetComponent<Dice>();
        diceManager.diceList.Add(diceScript);

        diceScript.DicePhysics(dice);
        Profiler.EndSample();
    }

    

    private bool IsTouchOverUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = touchControls.Touch.TouchPosition.ReadValue<Vector2>()
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }
}
