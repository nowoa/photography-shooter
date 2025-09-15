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

    private float _viewFinderWidth = 1920f;
    public int ViewFinderChangeSpeed = 100;
    public RectTransform ViewfinderImage;
    private int _imageWidth = 1920 / 4;
    private int _imageHeight = 1080 / 4;

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TakePicture();
        }

        UpdateViewfinder();

    }

    private void UpdateViewfinder()
    {
        _viewFinderWidth += Input.mouseScrollDelta.y * ViewFinderChangeSpeed * Time.deltaTime;
        _viewFinderWidth = Mathf.Clamp(_viewFinderWidth, 800, 1920);
        ViewfinderImage.sizeDelta = new Vector2(_viewFinderWidth, 1080);
        _imageWidth = (int)_viewFinderWidth / 4;
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
        
        pictureTexture = new Texture2D(_imageWidth,_imageHeight);
        picture.rectTransform.sizeDelta = new Vector2(_imageWidth, _imageHeight);
        picture.texture = pictureTexture;
        temp = RenderTexture.GetTemporary(_imageWidth, _imageHeight, 24, GraphicsFormat.R8G8B8A8_SRGB);
        RenderCamera.targetTexture = temp;
    }

    
    private void TakePicture()
    {
        SetAspectRatio();
        
        RenderCamera.transform.position = _camera.transform.position;
        RenderCamera.transform.rotation = _camera.transform.rotation;
        RenderCamera.Render();
        RenderTexture.active = RenderCamera.targetTexture;
        pictureTexture.ReadPixels(new Rect(0,0,_imageWidth,_imageHeight),0,0);
        pictureTexture.Apply();
        RenderTexture.active = null;
        
        /*RenderCamera.Render();*/
    }
}
