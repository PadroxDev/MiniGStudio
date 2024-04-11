using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGStudio
{
    public class StartOptionMenuFunctions : MonoBehaviour
    {
        [SerializeField] private Canvas StartMenuCanvas;
        [SerializeField] private Canvas StartOptionsCanvas;
        [SerializeField] private AudioClip ButtonSound;
        [SerializeField] private Character Character;

        private void Awake()
        {
            Character.Controller.onMenu += OnMenu;
        }

        public void MyBackFunction()
        {
            SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 0.05f);
            StartOptionsCanvas.gameObject.SetActive(false);
            StartMenuCanvas.gameObject.SetActive(true);
        }

        private void OnMenu()
        {
            if (StartOptionsCanvas.gameObject.activeSelf == true)
            {
                StartOptionsCanvas.gameObject.SetActive(false);
                StartMenuCanvas.gameObject.SetActive(true);
            }
            
        }
    }
}
