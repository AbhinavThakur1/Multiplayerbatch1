using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.EventSystems;

public class ThirdPersonCameraController : MonoBehaviour
{
    CinemachineOrbitalFollow _cameraCM;
    float _inputX, _inputY, _verticalDown = 45f, _verticalUp = 25f, _verticalClamp = 65f;
    public float _sensitivity = 300f;

    private void Start()
    {
        _cameraCM = GetComponent<CinemachineOrbitalFollow>();
        _sensitivity = PlayerPrefs.GetFloat("sens") / 2f;
    }

    void LateUpdate()
    {
        PcThirdPersonControl();
    }


    void PcThirdPersonControl()
    {
        _inputX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensitivity;
        _inputY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensitivity;
        _cameraCM.HorizontalAxis.Value += _inputX;
        if (_cameraCM.VerticalAxis.Value < _verticalDown && _cameraCM.VerticalAxis.Value > _verticalUp)
        {
            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
        }
        else if (_cameraCM.VerticalAxis.Value >= _verticalDown && Mathf.Clamp(Mathf.Atan(_inputY * Time.deltaTime * _sensitivity), -_verticalClamp, _verticalClamp) > 0)
        {
            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
        }
        else if (_cameraCM.VerticalAxis.Value <= _verticalUp && Mathf.Clamp(Mathf.Atan(_inputY * Time.deltaTime * _sensitivity), -_verticalClamp, _verticalClamp) < 0f)
        {
            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
        }
    }

    void MobileThirdPersonControl()
    {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            foreach (Touch _touch in Input.touches)
            {
                if (!EventSystem.current.IsPointerOverGameObject(_touch.fingerId))
                {
                    if (_touch.phase == TouchPhase.Began || _touch.phase == TouchPhase.Moved)
                    {
                        _inputX = _touch.deltaPosition.x * Time.deltaTime * _sensitivity;
                        _inputY = _touch.deltaPosition.y * Time.deltaTime * _sensitivity;
                        _cameraCM.HorizontalAxis.Value += _inputX;
                        if (_cameraCM.VerticalAxis.Value < _verticalDown && _cameraCM.VerticalAxis.Value > _verticalUp)
                        {
                            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
                        }
                        else if (_cameraCM.VerticalAxis.Value >= _verticalDown && Mathf.Clamp(Mathf.Atan(_inputY * Time.deltaTime * _sensitivity), -_verticalClamp, _verticalClamp) > 0)
                        {
                            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
                        }
                        else if (_cameraCM.VerticalAxis.Value <= _verticalUp && Mathf.Clamp(Mathf.Atan(_inputY * Time.deltaTime * _sensitivity), -_verticalClamp, _verticalClamp) < 0f)
                        {
                            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_inputY * Time.deltaTime * _sensitivity, -_verticalClamp, _verticalClamp);
                        }
                    }
                }
            }
        }
    }
    //initial y value = 10
    //  camera.y > 0  we haven't reached the lower limit
    //  value > 0 =  5
    // -5 + 10 = 5
    // camera.y new value will be 5 > lower limit 0
    //initial y value = 5
    //  camera.y < 20  we haven't reached the upper limit
    //  value < 0 =  -10
    // -(-10) = 10
    // camera.y new value will be 15 < upper limit 20
}
