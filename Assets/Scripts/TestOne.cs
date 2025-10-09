using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestOne : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObjects;
    ArrayList _list = new ArrayList();
    [SerializeField] GameObject _cube;
    [SerializeField] Button _btn;
    [SerializeField] Slider _slider;
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] TMP_Dropdown _dropdown;    
    [SerializeField] public  int _number = 9, i = 0;
    float _decimal = 8.0f;
    string _name = "Himanshu  89705748363w58o6";
    char _singleCharacter = 'a';
    public bool _trueFalse = false;
    object _allForOne = null;

    private void Start()
    {
        _btn.onClick.AddListener(ButtonClicked);
        _slider.onValueChanged.AddListener(SliderValueChange);
        _dropdown.onValueChanged.AddListener(DropDownValueChanged);
        _inputField.onValueChanged.AddListener(InputGiven);
        //for (int i = 0; i < _number; i++)
        //{
        //    for (int j = 2; j < _number; j++)
        //    {
        //        for (int k = 4; k < _number; k++)
        //        {
        //            for (int l = 6; l < _number; l++)
        //            {
        //                Debug.Log(i.ToString() + " i");
        //                Debug.Log(j.ToString() + " j");
        //                Debug.Log(k.ToString() + " k");
        //                Debug.Log(l.ToString() + " l");
        //            }
        //        }
        //    }
        //}
        foreach (GameObject i in _gameObjects)
        {
            //Debug.Log(i.gameObject.name);
            _list.Add(i);
            _list.Add("list");
        }
        if (_list.Contains("list"))
        {
            _list.Remove("list");
        }
        foreach (object i in _list)
        {
            _list.Remove(1);
            //Debug.Log(i);
        }
        StartCoroutine("TurnOnOffAObject");
    }

    void InputGiven(string value)
    {
        _text.text = value;
    }

    IEnumerator TurnOnOffAObject()
    {
        while (i < 10)
        {
            yield return new WaitForSeconds(1);
            if (_cube.activeSelf)
            {
                _cube.SetActive(false);
            }
            else
            {
                _cube.SetActive(true);
            }
            i++;
            Debug.Log(i);
        }
    }

    private void Update()
    {
        //Operators();
        //while (i != 9)
        //{
        //    i++;
        //    Debug.Log(i.ToString() + " i");
        //}
        //TakeValue(NumberValueTakeAndReturn(_number));
    }

    void Operators()
    {
        // = assigner _number = 9;
        // == compare  8 == 8;
        // < less then 8 < 9;
        // > greater then 9 > 8;
        // ! not operator true  == !false; (!false = true)
        // != not equal 9 != 8;
        // && left and right condition should be true 8 != 9 && 8 == 9
        // || left or right one of these should be true 8 != 9 || 8 == 9
        // <= less then equaltoo 9 <= 9
        // >= greater then equaltoo 10 >= 9
        // ++ increment by one 9++ = 10;
        // -- decrement by one 9-- = 8;

        if (_number == 9)
        {
            Debug.Log("Number is equal to 9");
        }
        else if (_number < 11 && _number  != 8)
        {
            Debug.Log("Number less then 11 and not equal to 8");
        }
        else if (_number > 17)
        {
            Debug.Log("Number greater then 9");
        }
        else if (_number != 9 && _number == 8)
        {
            Debug.Log("Number is equal to 8");
        }
        else if (_number != 9 || _number == 16)
        {
            Debug.Log("Number is equal to 16");
        }
        else
        {
            Debug.Log("Number is not equal to 9 or 10");
        }
    }

    public void ButtonClicked()
    {
        Debug.Log("Clicked");

    }
    
    public void SliderValueChange(float value)
    {
        _text.text = value.ToString();
        Debug.Log(value);
    }

    public void DropDownValueChanged(int value)
    {
        Debug.Log(value);
    }

    // no return no take\\
    public void FirstFunction()
    {
        _number = _number + 1;
        //9 = 9 + 1;
    }

    // return no take
    public int NumberValueReturn()
    {
        return _number;
    }

    //take no return
    public void TakeValue(int value)
    {
        int _kbcd = _number;
        Debug.Log(_kbcd);
    }

    //takes and returns
    public int NumberValueTakeAndReturn(int value)
    {
        return value;
    }
}
