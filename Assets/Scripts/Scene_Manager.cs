using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class Scene_Manager : MonoBehaviour
{
    [System.Serializable]
    public class SceneNames
    {
        public string MAIN_MENU;
        public string GAME;
        public string SETTINGS;
    }

    private EventInstance Menumusic;
    [SerializeField] private EventReference Menumusic_MS;

    private EventInstance Hovers;
    [SerializeField] private EventReference Hover_sfx;

    [SerializeField]
    private SceneNames sceneNames;

    private void Start()
    {
        Menumusic = Audiomanager.instance.PlaySound(Menumusic_MS, transform.position);
    }

    public void Hoversfxtrig()
    {
        Hovers = Audiomanager.instance.PlaySound(Hover_sfx, transform.position);
    }

    public void Gamescene()
    {
        Audiomanager.instance.StopSound(Menumusic);
        SceneManager.LoadScene(sceneNames.GAME);
    }

    public void Settings()
    {
        SceneManager.LoadScene(sceneNames.SETTINGS);
    }

    public void MainMenu()
    {
        Audiomanager.instance.StopSound(Menumusic);
        SceneManager.LoadScene(sceneNames.MAIN_MENU);
    }
    public void Exitgame()
    {
        Application.Quit();
    }
}