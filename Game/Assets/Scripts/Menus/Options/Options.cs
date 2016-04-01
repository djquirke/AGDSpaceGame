using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Options : MonoBehaviour {


    private List<Resolution> AvaliableResolutions = new List<Resolution>();
    private int Resolutionidx = 0;

    public Slider BrightnessSlider = null;

    public Text ResolutionText = null;
    public Text GraphicsText = null;


	// Use this for initialization
	void Start () {
        AvaliableResolutions.AddRange(Screen.resolutions);

        foreach (var item1 in AvaliableResolutions)
        {
            foreach (var item2 in AvaliableResolutions)
            {
                if(item1.height == item2.height &&
                    item1.width == item2.width &&
                    item1.refreshRate != item2.refreshRate)
                {
                    if(item1.refreshRate > item2.refreshRate)
                    {
                        AvaliableResolutions.Remove(item2);
                    }
                    else
                    {
                        AvaliableResolutions.Remove(item1);
                        break;
                    }

                }
            }
        }
        for(;Resolutionidx < AvaliableResolutions.Count; ++Resolutionidx)
        {
            if(AvaliableResolutions[Resolutionidx].width == Screen.currentResolution.width &&
                AvaliableResolutions[Resolutionidx].height == Screen.currentResolution.height)
            {
                break;
            }
        }

        ResolutionText.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;

        GraphicsText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        BrightnessSlider.value = RenderSettings.ambientLight.r;
	}

    public void ResolutionUp()
    {
        if(Resolutionidx+1 == AvaliableResolutions.Count)
        {
            return;
        }
        ++Resolutionidx;

        Screen.SetResolution(AvaliableResolutions[Resolutionidx].width,AvaliableResolutions[Resolutionidx].height,Screen.fullScreen);
        
        
        ResolutionText.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
    }
    public void ResolutionDown()
    {
        if (Resolutionidx == 0)
        {
            return;
        }
        --Resolutionidx;

        Screen.SetResolution(AvaliableResolutions[Resolutionidx].width, AvaliableResolutions[Resolutionidx].height, Screen.fullScreen);

        

        ResolutionText.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
    }

    public void SetWindowMode(bool Windowed)
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Windowed);
    }
    public void setBrightness()
    {
        float value = BrightnessSlider.value;
        RenderSettings.ambientLight = new Color(value, value, value, 1.0f);
    }
    public void SetVSync(bool VSync)
    {
        QualitySettings.vSyncCount = (VSync) ? 1 : 0;
    }
    public void GraphicsUp()
    {
        QualitySettings.IncreaseLevel();
        GraphicsText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void GraphicsDown()
    {
        QualitySettings.DecreaseLevel();
        GraphicsText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        
    }
}
