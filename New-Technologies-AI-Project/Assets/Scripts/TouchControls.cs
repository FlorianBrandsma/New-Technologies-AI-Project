using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchControls : MonoBehaviour
{
    public Camera UICamera;

    public RawImage joystick;
    public RawImage joystickDirection;

    public float minSensitivity;

    private Vector2 startPos;
    private Vector2 sensitivity;

    private float joystickOffset = 1.5f;
    private float joystickSize = 2f;
    private float speed = 1f;

    public IPlayable Playable { get { return GetComponent<IPlayable>(); } }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitializeJoystick();
        }

        if (Input.GetMouseButton(0))
        {
            Move();
            Rotate();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DeactivateJoystick();
            Playable.StopMoving();
        }
    }

    private void InitializeJoystick()
    {
        startPos.x = Input.mousePosition.x;
        startPos.y = Input.mousePosition.y;

        joystick.transform.localScale = new Vector3(joystickSize, joystickSize, 1);
        joystick.transform.position = new Vector3(UICamera.ScreenToWorldPoint(Input.mousePosition).x, UICamera.ScreenToWorldPoint(Input.mousePosition).y, joystick.transform.position.z);

        joystick.gameObject.SetActive(true);
    }

    private void DeactivateJoystick()
    {
        joystick.gameObject.SetActive(false);
    }

    private void Move()
    {
        sensitivity.x = Mathf.Clamp(((Input.mousePosition.x - startPos.x) / joystick.rectTransform.rect.width) * joystickOffset, -1, 1);
        sensitivity.y = Mathf.Clamp(((Input.mousePosition.y - startPos.y) / joystick.rectTransform.rect.height) * joystickOffset, -1, 1);

        if(sensitivity.x > minSensitivity || sensitivity.y > minSensitivity || sensitivity.x < -minSensitivity || sensitivity.y < -minSensitivity)
        {
            Playable.Move(Mathf.Abs(sensitivity.x) > Mathf.Abs(sensitivity.y) ? Mathf.Abs(sensitivity.x) : Mathf.Abs(sensitivity.y));
        }
    }

    private void Rotate()
    {
        RotateJoystickDirection();
        RotateCharacter();
    }

    private void RotateJoystickDirection()
    {
        Vector3 screenPos = UICamera.WorldToScreenPoint(joystickDirection.transform.position);

        float angle = Mathf.Atan2(Input.mousePosition.x - screenPos.x, Input.mousePosition.y - screenPos.y) / (2 * Mathf.PI) * 360;

        joystickDirection.transform.eulerAngles = new Vector3(0, 0, -angle);

        float newDist = Mathf.Clamp01(Vector3.Distance(screenPos, Input.mousePosition) / 65);

        joystickDirection.transform.localScale = new Vector3(0.5f + newDist / 2, 0.5f + newDist / 2, 1);
    }

    private void RotateCharacter()
    {
        transform.eulerAngles = new Vector3(0, -joystickDirection.transform.eulerAngles.z, 0);
    }
}