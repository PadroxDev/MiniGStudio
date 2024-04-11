using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGStudio
{
    public class OptionsMenuFunctions : MonoBehaviour
    {
        [SerializeField] private Canvas InGameMenuCanvas;
        [SerializeField] private Canvas OptionsCanvas;
        [SerializeField] private AudioClip ButtonSound;
        [SerializeField] private Character Character;

        private void Awake()
        {
            Character.Controller.onMenu += OnMenu;
        }

        public void MyBackFunction()
        {
            SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 0.05f);
            OptionsCanvas.gameObject.SetActive(false);
            InGameMenuCanvas.gameObject.SetActive(true);
        }

        private void OnMenu()
        {
            if (OptionsCanvas.gameObject.activeSelf)
            {
                OptionsCanvas.gameObject.SetActive(false);
                InGameMenuCanvas.gameObject.SetActive(true);
            }
        }
    }
}
