using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Button audioButton;
    public Sprite unmutedIcon;
    public Sprite mutedIcon;

    private void Start()
    {
        audioButton.onClick.AddListener(AudioButtonPressed);

    }

    public void AudioButtonPressed()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource.mute == false)
        {
            audioSource.mute = true;
            audioButton.image.sprite = mutedIcon;

        } else
        {

            audioSource.mute = false;
            audioButton.image.sprite = unmutedIcon;
        }

    }
}
