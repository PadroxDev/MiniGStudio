using System.Collections;
using System.Collections.Generic;
using MiniGStudio;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerController;
using UnityEngine.TextCore.Text;

namespace MiniGStudio
{
    public class StartMenuFunctions : MonoBehaviour
    {
        [SerializeField] private Canvas StartMenuCanvas;
        [SerializeField] private Canvas StartOptionsCanvas;
        [SerializeField] private Character Character;
        [SerializeField] private AudioSource MusicManager;
        [SerializeField] private AudioClip ButtonSound;
        [SerializeField] private AudioClip MenuSound;
        [SerializeField] private AudioClip MainMusic;

        public void Awake()
        {
            Time.timeScale = 0f;
        }

        public void MyPlayFunction()
        {
            Character.Controller.gameObject.SetActive(true);
            SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 0.05f);
            MusicManager.Stop();
            MusicManager.clip = MainMusic;
            MusicManager.Play();
            StartMenuCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void MyOptionFunction()
        {
            SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 0.05f);
            StartMenuCanvas.gameObject.SetActive(false);
            StartOptionsCanvas.gameObject.SetActive(true);
        }

        public void MyQuitFunction()
        {
            Application.Quit();
        }
    }
}
