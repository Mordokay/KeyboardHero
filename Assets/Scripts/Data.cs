using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    public string storyTitle;
    public string storyContent;

    private static Data dataInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);
//
        if (dataInstance == null)
        {
            dataInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
