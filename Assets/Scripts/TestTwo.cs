using UnityEngine;

public class TestTwo : MonoBehaviour
{
    [SerializeField] TestOne _test;
    int _numbers = 1;

    private void Start()
    {
        
    }

    public void ButtonClicked()
    {
        _test.FirstFunction();
    }

}
