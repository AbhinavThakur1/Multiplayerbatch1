using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _orientation, _thirdPersonCamera, _firtPersonCamera;
    [SerializeField] Image _healthBar;
    AudioSource _audioSource;
    FixedJoystick _joystick;
    PhotonView _photonView;
    Slider _progressSlider, _senstivity;
    [SerializeField] AudioClip _jumpSound, _gunSound;
    Rigidbody _rb;
    Vector3 _inputDir;
    Animator _animator;
    [SerializeField] float _jumpForce, _groundCheckMaxDistance, _speed, _walkSpeed = 20f, _runSpeed = 40f , _bulletDistance;
    [SerializeField] LayerMask _layerMask;
    RaycastHit _rayHit;
    float _inputY, _inputX;
    public float _sens = 300f, _health, _maxhealth = 100f;

    void Start()
    {
        _health = _maxhealth;
        _photonView = GetComponent<PhotonView>();
        _photonView.RPC("HealthUpdate", RpcTarget.All, _health);
        _progressSlider = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Slider>();
        _senstivity = GameObject.FindGameObjectWithTag("Finish").GetComponent<Slider>();
        if (!PlayerPrefs.HasKey("sens"))
        {
            PlayerPrefs.SetFloat("sens", _sens);
            _senstivity.value = _sens;
        }
        else
        {
            _sens = PlayerPrefs.GetFloat("sens");
            _senstivity.value = _sens;
        }
        if (!_photonView.IsMine)
        {
            _thirdPersonCamera.SetActive(false);
            _firtPersonCamera.SetActive(false);
        }
        _senstivity.onValueChanged.AddListener(SliderValueChange);
        //_joystick = FindFirstObjectByType<FixedJoystick>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.linearDamping = 4f;
        _animator = GetComponent<Animator>();
        _speed = _walkSpeed;
    }

    void SliderValueChange(float value)
    {
        _sens = value;
        PlayerPrefs.SetFloat("sens", _sens);
        _firtPersonCamera.GetComponent<FirstPersonCameraController>()._sensitivity = _sens;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            KeyControls();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Movement();
        }
        _healthBar.fillAmount = _health / _maxhealth;
    }

    void Movement()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
        //_inputX = _joystick.Horizontal;
        //_inputY = _joystick.Vertical;

        if (_firtPersonCamera.activeSelf)
        {
            FirstPersonCameraOn();
        }
        else
        {
            ThirPersonCameraOn();
        }
    }

    void FirstPersonCameraOn()
    {
        _inputDir = _firtPersonCamera.transform.forward * _inputY + _firtPersonCamera.transform.right * _inputX;
        if (_inputDir.magnitude != 0)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            _rb.AddForce(_inputDir.normalized * _speed, ForceMode.Force);
            _animator.SetInteger("Speed", (int)(_speed / 20f));
        }
        else
        {
            _audioSource.Stop();
            _animator.SetInteger("Speed", 0);
        }
    }

    void ThirPersonCameraOn()
    {
        _orientation.transform.forward = transform.position -
    new Vector3(_thirdPersonCamera.transform.position.x, transform.position.y, _thirdPersonCamera.transform.position.z);
        _inputDir = _orientation.transform.forward * _inputY + _orientation.transform.right * _inputX;
        if (_inputDir.magnitude != 0)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            _rb.AddForce(_inputDir.normalized * _speed, ForceMode.Force);
            _animator.SetInteger("Speed", (int)(_speed / 20f));
            transform.forward = Vector3.Lerp(transform.forward, _inputDir.normalized, 6f * Time.deltaTime);
        }
        else
        {
            _audioSource.Stop();
            _animator.SetInteger("Speed", 0);
        }
    }

    void KeyControls()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = 40;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 20;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var ray =  _firtPersonCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _rayHit, _bulletDistance);
        }
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("Fire", true);
            StartCoroutine(FireTurnOn());
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _animator.SetBool("Fire", false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_firtPersonCamera.activeSelf)
            {
                _firtPersonCamera.SetActive(false);
                _thirdPersonCamera.SetActive(true);
            }
            else
            {
                _firtPersonCamera.SetActive(true);
                _thirdPersonCamera.SetActive(false);
            }
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if(SceneManager.GetActiveScene().buildIndex < 2)
        //    {
        //        _scene++;
        //        PlayerPrefs.SetInt("level", _scene);
        //        StartCoroutine(LoadingScreen(_scene));
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (SceneManager.GetActiveScene().buildIndex > 0)
        //    {
        //        _scene--;
        //        PlayerPrefs.SetInt("level", _scene);
        //        StartCoroutine(LoadingScreen(_scene));
        //    }
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.position -= new Vector3(1f, 0f, 0f);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    transform.position += new Vector3(1f, 0f, 0f);
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position += new Vector3(0f, 0f, 1f);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position -= new Vector3(0f, 0f, 1f);
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("mouse key Clicked");
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    Debug.Log("mouse key is in Hold");
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    Debug.Log("mouse key Unclicked");
        //}
    }
    IEnumerator LoadingScreen(int _sceneIndex)
    {
        yield return new WaitForSeconds(0.1f);
        AsyncOperation _loadingScene = SceneManager.LoadSceneAsync(_sceneIndex);
        while (!_loadingScene.isDone)
        {
            _progressSlider.value = _loadingScene.progress;
            yield return null;
        }
    }

    IEnumerator FireTurnOn()
    {
        while (_animator.GetBool("Fire"))
        {
            Fire();
            Debug.Log("fire");
            _audioSource.PlayOneShot(_gunSound);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Fire()
    {
        if (_firtPersonCamera.activeSelf)
        {
            Physics.Raycast(_firtPersonCamera.transform.position, _firtPersonCamera.transform.forward, out _rayHit, _bulletDistance);
            if (_rayHit.collider != null)
            {
                if (_rayHit.collider.GetComponent<EnemyAi>())
                {
                    _rayHit.collider.GetComponent<EnemyAi>().GotHit(gameObject);
                }
            }
        }
        else
        {
            Physics.Raycast(_thirdPersonCamera.transform.position, _thirdPersonCamera.transform.forward, out _rayHit, _bulletDistance);
            if (_rayHit.collider != null)
            {
                if (_rayHit.collider.GetComponent<EnemyAi>())
                {
                    _rayHit.collider.GetComponent<EnemyAi>().GotHit(gameObject);
                }
            }
        }
    }

    void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, _groundCheckMaxDistance, _layerMask))
        {
            _animator.SetBool("Jump", true);
        }
    }

    public void JumpForce()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        _audioSource.PlayOneShot(_jumpSound);
    }

    public void JumpReset()
    {
        _animator.SetBool("Jump", false);
    }

    public void GotHit()
    {
        if (_health > 0)
        {
            _health -= 20f;
            _photonView.RPC("HealthUpdate", RpcTarget.AllBuffered, _health);
            //Debug.Log("Sad guy hit me");
        }
    }

    [PunRPC]
    void HealthUpdate(float _newHealth)
    {
        _health = _newHealth;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.collider.gameObject.name + "Collision start");
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log(collision.collider.gameObject.name + "is Colliding");
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log(collision.collider.gameObject.name + "is Collision ended");
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name + "Entered");
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log(other.gameObject.name + "is inside");
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log(other.gameObject.name + "Exited");
    //}
}
