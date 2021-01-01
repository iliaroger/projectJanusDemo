using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessibility : MonoBehaviour
{

    // plugin active state
    private bool _pluginActive = false;

    public void AccessibilityIconOnClick()
    {
        _pluginActive = !_pluginActive;
    }

    public void ToggleAccessibilityPlugin()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _pluginActive = !_pluginActive;       
        }
    }

    public void PluginCloseButtonOnClick()
    {
        Debug.Log("plugin button on click");
    }
    
    void Start()
    {
        
    }

 
    void Update()
    {
        ToggleAccessibilityPlugin();
    }
}
