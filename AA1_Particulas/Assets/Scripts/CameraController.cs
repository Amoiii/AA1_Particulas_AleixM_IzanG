using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Velocidades")]
    public float moveSpeed = 200000f;           
    public float mouseSensitivity = 2f;     
    public float zoomSpeed = 500f;          
    public float panSpeed = 0.5f;           

  
    private float yaw;
    private float pitch;

    void Start()
    {
      
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
       
        if (Input.GetMouseButton(2))
        {
            float panX = -Input.GetAxis("Mouse X") * panSpeed;
            float panY = -Input.GetAxis("Mouse Y") * panSpeed;
            transform.Translate(new Vector3(panX, panY, 0f), Space.Self);
        }
        else
        {
           
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * mouseSensitivity;
            pitch -= mouseY * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -80f, 80f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // Movimiento con teclado (WASD o flechas)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        transform.position += move * moveSpeed * Time.deltaTime;

        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * zoomSpeed * Time.deltaTime, Space.Self);

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
