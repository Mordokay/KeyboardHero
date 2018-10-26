using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int letterMatch;
    public int letterMiss;

    void Start () {
        letterMatch = 0;
        letterMiss = 0;
    }

    public void Restart() {
        SceneManager.LoadScene("mainScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("mainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
