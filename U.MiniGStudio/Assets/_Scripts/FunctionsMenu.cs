using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopDownController
{
    public class FunctionsMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        public void MyPlayGame()
        {
            SceneManager.LoadScene(1);
        }

        // Update is called once per frame
        public void MyQuitGame()
        {
            Application.Quit();
        }
    }
}
