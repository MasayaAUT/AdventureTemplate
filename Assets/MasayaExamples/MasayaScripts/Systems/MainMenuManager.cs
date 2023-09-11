using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace MasayaScripts
{
    public class MainMenuManager : MonoBehaviour
    {

        [SerializeField]private string levelName = "SampleScene";

        public void StartGame()
        {
            SceneManager.LoadScene(levelName);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
