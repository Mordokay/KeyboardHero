using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    GameManager gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

    public void Restart() {
        SceneManager.LoadScene(0);
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
