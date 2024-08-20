using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager instance;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider VoicesSlider;
    [SerializeField] private TextMeshProUGUI MasterVolumeTxt;
    [SerializeField] private TextMeshProUGUI BGMVolumeTxt;
    [SerializeField] private TextMeshProUGUI SFXVolumeTxt;
    [SerializeField] private TextMeshProUGUI VoicesVolumeTxt;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Load();
        SetMasterVolume();
        SetSFXVolume();
        SetBGMVolume();
        SetVoicesVolume();
    }
    public void SetMasterVolume()
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(MasterSlider.value) * 20);
        MasterVolumeTxt.text = Mathf.RoundToInt((MasterSlider.value / 1 * 100)).ToString();
    }

    public void SetBGMVolume()
    {
        mixer.SetFloat("BGMVolume", Mathf.Log10(BGMSlider.value) * 20 * MasterSlider.value);
        BGMVolumeTxt.text = Mathf.RoundToInt((BGMSlider.value / 1 * 100)).ToString();
    }

    public void SetSFXVolume()
    {
        mixer.SetFloat("SFXVolume",Mathf.Log10(SFXSlider.value) * 20 * MasterSlider.value);
        SFXVolumeTxt.text = Mathf.RoundToInt((SFXSlider.value / 1 * 100)).ToString();
    }
    public void SetVoicesVolume()
    {
        mixer.SetFloat("VoicesVolume", Mathf.Log10(VoicesSlider.value) * 20 * MasterSlider.value);
        VoicesVolumeTxt.text = Mathf.RoundToInt((VoicesSlider.value / 1 * 100)).ToString();
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void TurnOnOffAudioMenu()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void SaveMaster()
    {
        PlayerPrefs.SetFloat("MasterVolume",MasterSlider.value);
    }

    public void SaveSFX()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

    public void SaveBGM()
    {
        PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
    }

    public void SaveVoices()
    {
        PlayerPrefs.SetFloat("VoicesVolume", VoicesSlider.value);
    }

    public float GetMaster()
    {
        return PlayerPrefs.GetFloat("MasterVolume");
    }

    public float GetSFX()
    {
        return PlayerPrefs.GetFloat("SFXVolume") * PlayerPrefs.GetFloat("MasterVolume");
    }

    public float GetBGM()
    {
        return PlayerPrefs.GetFloat("BGMVolume") * PlayerPrefs.GetFloat("MasterVolume");
    }

    public float GetVoices()
    {
        return PlayerPrefs.GetFloat("VoicesVolume") * PlayerPrefs.GetFloat("MasterVolume");
    }

    public void Load()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume");

        VoicesSlider.value = PlayerPrefs.GetFloat("VoicesVolume");
    }
}

