using UnityEngine;
using UnityEngine.UI;

public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private int windowedWidth = 1280;
    [SerializeField] private int windowedHeight = 720;

    private void Start()
    {
        bool isFull = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.isOn = isFull;
            fullscreenToggle.onValueChanged.AddListener(ChangeFullscreen);
        }
        ApplyFullscreen(isFull);
    }

    public void ChangeFullscreen(bool isOn)
    {
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt("fullscreen", isOn ? 1 : 0);
    }

    private void ApplyFullscreen(bool isFull)
    {
        if (isFull)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.SetResolution(windowedWidth, windowedHeight, false);
        }
    }

    private void OnDestroy()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.RemoveListener(ChangeFullscreen);
    }
}

