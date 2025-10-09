using UnityEngine;
using UnityEngine.EventSystems;

public class FirstPersonCameraController : MonoBehaviour
{
    [SerializeField] Transform _player;
    float _inputX, _inputY, _rotationX, _rotationY, _verticalDown = 45f, _verticalUp = -45, _verticalClamp = 10f;
    public float _sensitivity = 300f, _valuex;

    private void Start()
    {
        _sensitivity = PlayerPrefs.GetFloat("sens");
    }

    void Update()
    {
        PcFirstPersonControl();
    }

    void PcFirstPersonControl()
    {
        _inputX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensitivity;
        //_inputY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensitivity;
        _rotationY += _inputX;
        _valuex = Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensitivity * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
        if (_rotationX < _verticalDown && _rotationX > _verticalUp)
        {
            _rotationX -= _valuex;
        }
        else if (_rotationX < _verticalUp && _valuex < 0f)
        {
            _rotationX -= _valuex;
        }
        else if (_rotationX > _verticalDown && _valuex > 0f)
        {
            _rotationX -= _valuex;
        }
        transform.rotation = Quaternion.Euler(new Vector3(_rotationX, _rotationY, 0f));
        _player.rotation = Quaternion.Euler(new Vector3(0f, _rotationY, 0f));
    }

    void MobileFirstPersonControl()
    {
        if(Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            foreach (Touch _touch in Input.touches)
            {
                if (!EventSystem.current.IsPointerOverGameObject(_touch.fingerId))
                {
                    if (_touch.phase == TouchPhase.Began || _touch.phase == TouchPhase.Moved)
                    {
                        _inputX = _touch.deltaPosition.x * Time.deltaTime * _sensitivity;
                        _inputY = _touch.deltaPosition.y * Time.deltaTime * _sensitivity;
                        _rotationY += _inputX;
                        if (_rotationX < _verticalDown && _rotationX > _verticalUp)
                        {
                            _rotationX -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
                        }
                        else if (_rotationX < _verticalUp && Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp) < 0f)
                        {
                            _rotationX -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
                        }
                        else if (_rotationX > _verticalDown && Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp) > 0f)
                        {
                            _rotationX -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
                        }
                        transform.rotation = Quaternion.Euler(new Vector3(_rotationX, _rotationY, 0f));
                        _player.rotation = Quaternion.Euler(new Vector3(0f, _rotationY, 0f));
                    }
                }
            }
        }
    }

}
