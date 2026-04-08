using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    [System.Serializable]
    public class SceneNames
    {
        public string MAIN_MENU;
        public string GAME;
        public string SETTINGS;
    }

    [SerializeField]
    private SceneNames sceneNames;

    public void Gamescene()
    {
        SceneManager.LoadScene(sceneNames.GAME);
    }

    public void Settings()
    {
        SceneManager.LoadScene(sceneNames.SETTINGS);
    }

    public void Exitgame()
    {
        Application.Quit();
    }
}