using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;

public class processText : MonoBehaviour {

    string[] linesFromfile;
    public bool generateNewLine;

    GameObject ribbon;

    public List<myLine> allLines = new List<myLine>();

    public List<GameObject> allLetters = new List<GameObject>();
    public List<GameObject> allPapers = new List<GameObject>();

    int currentLine;
    GameManager gm;

    public GameObject EndOfGameMassage;
    public Text correctKeysText;
    public Text incorrectKeysText;
    public Text time;

    public GameObject mainMenu;

    public class myLine {
        public int letterCount;
        public int letterToCheckPos;

        public List<GameObject> letters = new List<GameObject>();
    }

    public AudioSource keySound;
    public AudioSource newLineSound;

    public void loadGameResults()
    {
        EndOfGameMassage.SetActive(true);
        correctKeysText.text = "Correct Keys: " + gm.letterMatch;
        incorrectKeysText.text = "Incorrect Keys: " + gm.letterMiss;
        time.text = "Time: " + Time.timeSinceLevelLoad;
        Time.timeScale = 0;
    }

    void Start() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        generateNewLine = true;
        currentLine = 0;

        linesFromfile = GameObject.FindGameObjectWithTag("DataTraveler").GetComponent<Data>().storyContent.Split('\n');

        ribbon = Instantiate( Resources.Load("RibbonVibrator"), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;

        for (int i = 0; i < (linesFromfile.Length / 17) + 1; i++)
        {
            GameObject paperAux = Instantiate(Resources.Load("paperBackround"), new Vector3(0.0f, (i * (-38)) - 17, 0.0f), Quaternion.identity) as GameObject;
            allPapers.Add(paperAux);
        }

        for (int lineNumber = 0;  lineNumber < linesFromfile.Length; lineNumber++)
        {
            myLine line = new myLine();
            line.letterToCheckPos = 0;
            line.letterCount = linesFromfile[lineNumber].Length;

            int halfSize = linesFromfile[lineNumber].Length / 2;

            for (int i = 0; i < linesFromfile[lineNumber].Length; i++)
            {
                GameObject letterAux = Instantiate(Resources.Load("Letter"), new Vector3(-(halfSize / 2) + i * 0.5f, -lineNumber * 2 - ((lineNumber / 17) * 4.0f), 0.0f), Quaternion.identity) as GameObject;
                letterAux.GetComponent<TextMesh>().text = linesFromfile[lineNumber][i].ToString();
                line.letters.Add(letterAux);
                allLetters.Add(letterAux);
            }
            allLines.Add(line);
        }

        ribbon.transform.position = allLines[0].letters[0].transform.position;
    }

    public void ToggleMenu() {
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else {
            mainMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
            Debug.Log("menu!!!");
        }

        if (currentLine < allLines.Count)
        {
            if (Input.inputString != "")
            {
                if (allLines[currentLine].letterCount > 0)
                {
                    if (allLines[currentLine].letters[allLines[currentLine].letterToCheckPos].GetComponent<TextMesh>().text == Input.inputString)
                    {
                        allLines[currentLine].letters[allLines[currentLine].letterToCheckPos].GetComponent<TextMesh>().color = new Color(220, 0, 0);

                        keySound.Play(); ;

                        allLines[currentLine].letterToCheckPos += 1;
                        allLines[currentLine].letterCount -= 1;

                        gm.letterMatch += 1;
                    }
                    else
                    {
                        gm.letterMiss += 1;
                    }
                }
            }

            if (allLines[currentLine].letterCount > 0)
            {
                ribbon.transform.position = allLines[currentLine].letters[allLines[currentLine].letterToCheckPos].transform.position + Vector3.right * -0.06f;
            }
            else
            {
                newLineSound.Play();
                currentLine += 1;
                if(currentLine < allLines.Count-1)
                    ribbon.transform.position = allLines[currentLine].letters[allLines[currentLine].letterToCheckPos].transform.position;

                if (currentLine % 17 == 0)
                {
                    foreach (GameObject letter in allLetters)
                    {
                        letter.GetComponent<letterManager>().moveToNewPage();
                    }
                    foreach (GameObject paper in allPapers)
                    {
                        paper.GetComponent<paperManager>().moveToNewPage();
                    }
                }
                else
                {
                    foreach (GameObject letter in allLetters)
                    {
                        letter.GetComponent<letterManager>().moveUp();
                    }
                    foreach (GameObject paper in allPapers)
                    {
                        paper.GetComponent<paperManager>().moveUp();
                    }
                }
            }
        }
        else
        {
            loadGameResults();
        }
    }
}
