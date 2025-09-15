using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float Speed = 1f;
    public float Sensitivity = 0.1f;
    public Image picture;
    private Texture2D _pictureTexture;
    private Camera _camera;
    private Texture2D pictureTexture;

    public Camera RenderCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pictureTexture = new Texture2D(400, 225);
        picture.material.mainTexture = pictureTexture;
        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;
        RenderCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TakePicture();
        }
    }

    private void Move()
    {
        Vector3 movementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movementVector += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
             movementVector -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector += transform.right;
        }

        movementVector = movementVector.normalized;
        transform.position += movementVector * (Speed * Time.deltaTime);
    }

    private float _pitch = 0f;
    private void Look()
    {
        Vector3 mouseDelta = Input.mousePositionDelta;
        _pitch -= mouseDelta.y * Sensitivity;
        _camera.transform.localRotation = Quaternion.Euler(_pitch,0f,0f);
        transform.Rotate(transform.up, mouseDelta.x * Sensitivity);
    }

    
    private void TakePicture()
    {
        RenderCamera.transform.position = _camera.transform.position;
        RenderCamera.transform.rotation = _camera.transform.rotation;
        RenderCamera.Render();
        RenderTexture.active = RenderCamera.targetTexture;
        pictureTexture.ReadPixels(new Rect(0,0,400,225),0,0);
        pictureTexture.Apply();
        /*RenderCamera.Render();*/
    }
}
