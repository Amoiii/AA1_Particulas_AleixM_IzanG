using UnityEngine;

public class CameraController : MonoBehaviour
{
   
    public float moveSpeed = 200000f;           
    public float mouseSensitivity = 2f;         
    public float zoomSpeed = 500f;             
    public float panSpeed = 0.5f;              

   
    public Transform[] followTargets;
   
    public Vector3 followOffset = new Vector3(0, 50, -100);
    
    public Vector3 closeFollowOffset = new Vector3(0, 10, -20);
    
    public float followSpeed = 5f;

    private float yaw;
    private float pitch;
    private Transform currentTarget = null; 

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
        // Si se presionan WASD o flechas, se cancela el seguimiento.
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f)
        {
            currentTarget = null;
        }

        // Asigna un target según la tecla numérica pulsada
        if (Input.GetKeyDown(KeyCode.Alpha0) && followTargets.Length > 0)
        {
            currentTarget = followTargets[0];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && followTargets.Length > 1)
        {
            currentTarget = followTargets[1];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && followTargets.Length > 2)
        {
            currentTarget = followTargets[2];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && followTargets.Length > 3)
        {
            currentTarget = followTargets[3];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && followTargets.Length > 4)
        {
            currentTarget = followTargets[4];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && followTargets.Length > 5)
        {
            currentTarget = followTargets[5];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && followTargets.Length > 6)
        {
            currentTarget = followTargets[6];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && followTargets.Length > 7)
        {
            currentTarget = followTargets[7];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && followTargets.Length > 8)
        {
            currentTarget = followTargets[8];
            followOffset = closeFollowOffset;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && followTargets.Length > 9)
        {
            currentTarget = followTargets[9];
            followOffset = closeFollowOffset;
        }

        if (currentTarget != null)
        {
            // Ajustar zoom relativo al target con la rueda: modifica el offset Z.
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.001f)
            {
                followOffset.z -= scroll * zoomSpeed * Time.deltaTime;
            }
            // Movimiento suave hacia el objetivo + offset y rotación para mirar al target.
            Vector3 targetPos = currentTarget.position + followOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
            Quaternion targetRot = Quaternion.LookRotation(currentTarget.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * followSpeed);
        }
        else
        {
            // Sin target: permite el control manual.
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

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            transform.position += move * moveSpeed * Time.deltaTime;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            transform.Translate(Vector3.forward * scroll * zoomSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
