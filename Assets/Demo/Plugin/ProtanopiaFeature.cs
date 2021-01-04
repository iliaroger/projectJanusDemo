using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using UnityEngine.UI;

public class ProtanopiaFeature : MonoBehaviour
{

    // ui reference
    public GameObject SelectedBorder;
    public GameObject UnselectedBorder;
    public GameObject FeatureText;
    public GameObject FeatureSelectedIcon;
    public GameObject ActiveSlot1;
    public GameObject InactiveSlot1;
    public GameObject ActiveSlot2;
    public GameObject InactiveSlot2;
    public GameObject ActiveSlot3;
    public GameObject InactiveSlot3;
    public GameObject CustomPanel;
    public GameObject GreenChannelSlider;
    public GameObject BlueChannelSlider;

    // ui components
    private TextMeshProUGUI _featureText;
    private Slider _greenChannelSlider;
    private Slider _blueChannelSlider;
    // ui state
    static bool ProtanopiaActive = false;
    private _modes _activeMode = _modes.unselected;
    private enum _modes
    {
        unselected, 
        preset1,
        preset2,
        preset3,
    }

    // dictionary objects
    private IDictionary<int, Color> _initObjectsData = new Dictionary<int, Color>();
    private List<Object> _storedObjects = new List<Object>();

    // init color values 
    private Dictionary<int, float> _redColorValues = new Dictionary<int, float>();
    private Dictionary<int, float> _greenColorValues = new Dictionary<int, float>();
    private Dictionary<int, float> _blueColorValues = new Dictionary<int, float>();

    // protanopia + custom sliders
    private void Preset1()
    {
        SaveObjectColor();
        ApplyProtanopia();
        ProtanopiaActive = true;
    }

    private void Preset2()
    {
        ProtanopiaActive = true;
    }

    private void Preset3()
    {
        Foe[] _foeSubjects = FindObjectsOfType(typeof(Foe)) as Foe[];
        foreach (Foe foeSubject in _foeSubjects)
        {
            Renderer _foeRenderer = foeSubject.GetComponent<Renderer>();
            _foeRenderer.material.color = new Color(0, _foeRenderer.material.color.g, _foeRenderer.material.color.b);
        }
        ProtanopiaActive = true;
    }

    private void SaveObjectColor()
    {
        // create a dictionary and save unique instance id and color
        Object[] _allGameObjects = FindObjectsOfType<GameObject>();
        Stopwatch _calc = new Stopwatch();
        _calc.Start();
        foreach (GameObject obj in _allGameObjects)
        {
            FirstPersonAIO _hasControllerScript = obj?.GetComponent<FirstPersonAIO>();
            MeshRenderer _hasMeshRenderer = obj?.GetComponent<MeshRenderer>();
            Ignore _hasIgnoreComponent = obj?.GetComponent<Ignore>();
            if (_hasControllerScript == null)
            {
                if (_hasMeshRenderer != null && _hasIgnoreComponent == null)
                {
                    Renderer _checkColor = obj?.GetComponent<Renderer>();
                    _storedObjects.Add(obj);
                    _redColorValues.Add(obj.GetInstanceID(), _checkColor.material.color.r);
                    _greenColorValues.Add(obj.GetInstanceID(), _checkColor.material.color.g);
                    _blueColorValues.Add(obj.GetInstanceID(), _checkColor.material.color.b);
                }
            }
        }
        _calc.Stop();
        
        string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            _calc.Elapsed.Hours, _calc.Elapsed.Minutes, _calc.Elapsed.Seconds,
            _calc.Elapsed.Milliseconds / 10);

        UnityEngine.Debug.Log(elapsedTime);
    }

    private void ApplyProtanopia()
    {
        foreach (GameObject obj in _storedObjects)
        {
            try
            {
                Renderer _objRenderer = obj?.GetComponent<Renderer>();

                if (_objRenderer != null)
                {
                    _objRenderer.material.color = new Color(0, _objRenderer.material.color.g, _objRenderer.material.color.b);
                }
            }
            catch
            {
                throw new System.Exception("couldnt fetch component from gameobject");
            }
        }
    }

    private void ResetProtanopia()
    {
        foreach (GameObject obj in _storedObjects)
        {
            foreach (KeyValuePair<int, float> redPair in _redColorValues)
            {
                if (obj.GetInstanceID() == redPair.Key)
                {
                    foreach (KeyValuePair<int, float> greenPair in _greenColorValues)
                    {
                        if (obj.GetInstanceID() == greenPair.Key)
                        {
                            foreach (KeyValuePair<int, float> bluePair in _blueColorValues)
                            {
                                if (obj.GetInstanceID() == bluePair.Key)
                                {
                                    Renderer _objRenderer = obj?.GetComponent<Renderer>();

                                    if (_objRenderer != null)
                                    {
                                        _objRenderer.material.color = new Color(redPair.Value, greenPair.Value, bluePair.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        _redColorValues.Clear();
        _greenColorValues.Clear();
        _blueColorValues.Clear();
    }

    // preset2 -> outline, preset3 -> outline + highlight

    private float EqualValueSlider(float value, float objValue)
    {
        if ((value - 0.5f) + objValue <= 1)
        {
            return (value - 0.5f) + objValue;
        }
        else return 1;
        
    }

    public void GreenChannelSliderOnChange()
    {
        Slider _greenSliderComponent = _greenChannelSlider.GetComponent<Slider>();
        float _sliderValue = _greenSliderComponent.value;
        //grab all materials from storedobjects
        foreach (GameObject obj in _storedObjects)
        {
            Renderer _objRenderer = obj?.GetComponent<Renderer>();
            _objRenderer.material.color = new Color(0, EqualValueSlider(_sliderValue, _objRenderer.material.color.g), _objRenderer.material.color.b);
        }
    }

    public void BlueChannelSliderOnChange()
    {
        Slider _blueSliderComponent = _blueChannelSlider?.GetComponent<Slider>();
        float _sliderValue = _blueSliderComponent.value;

        foreach (GameObject obj in _storedObjects)
        {
            Renderer _objRenderer = obj?.GetComponent<Renderer>();
            _objRenderer.material.color = new Color(0, _objRenderer.material.color.g, EqualValueSlider(_sliderValue, _objRenderer.material.color.b));
        }
    }


    public void ResetColorValues()
    {
        _greenChannelSlider.value = 0.5f;
        _blueChannelSlider.value = 0.5f;
        ResetProtanopia();
        SaveObjectColor();
        ApplyProtanopia();
    }

    public void ProtanopiaOnClick()
    {
        if ((int)_activeMode < 3)
        {
            _activeMode++;
        }
        else _activeMode = 0;
     
        switch (_activeMode)
        {
            case _modes.unselected:
                SelectedBorder.SetActive(false);
                UnselectedBorder.SetActive(true);
                FeatureSelectedIcon.SetActive(false);
                InactiveSlot1.SetActive(true);
                InactiveSlot2.SetActive(true);
                InactiveSlot3.SetActive(true);
                ActiveSlot1.SetActive(false);
                ActiveSlot2.SetActive(false);
                ActiveSlot3.SetActive(false);
                CustomPanel.SetActive(false);
                _featureText.text = "Protanopia \n (Red)";
                ResetProtanopia();
                break;
            case _modes.preset1:
                SelectedBorder.SetActive(true);
                UnselectedBorder.SetActive(false);
                FeatureSelectedIcon.SetActive(true);
                ActiveSlot1.SetActive(true);
                ActiveSlot2.SetActive(false);
                ActiveSlot3.SetActive(false);
                InactiveSlot1.SetActive(false);
                InactiveSlot2.SetActive(true);
                InactiveSlot3.SetActive(true);
                CustomPanel.SetActive(true);
                _featureText.text = "Protanopia \n (Preset1)";
                Preset1();
                break;
            case _modes.preset2:
                SelectedBorder.SetActive(true);
                UnselectedBorder.SetActive(false);
                FeatureSelectedIcon.SetActive(true);
                ActiveSlot1.SetActive(false);
                ActiveSlot2.SetActive(true);
                ActiveSlot3.SetActive(false);
                InactiveSlot1.SetActive(true);
                InactiveSlot2.SetActive(false);
                InactiveSlot3.SetActive(true);
                CustomPanel.SetActive(false);
                _featureText.text = "Protanopia \n (Preset2)";
                break;
            case _modes.preset3:
                SelectedBorder.SetActive(true);
                UnselectedBorder.SetActive(false);
                FeatureSelectedIcon.SetActive(true);
                ActiveSlot1.SetActive(false);
                ActiveSlot2.SetActive(false);
                ActiveSlot3.SetActive(true);
                InactiveSlot1.SetActive(true);
                InactiveSlot2.SetActive(true);
                InactiveSlot3.SetActive(false);
                CustomPanel.SetActive(false);
                _featureText.text = "Protanopia \n (Preset3)";
                break;
        }
        
    }

    private void Awake()
    {
        _featureText = FeatureText.GetComponent<TextMeshProUGUI>();
        _featureText.text = "Protanopia \n (Red)";
        _greenChannelSlider = GreenChannelSlider.GetComponent<Slider>();
        _blueChannelSlider = BlueChannelSlider.GetComponent<Slider>();
    }

    void Start()
    {
       
    }

    
    void Update()
    {
        
    }
}
