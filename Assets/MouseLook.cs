using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 200f;

    public Transform playerBody;

    private float rotationAroundX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        RotateCamera(mouseY);
        RotatePlayerBody(mouseX);
    }

    private void RotatePlayerBody(float mouseX)
    {
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void RotateCamera(float mouseY)
    {
        rotationAroundX -= mouseY;
        rotationAroundX = Mathf.Clamp(rotationAroundX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(
            rotationAroundX,
            0f,
            0f);
    }
}
