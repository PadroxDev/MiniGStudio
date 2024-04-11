using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGStudio
{
    public class MenuFunctions : MonoBehaviour
    {
        public void Start()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
        public void MyPlayFunction()
        {
            SceneManager.LoadScene("Boss Fight");
        }

        public void MyOptionFunction()
        {
            SceneManager.LoadScene("Options");
        }

        public void MyQuitFunction()
        {
            Application.Quit();
        }
    }
}
