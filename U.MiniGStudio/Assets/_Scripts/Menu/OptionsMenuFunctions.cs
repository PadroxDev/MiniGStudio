using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGStudio
{
    public class OptionsMenuFunctions : MonoBehaviour
    {
        public void MyVolumeFunction(float Volume)
        {
            PlayerPrefs.SetFloat("volume", Volume);
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }

        public void MyReturnFunction()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
