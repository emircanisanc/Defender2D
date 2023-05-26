using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsUI : MonoBehaviour {

    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;

    private TextMeshProUGUI soundVolumeText;
    private TextMeshProUGUI musicVolumeText;

    public event EventHandler OnPauseToggled;

    void Awake() {
        soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();

        transform.Find("soundIncreaseBtn").GetComponent<Button>().onClick.AddListener(() => {
            soundManager.IncreaseVolume();
            UpdateSoundVolumeText();
        });
        transform.Find("soundDecreaseBtn").GetComponent<Button>().onClick.AddListener(() => {
            soundManager.DecreaseVolume();
            UpdateSoundVolumeText();
        });
        transform.Find("musicIncreaseBtn").GetComponent<Button>().onClick.AddListener(() => {
            musicManager.IncreaseVolume();
            UpdateMusicVolumeText();
        });
        transform.Find("musicDecreaseBtn").GetComponent<Button>().onClick.AddListener(() => {
            musicManager.DecreaseVolume();
            UpdateMusicVolumeText();
        });
        transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() => {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });
        transform.Find("edgeScrollingToggle").GetComponent<Toggle>().onValueChanged.AddListener((bool set) => {
            CameraHandler.Instance.SetEdgeScrolling(set);
        });

    }

    void Start() {
        UpdateSoundVolumeText();
        gameObject.SetActive(false);

        transform.Find("edgeScrollingToggle").GetComponent<Toggle>().SetIsOnWithoutNotify(CameraHandler.Instance.GetEdgeScrolling());
    }

    private void UpdateSoundVolumeText() {
        soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
    }

    private void UpdateMusicVolumeText() {
        musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 10).ToString());
    }

    public void ToggleVisible() {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
        OnPauseToggled?.Invoke(this, EventArgs.Empty);
    }

}
