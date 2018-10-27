using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject levelSelector;
    public GameObject levelMaker;

    public GameObject[] allLevelButtons;

    bool choosingLevel;
    public int pageLevelNumber;

    bool makingLevel;

    public InputField titleLevelMaker;
    public InputField contentLevelMaker;
    public InputField passwordBox;
    float timeSincePasswordError;
    public GameObject passwordErrorBox;

    public GameObject previousPage;
    public GameObject nextPage;

    string addStory = "http://mordokay.com/webgl/Games/KeyboardHero/addStory.php";
    string getStories = "http://mordokay.com/webgl/Games/KeyboardHero/GetStories.php";

    public class Story
    {
        public string StoryTitle { get; set; }
        public string StoryContent { get; set; }
        public Story(string title, string content)
        {
            StoryTitle = title;
            StoryContent = content;
        }
    }

    List<Story> myStories = new List<Story>();

    public void Quit() {
        Application.Quit();
    }

    public void hideAllLevelSelectorButtons() {
        foreach (GameObject button in allLevelButtons) {
            button.SetActive(false);
        }
    }

    public void makeLevel() {
        mainMenu.SetActive(false);
        levelSelector.SetActive(false);
        levelMaker.SetActive(true);
        hideAllLevelSelectorButtons();
    }

    public void archiveText() {
        if (passwordBox.text.Equals("foquinha44"))
        {
            string text = contentLevelMaker.text;
            string result = "";
            string[] lines = text.Split('\n');
            foreach(string l in lines)
            {
                if(l.Length > 50)
                {
                    string collective = "";
                    string[] words = l.Split(' ');
                    foreach(string w in words)
                    {
                        if(w.Length > 50)
                        {
                            return;
                        }
                        else if(collective.Length + w.Length > 50)
                        {
                            if(collective.EndsWith(" "))
                            {
                                collective = collective.Substring(0, collective.Length - 1);
                            }
                            //Debug.Log("Add a new line here : " + collective);
                            result += collective + System.Environment.NewLine;
                            collective = w + " ";
                        }
                        else
                        {
                            collective += w + " ";
                        }
                    }
                    if (collective.EndsWith(" "))
                    {
                        collective = collective.Substring(0, collective.Length - 1);
                    }
                    result += collective + System.Environment.NewLine;
                }
                else
                {
                    result += l + System.Environment.NewLine;
                }
            }

            //Debug.Log(result);
            StartCoroutine(AddStoryEnumerator(titleLevelMaker.text, result.Trim()));
            menu();
            titleLevelMaker.text = "";
            contentLevelMaker.text = "";
            passwordBox.text = "";
        }
        else
        {
            passwordErrorBox.SetActive(true);
            timeSincePasswordError = 1.0f;
        }
    }

    public IEnumerator AddStoryEnumerator(string title, string story)
    {
        string post_url = addStory + "?title=" + WWW.EscapeURL(title) + "&story=" + WWW.EscapeURL(story);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;

        StartCoroutine(GetAllStoriesEnumerator());
        //Debug.Log(hs_post.text);
    }

    public IEnumerator GetAllStoriesEnumerator()
    {
        myStories.Clear();
        WWW hs_post = new WWW(getStories);
        yield return hs_post;

        string result = hs_post.text;
        string[] stories = result.Split(new[] { "||||" }, System.StringSplitOptions.None);

        foreach(string s in stories)
        {
            if (s != "")
            {
                string[] data = s.Split(new[] { "&&&&" }, System.StringSplitOptions.None);
                myStories.Add(new Story(data[0], data[1]));
            }
        }
        //Debug.Log(hs_post.text);
    }
    public void menu()
    {
        mainMenu.SetActive(true);
        levelSelector.SetActive(false);
        levelMaker.SetActive(false);
        hideAllLevelSelectorButtons();
        pageLevelNumber = 0;
    }

    public void selectlevel() {

        previousPage.SetActive(false);

        if (myStories.Count > 12)
        {
            nextPage.SetActive(true);
        }
        else
        {
            nextPage.SetActive(false);
        }

        hideAllLevelSelectorButtons();
        mainMenu.SetActive(false);
        levelSelector.SetActive(true);
        choosingLevel = true;

        for(int i = 0; i < 12 && i < myStories.Count; i++)
        {
            allLevelButtons[i].SetActive(true);
            allLevelButtons[i].GetComponent<LevelSelector>().storyTitle = myStories[12 * pageLevelNumber + i].StoryTitle;
            allLevelButtons[i].GetComponent<LevelSelector>().storyContent = myStories[12 * pageLevelNumber + i].StoryContent;
            allLevelButtons[i].GetComponentInChildren<Text>().text = myStories[12 * pageLevelNumber + i].StoryTitle;
        }   
    }

    public void NextPage()
    {
        hideAllLevelSelectorButtons();
        if (myStories.Count > (pageLevelNumber + 1) * 12)
        {
            pageLevelNumber += 1;
            for (int i = 0; i < 12 && i < myStories.Count - pageLevelNumber * 12; i++)
            {
                allLevelButtons[i].SetActive(true);
                allLevelButtons[i].GetComponent<LevelSelector>().storyTitle = myStories[12 * pageLevelNumber + i].StoryTitle;
                allLevelButtons[i].GetComponent<LevelSelector>().storyContent = myStories[12 * pageLevelNumber + i].StoryContent;
                allLevelButtons[i].GetComponentInChildren<Text>().text = myStories[12 * pageLevelNumber + i].StoryTitle;
            }
        }
        if (myStories.Count < (pageLevelNumber+1) * 12)
        {
            nextPage.SetActive(false);
            previousPage.SetActive(true);
        }

    }

    public void PreviousPage()
    {
        pageLevelNumber -= 1;
        hideAllLevelSelectorButtons();
        if (pageLevelNumber == 0)
        {
            previousPage.SetActive(false);
        }

        for (int i = 0; i < 12 && i < myStories.Count - pageLevelNumber * 12; i++)
        {
            allLevelButtons[i].SetActive(true);
            allLevelButtons[i].GetComponent<LevelSelector>().storyTitle = myStories[12 * pageLevelNumber + i].StoryTitle;
            allLevelButtons[i].GetComponent<LevelSelector>().storyContent = myStories[12 * pageLevelNumber + i].StoryContent;
            allLevelButtons[i].GetComponentInChildren<Text>().text = myStories[12 * pageLevelNumber + i].StoryTitle;
        }
        nextPage.SetActive(true);
    }

    void Start () {
        StartCoroutine(GetAllStoriesEnumerator());
        pageLevelNumber = 0;
        choosingLevel = false;
    }
    private void Update()
    {
        if(timeSincePasswordError > 0)
        {
            timeSincePasswordError -= Time.deltaTime;
        }
        else if(passwordErrorBox.activeSelf)
        {
            passwordErrorBox.SetActive(false);
            timeSincePasswordError = 0.0f;
        }
    }
}
