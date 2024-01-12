using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayScript : MonoBehaviour
{

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    private const string FullscreenKey = "Fullscreen";
    private const string ResolutionIndexKey = "ResolutionIndex";

    private void Start()
    {

        fullscreenToggle.isOn = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;


        resolutions = Screen.resolutions;


        if (!PlayerPrefs.HasKey(ResolutionIndexKey))
        {
            SetDefaultResolution();
        }

        
        PopulateResolutionsDropdown();

        
        int savedResolutionIndex = PlayerPrefs.GetInt(ResolutionIndexKey, 0);
        resolutionDropdown.value = savedResolutionIndex;
        OnResolutionDropdownChanged(); 
    }

    // Function to toggle fullscreen mode
    public void ToggleFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt(FullscreenKey, fullscreenToggle.isOn ? 1 : 0);
    }


    private void PopulateResolutionsDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        foreach (Resolution resolution in resolutions)
        {
            string option = $"{resolution.width} x {resolution.height} {resolution.refreshRate}Hz";
            resolutionOptions.Add(option);
        }

        resolutionDropdown.AddOptions(resolutionOptions);
    }


    // Function to set the default resolution to the highest available resolution
    private void SetDefaultResolution()
    {
  
        Screen.SetResolution(800, 600, Screen.fullScreen);

   
        if (resolutions.Length > 0)
        {
            Resolution defaultResolution = resolutions[resolutions.Length - 1];
            Screen.SetResolution(defaultResolution.width, defaultResolution.height, Screen.fullScreen);

            // Log the default resolution
            Debug.Log($"Default resolution set to: {defaultResolution.width} x {defaultResolution.height} {defaultResolution.refreshRate}Hz");

        }
        PlayerPrefs.SetInt(ResolutionIndexKey, resolutions.Length - 1);
        PlayerPrefs.Save();
    }

    // Function to handle resolution dropdown value change
    public void OnResolutionDropdownChanged()
    {
        int selectedResolutionIndex = resolutionDropdown.value;

        if (selectedResolutionIndex >= 0 && selectedResolutionIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[selectedResolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

            
            PlayerPrefs.SetInt(ResolutionIndexKey, selectedResolutionIndex);
            PlayerPrefs.Save();

       
            Debug.Log($"Resolution changed to: {selectedResolution.width} x {selectedResolution.height} {selectedResolution.refreshRate}Hz");
        }
        else
        {
            Debug.LogError("Invalid resolution index selected!");
        }
    }
}
