using System.Collections;
using System.Collections.Generic;
using MiniGStudio;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerController;
using UnityEngine.TextCore.Text;

namespace MiniGStudio
{
    public class InGameMenuFunctions : MonoBehaviour
    {
        [SerializeField] private Canvas InGameMenuCanvas;
        [SerializeField] private Canvas OptionsCanvas;
        [SerializeField] private Character Character;
        [SerializeField] private AudioClip ButtonSound;
        [SerializeField] private AudioClip MenuSound;

        private void Awake()
        {
            Character.Controller.onMenu += OnMenu;
        }

        public void MyResumeFunction()
        {
            SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 0.05f);
            InGameMenuCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void MyOptionFunction()
        {
            SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 0.05f);
            InGameMenuCanvas.gameObject.SetActive(false);
            OptionsCanvas.gameObject.SetActive(true);
        }

        public void MyQuitFunction()
        {
            Application.Quit();
        }

        private void OnMenu()
        {
            if (InGameMenuCanvas.gameObject.activeSelf) return; 
            InGameMenuCanvas.gameObject.SetActive(true);
            if (OptionsCanvas.gameObject.activeSelf) return;
            SoundFXManager.instance.PlaySoundFXClip(MenuSound, transform, 0.05f);
            Time.timeScale = 0;
        }
    }
}
