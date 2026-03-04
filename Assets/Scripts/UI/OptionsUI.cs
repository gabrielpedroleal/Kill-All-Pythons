using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;

    [SerializeField] private Button backButton;
    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider EnvironmentVolumeSlider;


    private void Start()
    {
        backButton.onClick.AddListener(ClosePanel);
        MasterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);
        SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChanged);
        EnvironmentVolumeSlider.onValueChanged.AddListener(OnEnvironmentVolumeSliderChanged);

        MasterVolumeSlider.SetValueWithoutNotify(GameManager.Instance.AudioManager.GetMixerVolume(MixerGroup.Master));
        SFXVolumeSlider.SetValueWithoutNotify(GameManager.Instance.AudioManager.GetMixerVolume(MixerGroup.SFX));
        EnvironmentVolumeSlider.SetValueWithoutNotify(GameManager.Instance.AudioManager.GetMixerVolume(MixerGroup.Environment));
    }

    private void ClosePanel()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.ButtonClick);
        this.gameObject.SetActive(false);

        if (mainMenuUI != null && mainMenuUI.gameObject.activeInHierarchy)
        {
            mainMenuUI.CloseOptionsMenu();
        }
        else 
        {
            GameManager.Instance.UIManager.CloseOptionPanel();
        }
    }

    private void OnMasterVolumeSliderChanged(float value)
    {
        GameManager.Instance.AudioManager.SetMixerVolume(MixerGroup.Master, value);
    }

    private void OnSFXVolumeSliderChanged(float value)
    {
        GameManager.Instance.AudioManager.SetMixerVolume(MixerGroup.SFX, value);
    }

    private void OnEnvironmentVolumeSliderChanged(float value)
    {
        GameManager.Instance.AudioManager.SetMixerVolume(MixerGroup.Environment, value);
    }
}
