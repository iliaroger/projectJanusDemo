using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Threading;

public class ProtanopiaFeature : MonoBehaviour
{

    // ui reference
    public GameObject SelectedBorder;
    public GameObject UnselectedBorder;
    public GameObject FeatureText;
    private TextMeshProUGUI _featureText;
    public GameObject FeatureSelectedIcon;
    public GameObject ActiveSlot1;
    public GameObject InactiveSlot1;
    public GameObject ActiveSlot2;
    public GameObject InactiveSlot2;
    public GameObject ActiveSlot3;
    public GameObject InactiveSlot3;

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

    // protanopia + custom sliders
    private void Preset1()
    {
        // preset 3 
        Foe[] _foeSubjects = FindObjectsOfType(typeof(Foe)) as Foe[];
        foreach (Foe foeSubject in _foeSubjects)
        {
            Renderer _foeRenderer = foeSubject.GetComponent<Renderer>();
            _foeRenderer.material.color = new Color(0, _foeRenderer.material.color.g, _foeRenderer.material.color.b);
        }

        SaveObjectColor();

        //MeshRenderer[] _allObjects = FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];

        //foreach (MeshRenderer gameObject in _allObjects)
        //{
        //    Renderer _shader = gameObject.GetComponent<Renderer>();
        //    Debug.Log(_shader.material.name);
        //}


    }

    private void SaveObjectColor()
    {
        // create a dictionary and save unique instance id and color
        Object[] _allGameObjects = FindObjectsOfType<GameObject>();
        List<Object> _storedObjects = new List<Object>();
        string[] _filterComponents = { "Meter", "StaminaPower", "AutoCrosshair", "[Debug Updater]", "Protanopia", "Deuteranopia", "Tritanopia", "Fonts", "Effects" };
        Stopwatch _calc = new Stopwatch();
        _calc.Start();
        foreach (GameObject obj in _allGameObjects)
        {
            FirstPersonAIO _hasControllerScript = obj?.GetComponent<FirstPersonAIO>();
            MeshRenderer _hasMeshRenderer = obj?.GetComponent<MeshRenderer>();
            if (_hasControllerScript == null)
            {
                if (_hasMeshRenderer != null)
                {
                    bool _foundInstance = false;
                    for (int i = 0; i < _filterComponents.Length; i++)
                    {
                        if (obj.name == _filterComponents[i])
                        {
                            _foundInstance = true;
                        }
                        else _foundInstance = false;
                    }
                    if (_foundInstance)
                    {
                        _storedObjects.Add(obj);
                    }
                }
            }
        }

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
        _calc.Stop();
        
        string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            _calc.Elapsed.Hours, _calc.Elapsed.Minutes, _calc.Elapsed.Seconds,
            _calc.Elapsed.Milliseconds / 10);

        UnityEngine.Debug.Log(elapsedTime);
    }

    private void ResetObjectColor()
    {

    }

    // call method on slider change and adjust slider value to material color


    public void ProtanopiaOnClick()
    {
        if ((int)_activeMode < 3)
        {
            _activeMode++;
        }
        else
        {
            _activeMode = 0;
        }
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
                _featureText.text = "Protanopia \n (Red)";
                ProtanopiaActive = false;
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
                _featureText.text = "Protanopia \n (Preset1)";
                ProtanopiaActive = true;
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
                _featureText.text = "Protanopia \n (Preset2)";
                ProtanopiaActive = true;
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
                _featureText.text = "Protanopia \n (Preset3)";
                ProtanopiaActive = true;
                break;
        }
        
    }

    private void Awake()
    {
        _featureText = FeatureText.GetComponent<TextMeshProUGUI>();
        _featureText.text = "Protanopia \n (Red)";

    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
