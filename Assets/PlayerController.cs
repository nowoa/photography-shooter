using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float Speed = 1f;
    public float Sensitivity = 0.1f;
    public RawImage picture;
    private Texture2D _pictureTexture;
    private Camera _camera;
    private Texture2D pictureTexture;
    public Camera RenderCamera;
    private RenderTexture temp;

    [FormerlySerializedAs("Width")] public int imgWidth;

    [FormerlySerializedAs("Height")] public int imgHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;
        RenderCamera.enabled = false;
        SetAspectRatio();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetAspectRatio();
        }
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

    private void SetAspectRatio()
    {
        // cleanup old textures if they exist
        /*if (pictureTexture != null)
        {
            Destroy(pictureTexture);
            pictureTexture = null;
        }*/
        if (temp != null)
        {
            temp.Release();
        }
        
        pictureTexture = new Texture2D(imgWidth, imgHeight);
        Debug.Log(pictureTexture);
        picture.texture = pictureTexture;
        picture.rectTransform.sizeDelta = new Vector2(imgWidth, imgHeight);
        temp = RenderTexture.GetTemporary(imgWidth, imgHeight, 24, GraphicsFormat.R8G8B8A8_SRGB);
        RenderCamera.targetTexture = temp;
        TakePicture();
    }

    
    private void TakePicture()
    {
        RenderCamera.transform.position = _camera.transform.position;
        RenderCamera.transform.rotation = _camera.transform.rotation;
        RenderCamera.Render();
        RenderTexture.active = RenderCamera.targetTexture;
        pictureTexture.ReadPixels(new Rect(0,0,imgWidth,imgHeight),0,0);
        pictureTexture.Apply();
        RenderTexture.active = null;
        
        /*RenderCamera.Render();*/
    }
}
