using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum inflictionMethods { Multiply, Divide, Substract, Add, Highest, Lowest, Average, Nothing};

public class gameScript : MonoBehaviour
{
    [Range(2, 20)]
    public int numberOfPoints = 3;
    public Transform Points;
    public Transform inflictionPoints;
    [SerializeField] private GameObject pointPref;
    [SerializeField] private GameObject inflictionPointPref;
    private List<GameObject> allPoints;
    private List<GameObject> allInflictionPoints;
    private List<List<inflictionMethods>> availableMethods;
    public List<inflictionMethods> currentMethods;
    private Color32[] C;
    public GameObject mainCamera;
    public GameObject infoAboutPoint;
    public GameObject infoAboutInfliction;
    public GameObject mainCanvas;
    public GameObject pauseCanvas;
    private int choosingInflictionNumber = 99999;
    private int loopRepeat = 2;
    private Color32 currentColour;
    private Color32 startColour = new Color32(0, 0, 255, 255);
    private Color32 endColour = new Color32(255, 255, 255, 0);
    private string nl = System.Environment.NewLine;
    private int Volume = 0;
    private int Level = 1;
    private bool[] completedLevels;
    private bool Paused = false;
    public TextMeshProUGUI volumeText;
    public Slider volumeSlider;
    private List<inflictionMethods> anwser;
    public Transform targetColour;
    public SpriteRenderer bG;
    private AudioSource AS;
    public AudioClip[] Audios;
    public Transform winImage;
    private bool winned = false;

    void Start()
    {
        AS = this.GetComponent<AudioSource>();

        completedLevels = new bool[10];
        for (int i = 0; i < completedLevels.Length; i++)
        {
            completedLevels[i] = false;
        }

        allPoints = new List<GameObject>();
        allInflictionPoints = new List<GameObject>();
        availableMethods = new List<List<inflictionMethods>>();
        anwser = new List<inflictionMethods>();
        currentMethods = new List<inflictionMethods>();

        gameData data = saveSystem.loadProgress();
        if (data != null)
        {
            volumeSlider.value = data.Volume;
            Volume = data.Volume;
            completedLevels = new bool[data.completedLevels.Length];
            for (int i = 0; i < data.completedLevels.Length; i++)
            {
                completedLevels[i] = data.completedLevels[i];
            }
            Level = data.Level;
            checkLevel();
        }
        else
        {
            generateLevel();
        }
    }

    private void checkLevel()
    {
        if (Level == 1)
        {
            int n = 2;
            int[] cm = new int[n];
            cm[0] = 6;
            cm[1] = 6;
            Color32[] col = new Color32[n];
            col[0] = new Color32(255, 255, 255, 255);
            col[1] = new Color32(0, 0, 0, 255);
            loadLevel(n, cm, col);
        }
        if (Level == 2)
        {
            int n = 3;
            int[] cm = new int[n];
            cm[0] = 6;
            cm[1] = 6;
            cm[2] = 6;
            Color32[] col = new Color32[n];
            col[0] = new Color32(255, 0, 0, 255);
            col[1] = new Color32(0, 255, 0, 255);
            col[2] = new Color32(255, 255, 0, 255);
            loadLevel(n, cm, col);
        }
        if (Level == 3)
        {
            int n = 4;
            int[] cm = new int[n];
            cm[0] = 6;
            cm[1] = 6;
            cm[2] = 6;
            cm[3] = 6;
            Color32[] col = new Color32[n];
            col[0] = new Color32(255, 255, 255, 255);
            col[1] = new Color32(255, 0, 0, 255);
            col[2] = new Color32(0, 255, 0, 255);
            col[3] = new Color32(0, 0, 255, 255);
            loadLevel(n, cm, col);
        }
        if (Level == 4)
        {
            int n = 4;
            int[] cm = new int[n];
            cm[0] = 6;
            cm[1] = 6;
            cm[2] = 6;
            cm[3] = 6;
            Color32[] col = new Color32[n];
            col[0] = new Color32(1, 1, 1, 255);
            col[1] = new Color32(1, 1, 1, 255);
            col[2] = new Color32(128, 128, 128, 255);
            col[3] = new Color32(254, 254, 254, 254);
            loadLevel(n, cm, col);
        }
        if (Level == 5)
        {
            int n = 3;
            int[] cm = new int[n];
            cm[0] = 6;
            cm[1] = 6;
            cm[2] = 6;
            Color32[] col = new Color32[n];
            col[0] = new Color32(255, 128, 64, 255);
            col[1] = new Color32(255, 128, 64, 255);
            col[2] = new Color32(0, 0, 0, 255);
            loadLevel(n, cm, col);
        }
        if (Level == 11)
        {
            generateLevel();
        }
        
    }

    private void loadLevel(int n, int[] cm, Color32[] colours)
    {
        numberOfPoints = n;

        for (int i = 0; i < numberOfPoints; i++)
        {
            currentMethods.Add((inflictionMethods)cm[i]);
            Debug.Log("current method (number " + (i + 1).ToString() + ") is " + ((inflictionMethods)cm[i]).ToString());
        }

        for (int i = 0; i < numberOfPoints; i++)
        {
            List<inflictionMethods> list = new List<inflictionMethods>();
            availableMethods.Add(list);
        }

        for (int i = 0; i < availableMethods.Count; i++)
        {
            availableMethods[i].Add((inflictionMethods)0);
            availableMethods[i].Add((inflictionMethods)1);
            availableMethods[i].Add((inflictionMethods)2);
            availableMethods[i].Add((inflictionMethods)3);
            availableMethods[i].Add((inflictionMethods)6);
        }

        loopRepeat = 1;

        startColour = colours[0];

        C = new Color32[colours.Length];
        for (int i = 0; i < colours.Length; i++)
        {
            C[i] = colours[i];
        }

        endColour = startColour;

        targetColour.GetChild(1).GetComponent<Image>().color = endColour;
        targetColour.GetChild(2).GetComponent<TextMeshProUGUI>().text = "(" + endColour.r + "R, " + endColour.g + "G, " + endColour.b + "B)";

        generatePoints(numberOfPoints, C);
        generateInflictionPoints(numberOfPoints);
        generateInflictionColours();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !Paused && !winned)
        {
            playSound(0);
            bool info = false;
            bool infliction = false;
            Vector2 pos = new Vector2(mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x, mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(pos, mainCamera.transform.forward);
            if (hit.collider != null)
            {
                for (int i = 0; i < allPoints.Count; i++)
                {
                    if (hit.collider.gameObject == allPoints[i])
                    {
                        info = true;
                        getInfoAboutPoint(mainCamera.GetComponent<Camera>().WorldToScreenPoint(hit.collider.gameObject.transform.position), C[i]);
                        break;
                    }
                }
                for (int i = 0; i < allInflictionPoints.Count; i++)
                {
                    if (hit.collider.gameObject == allInflictionPoints[i] && i != choosingInflictionNumber)
                    {
                        infliction = true;
                        choosingInflictionNumber = i;
                        getInfoAboutInflictionPoint(mainCamera.GetComponent<Camera>().WorldToScreenPoint(hit.collider.gameObject.transform.position));
                        break;
                    }
                }
            }
            if (!info)
            {
                infoAboutPoint.SetActive(false);
            }
            if (!infliction)
            {
                choosingInflictionNumber = 99999;
                infoAboutInfliction.SetActive(false);
            }
        }

        if (Paused)
        {
            Volume = (int)volumeSlider.value;
            volumeText.text = "VOLUME: " + Volume + "%";

            AS.volume = (float)Volume / 100.0f;
        }
    }
    
    private void playSound(int s)
    {
        AS.clip = Audios[s];
        AS.Play();
    }

    private void generatePoints(int n, Color32[] colours)
    {
        for (int i = 0; i < n; i++)
        {
            GameObject P = Instantiate(pointPref);
            P.transform.position = new Vector2(4.3f * Mathf.Cos(360.0f / n * i * Mathf.PI / 180.0f), 4.3f * Mathf.Sin(360.0f / n * i * Mathf.PI / 180.0f));
            P.transform.SetParent(Points, true);
            P.GetComponent<SpriteRenderer>().color = colours[i];
            allPoints.Add(P);
        }
    }

    private void generateInflictionPoints(int n)
    {
        for (int i = 0; i < n; i++)
        {
            GameObject P = Instantiate(inflictionPointPref);
            P.transform.position = new Vector2(4.3f * Mathf.Cos((360.0f / numberOfPoints * i + 360 / (numberOfPoints * 2.0f)) * Mathf.PI / 180.0f), 4.3f * Mathf.Sin((360.0f / numberOfPoints * i + 360 / (numberOfPoints * 2.0f)) * Mathf.PI / 180.0f));
            P.transform.SetParent(inflictionPoints, true);
            P.transform.GetChild(1).GetComponent<TextMeshPro>().text = getSymb((int)currentMethods[i]);
            allInflictionPoints.Add(P);
        }
    }

    private void getInfoAboutPoint(Vector2 screenPoint, Color32 col)
    {
        infoAboutPoint.SetActive(true);
        RectTransform rt = infoAboutPoint.GetComponent<RectTransform>();
        rt.anchoredPosition = screenPoint + new Vector2(150, 75.0f / 2.0f);
        infoAboutPoint.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)col.r).ToString() + "R " + ((int)col.g).ToString() + "G " + ((int)col.b).ToString() + "B";
    }

    private void getInfoAboutInflictionPoint(Vector2 screenPoint)
    {
        infoAboutInfliction.SetActive(true);
        RectTransform rt = infoAboutInfliction.transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform rt2 = infoAboutInfliction.transform.GetChild(1).GetComponent<RectTransform>();
        int methodCount = availableMethods[choosingInflictionNumber].Count;
        rt.sizeDelta = new Vector2(200, methodCount * 50.0f - 10.0f);
        if (screenPoint.y + rt.sizeDelta.y / 2.0f + 40 + rt.sizeDelta.y > Screen.height)
        {
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt2.anchorMin = new Vector2(0, 1);
            rt2.anchorMax = new Vector2(0, 1);
            rt.anchoredPosition = screenPoint + new Vector2(rt.sizeDelta.x / 2.0f + 100, -rt.sizeDelta.y / 2.0f - 40);
            rt2.anchoredPosition = screenPoint + new Vector2(rt.sizeDelta.x / 2.0f + 100, -rt.sizeDelta.y / 2.0f - 40);
        }
        else
        {
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(0, 0);
            rt2.anchorMin = new Vector2(0, 0);
            rt2.anchorMax = new Vector2(0, 0);
            rt.anchoredPosition = screenPoint + new Vector2(rt.sizeDelta.x / 2.0f + 100, rt.sizeDelta.y / 2.0f + 40);
            rt2.anchoredPosition = screenPoint + new Vector2(rt.sizeDelta.x / 2.0f + 100, rt.sizeDelta.y / 2.0f + 40);
        }
        for (int i =0; i < infoAboutInfliction.transform.GetChild(1).childCount; i++)
        {
            infoAboutInfliction.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        float nextPos = infoAboutInfliction.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y / 2.0f - 20.0f;
        for (int i = 0; i < methodCount; i++)
        {
            for (int j = 0; j < availableMethods[choosingInflictionNumber].Count; j++)
            {
                if ((inflictionMethods)i == availableMethods[choosingInflictionNumber][j])
                {
                    //Debug.Log((inflictionMethods)i);
                    infoAboutInfliction.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                    infoAboutInfliction.transform.GetChild(1).GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, nextPos);
                    nextPos -= 50;
                    break;
                }
            }
        }
    }

    public void setInflictionMethod(int n)
    {
        playSound(1);

        currentMethods[choosingInflictionNumber] = (inflictionMethods)n;

        allInflictionPoints[choosingInflictionNumber].transform.GetChild(1).GetComponent<TextMeshPro>().text = getSymb(n);

        generateInflictionColours();

        Debug.Log("Changed infliction method for the infliction: " + choosingInflictionNumber + " to the " + (inflictionMethods)n);
        choosingInflictionNumber = 99999;
    }

    private void generateInflictionColours()
    {
        for (int i = 0; i < allInflictionPoints.Count; i++)
        {
            Color32 col = new Color32(0, 0, 0, 255);
            if (i > 0)
            {
                col = Inflictation(allInflictionPoints[i - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color, C[(i + 1) % numberOfPoints], currentMethods[i]); ;
                allInflictionPoints[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = col;
                if (col.r + col.g + col.b < 380)
                {
                    allInflictionPoints[i].transform.GetChild(1).GetComponent<TextMeshPro>().color = Color.white;
                }
                else
                {
                    allInflictionPoints[i].transform.GetChild(1).GetComponent<TextMeshPro>().color = Color.black;
                }
            }
            else
            {
                col = Inflictation(C[i], C[(i + 1) % numberOfPoints], currentMethods[i]);
                allInflictionPoints[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = col;
                if (col.r + col.g + col.b < 380)
                {
                    allInflictionPoints[i].transform.GetChild(1).GetComponent<TextMeshPro>().color = Color.white;
                }
                else
                {
                    allInflictionPoints[i].transform.GetChild(1).GetComponent<TextMeshPro>().color = Color.black;
                }
            }
        }

        //bG.color = allInflictionPoints[allInflictionPoints.Count - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        Color32 lc = allInflictionPoints[allInflictionPoints.Count - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        if (lc.r == endColour.r && lc.g == endColour.g && lc.b == endColour.b)
        {
            winned = true;

            Debug.Log("YOU WIN");

            winImage.gameObject.SetActive(true);

            if (Level != 11)
            {
                completedLevels[Level - 1] = true;
                Debug.Log("Level number " + (Level - 1).ToString());
                Debug.Log("Level completed " + (completedLevels[Level - 1]).ToString());
                saveSystem.saveProgress(Level, completedLevels, Volume);
            }
        }
    }

    public void startLoop()
    {
        currentColour = C[0];

        for (int j = 0; j < numberOfPoints; j++)
        {
            currentColour = Inflictation(currentColour, C[(j + 1) % C.Length], currentMethods[j]);
            Debug.Log("Current colour = " + currentColour + ", next colour = " + C[(j + 1) % C.Length].ToString() + ", operation with colour number " + (j + 1) + ", inflictation method = " + currentMethods[j]);
        }

        endColour = currentColour;

        targetColour.GetChild(1).GetComponent<Image>().color = endColour;
        targetColour.GetChild(2).GetComponent<TextMeshProUGUI>().text = "(" + endColour.r + "R, " + endColour.g + "G, " + endColour.b + "B)";

        currentColour = startColour;

        for (int i = 0; i < currentMethods.Count; i++)
        {
            currentMethods[i] = (inflictionMethods)2;
        }
    }

    private Color32 Inflictation(Color32 a, Color32 b, inflictionMethods method)
    {
        byte R;
        byte G;
        byte B;
        Color32 col = new Color32(255, 255, 255, 255);

        if (method == (inflictionMethods)0)
        {
            R = (byte)(a.r * b.r % 255);
            G = (byte)(a.g * b.g % 255);
            B = (byte)(a.b * b.b % 255);
            col = new Color32(R, G, B, 255);
        }
        if (method == (inflictionMethods)1)
        {
            R = (byte)Mathf.Round(a.r / Mathf.Clamp(b.r, 1, 255));
            G = (byte)Mathf.Round(a.g / Mathf.Clamp(b.g, 1, 255));
            B = (byte)Mathf.Round(a.b / Mathf.Clamp(b.b, 1, 255));
            col = new Color32(R, G, B, 255);
        }
        if (method == (inflictionMethods)2)
        {
            R = (byte)((a.r - b.r + 255) % 255);
            G = (byte)((a.g - b.g + 255) % 255);
            B = (byte)((a.b - b.b + 255) % 255);
            col = new Color32(R, G, B, 255);
        }
        if (method == (inflictionMethods)3)
        {
            R = (byte)(a.r + b.r % 255);
            G = (byte)(a.g + b.g % 255);
            B = (byte)(a.b + b.b % 255);
            col = new Color32(R, G, B, 255);
        }
        if (method == (inflictionMethods)4)
        {
            if ((a.r + a.g + a.b) > (b.r + b.g + b.b))
            {
                col = a;
            }
            else
            {
                col = b;
            }
        }
        if (method == (inflictionMethods)5)
        {
            if ((a.r + a.g + a.b) > (b.r + b.g + b.b))
            {
                col = b;
            }
            else
            {
                col = a;
            }
        }
        if (method == (inflictionMethods)6)
        {
            R = (byte)Mathf.Round((a.r + b.r) / 2.0f);
            G = (byte)Mathf.Round((a.g + b.g) / 2.0f);
            B = (byte)Mathf.Round((a.b + b.b) / 2.0f);
            col = new Color32(R, G, B, 255);
        }
        if (method == (inflictionMethods)7)
        {
            col = a;
        }

        return col;
    }

    private void generateColours()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            C[i] = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        }
    }

    private string getSymb(int n)
    {
        string str = "";

        if (n == 0)
        {
            str = "X";
        }
        if (n == 1)
        {
            str = "/";
        }
        if (n == 2)
        {
            str = "-";
        }
        if (n == 3)
        {
            str = "+";
        }
        if (n == 4)
        {
            str = "↑";
        }
        if (n == 5)
        {
            str = "↓";
        }
        if (n == 6)
        {
            str = "~";
        }
        if (n == 7)
        {
            str = "...";
        }

        return str;
    }

    private void generateLevel()
    {
        numberOfPoints = Random.Range(3, 10);

        for (int i = 0; i < numberOfPoints; i++)
        {
            List<inflictionMethods> list = new List<inflictionMethods>();
            availableMethods.Add(list);
        }

        for (int i = 0; i < availableMethods.Count; i++)
        {
            availableMethods[i].Add((inflictionMethods)0);
            availableMethods[i].Add((inflictionMethods)1);
            availableMethods[i].Add((inflictionMethods)2);
            availableMethods[i].Add((inflictionMethods)3);
        }

        for (int i = 0; i < numberOfPoints; i++)
        {
            currentMethods.Add((inflictionMethods)Random.Range(0, 3));
        }

        //loopRepeat = Random.Range(1, 32 / (numberOfPoints * 2));
        loopRepeat = 1;

        startColour = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);

        C = new Color32[numberOfPoints];
        generateColours();
        C[0] = startColour;

        startLoop();

        anwser = currentMethods;

        generatePoints(numberOfPoints, C);
        generateInflictionPoints(numberOfPoints);
        generateInflictionColours();

        Debug.Log("Start colour: " + startColour);
        Debug.Log("End colour: " + endColour);
        Debug.Log("amount of repeats: " + loopRepeat);
    }

    public void closeGame()
    {
        playSound(0);
        Application.Quit();
    }

    public void goToMainMenu()
    {
        playSound(0);
        saveSystem.saveProgress(Level, completedLevels, Volume);
        SceneManager.LoadScene("mainMenu");
    }

    private void OnApplicationQuit()
    {
        saveSystem.saveProgress(Level, completedLevels, Volume);
    }

    public void Pause(bool p)
    {
        if (!winned)
        {
            playSound(0);
            if (p)
            {
                Paused = true;
                pauseCanvas.SetActive(true);
                mainCanvas.SetActive(false);
            }
            else
            {
                Paused = false;
                pauseCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                saveSystem.saveProgress(Level, completedLevels, Volume);
            }
        }
    }
}
