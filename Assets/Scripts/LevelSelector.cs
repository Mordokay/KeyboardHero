using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

    public string storyTitle;
    public string storyContent;

    public void Load() {
        GameObject.FindGameObjectWithTag("DataTraveler").GetComponent<Data>().storyTitle = storyTitle;
        GameObject.FindGameObjectWithTag("DataTraveler").GetComponent<Data>().storyContent = storyContent;
        SceneManager.LoadScene("mainScene");
    }
}
