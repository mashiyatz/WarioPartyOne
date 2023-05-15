using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public enum GameState { START, READY, PLAY, WIN, END }
    public GameState currentState;
    public int pointsToWin;

    public GameObject startPanel;
    public GameObject startText;
    public GameObject winPanel;
    public GameObject[] winText;
    public GameObject celebUI;
    public GameObject papaUI;

    public GameObject celebPrefab;
    public GameObject paparazziPrefab;

    private PlayerManager celeb;
    private PlayerManager paparazzi;
    private ActionButton celebAction;
    private ActionButton paparazziAction;

    private float winDisplayTime;
    private float resetCount = 0;

    // graphic ui
    public Transform stars;
    public Transform hearts;
    public Transform battery;
    public Image lens;
    public Image disguise;
    public Image celebFrame;
    public Image paparazziFrame;

    public GameObject[] winImage;

    public Color[] paparazziPalette;
    public Color[] celebPalette;

    public GameObject objectGenerator;
    public GameObject carGenerator;

    public Tilemap tilemapPath;
    public Tilemap obstaclePath;
    public Image flashPanel;

    public Image photo;
    public SpriteRenderer photograph;

    public Sprite[] photoSprites;
    public GoalManager goalManager;

    //
    public bool paparazziIsReady = false;
    public bool celebrityIsReady = false;
    public TextMeshProUGUI countdownTextbox;

    [SerializeField] private Vector3Int papaStartPos; 
    [SerializeField] private Vector3Int celebStartPos;

    private float countDownStart;

    void Start()
    {
        InstantiatePlayers();

        celeb.enabled = false;
        paparazzi.enabled = false;
        celebAction.enabled = false;
        paparazziAction.enabled = false;

        startPanel.SetActive(true);
        startText.SetActive(true);
        winText[0].SetActive(false);
        winText[1].SetActive(false);
        celebUI.SetActive(false);
        papaUI.SetActive(false);
        objectGenerator.SetActive(false);
        carGenerator.SetActive(false);

        currentState = GameState.START;
    }

    void InstantiatePlayers()
    {
        if (celeb != null) Destroy(celeb.gameObject);
        if (paparazzi != null) Destroy(paparazzi.gameObject);

        Vector3 papaPos = tilemapPath.GetCellCenterWorld(papaStartPos); 
        Vector3 celebPos = tilemapPath.GetCellCenterWorld(celebStartPos); 

        paparazzi = Instantiate(paparazziPrefab, papaPos, Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<PlayerManager>();
        celeb = Instantiate(celebPrefab, celebPos, Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<PlayerManager>();
        paparazziAction = paparazzi.GetComponentInChildren<ActionButton>();
        celebAction = celeb.GetComponentInChildren<ActionButton>();

        paparazzi.SetPathTilemap(tilemapPath);
        celeb.SetPathTilemap(tilemapPath);
        paparazzi.SetObstacleTilemap(obstaclePath);
        celeb.SetObstacleTilemap(obstaclePath);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(RestartGame());
        }

        if (currentState == GameState.START)
        {   
            if (Input.GetKeyDown(KeyCode.U))
            {
                // GameObject.Find("StartPaparazzi").GetComponent<TextMeshProUGUI>().text = "Ready";
                GameObject.Find("StartPaparazzi").SetActive(false);
                paparazziIsReady = true;
            } else if (Input.GetKeyDown(KeyCode.O)) {
                // GameObject.Find("StartPaparazzi").GetComponent<TextMeshProUGUI>().text = "Press to start";
                GameObject.Find("StartPaparazzi").SetActive(true);
                paparazziIsReady = false;
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                // GameObject.Find("StartCelebrity").GetComponent<TextMeshProUGUI>().text = "Ready";
                GameObject.Find("StartCelebrity").SetActive(false);
                celebrityIsReady = true;
            } else if (Input.GetKeyDown(KeyCode.Q))
            {
                // GameObject.Find("StartCelebrity").GetComponent<TextMeshProUGUI>().text = "Press to start";
                GameObject.Find("StartCelebrity").SetActive(true);
                celebrityIsReady = false;
            }



            if(paparazziIsReady && celebrityIsReady)
            {
                countDownStart = Time.time;
                currentState = GameState.READY;
            }
        }
        else if (currentState == GameState.READY)
        {
            // countdownTextbox.text = (3 - Mathf.Abs(countDownStart - Time.time)).ToString("F0");
            if (Time.time - countDownStart >= 1.0f)
            {
                // countdownTextbox.text = "";
                startPanel.SetActive(false);
                startText.SetActive(false);
                celebUI.SetActive(true);
                papaUI.SetActive(true);


                celeb.enabled = true;
                paparazzi.enabled = true;
                celebAction.enabled = true;
                paparazziAction.enabled = true;
                objectGenerator.SetActive(true);
                carGenerator.SetActive(true);

                currentState = GameState.PLAY;
            }
        }
        else if (currentState == GameState.PLAY)
        {
            UpdateUI();

            if (Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
            {
                resetCount += Time.deltaTime;
                if (resetCount > 3)
                {
                    currentState = GameState.END;
                    StartCoroutine(RestartGame());
                }
            } else
            {
                resetCount = 0;
            }

            if (celeb.score == pointsToWin || paparazzi.score == pointsToWin)
            {
                celeb.enabled = false;
                paparazzi.enabled = false;
                celebAction.enabled = false;
                paparazziAction.enabled = false;
                objectGenerator.SetActive(false);
                winPanel.SetActive(true);

                if (celeb.score > paparazzi.score)
                {
                    winImage[0].SetActive(true);
                    winText[0].SetActive(true);
                } else
                {
                    winImage[1].SetActive(true);
                    winText[1].SetActive(true);
                }

                winDisplayTime = Time.time;
                currentState = GameState.WIN;
            }

        }
        else if (currentState == GameState.WIN)
        {
            if (Time.time - winDisplayTime > 3)
            {
                currentState = GameState.END;
                StartCoroutine(RestartGame());
            }
        } else if (currentState == GameState.END)
        {
            ;
        }

    }

    void UpdateUI()
    {
        UpdateBattery();
        UpdateHearts();
        UpdateStars();
        UpdatePowerUpUI();
        UpdateSpeedUI();
    }

    void UpdateSpeedUI()
    {
        if (paparazzi.movementSpeed > 2)
        {
            paparazziFrame.color = Color.Lerp(
                paparazziPalette[0],
                paparazziPalette[1],
                Mathf.Abs(Mathf.Sin(Time.time * 10)));
        } else if (paparazzi.movementSpeed == 2)
        {
            paparazziFrame.color = paparazziPalette[0];
        }

        if (celeb.movementSpeed > 2)
        {
            celebFrame.color = Color.Lerp(
                celebPalette[0],
                celebPalette[1],
                Mathf.Abs(Mathf.Sin(Time.time * 10)));
        } else if (celeb.movementSpeed == 2)
        {
            celebFrame.color = celebPalette[0];
        }
    }

    void UpdatePowerUpUI()
    {
        if (paparazzi.GetComponent<StanItems>().isHoldingItem || paparazzi.GetComponent<StanItems>().CheckIfUsingTelephoto())
        {
            lens.color = new Color(1, 1, 1, 1);
        } else
        {
            lens.color = new Color(1, 1, 1, 0.4f);
        }

        if (paparazzi.GetComponent<StanItems>().CheckIfUsingTelephoto())
        {
            lens.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 72f));
        }

        if (celeb.GetComponent<CelebItems>().isHoldingItem || !celeb.GetComponent<CelebItems>().CheckIfVisible())
        {
            disguise.color = new Color(1, 1, 1, 1);
        }
        else
        {
            disguise.color = new Color(1, 1, 1, 0.4f);
        }

        if (!celeb.GetComponent<CelebItems>().CheckIfVisible())
        {
            disguise.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 72f));
        }
    }

    void UpdateBattery()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i + 1 > paparazzi.resources) battery.GetChild(i).gameObject.SetActive(false);
            else battery.GetChild(i).gameObject.SetActive(true);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i + 1 <= paparazzi.score) hearts.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            else hearts.GetChild(i).GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
        }
    }

    void UpdateStars()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i + 1 <= celeb.score) stars.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            else stars.GetChild(i).GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("IntroScene");
    }

    public void FlashCamera(bool isGoodPhoto)
    {
        StartCoroutine(PlayCameraFlash(isGoodPhoto));
        flashPanel.gameObject.GetComponent<AudioSource>().Play();
    }

    IEnumerator PlayCameraFlash(bool isGoodPhoto)
    {
        float startTime = Time.time;
        float angle = Random.Range(-25, 25);

/*        if (isGoodPhoto) photo.sprite = photoSprites[0];
        else photo.sprite = photoSprites[1];
        photo.transform.rotation = Quaternion.Euler(Vector3.forward * angle);*/
        
        if (isGoodPhoto) photograph.sprite = photoSprites[0];
        else photograph.sprite = photoSprites[1];
        photograph.transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        while (Time.time - startTime <= 0.2f)
        {
            flashPanel.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), (Time.time - startTime) / 0.2f);
            // photo.color = Color.Lerp(photo.color, new Color(1, 1, 1, 1), (Time.time - startTime) / 0.2f);
            photograph.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), (Time.time - startTime) / 0.2f);
            yield return null;
        }
        startTime = Time.time;
        StartCoroutine(FadePhotograph());
        while (Time.time - startTime <= 0.2f)
        {
            flashPanel.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), (Time.time - startTime) / 0.2f);
            // photo.color = Color.Lerp(photo.color, new Color(1, 1, 1, 0), (Time.time - startTime) / 0.2f);
            // photograph.color = Color.Lerp(photograph.color, new Color(1, 1, 1, 0), (Time.time - startTime) / 0.2f);
            yield return null;
        }
    }

    IEnumerator FadePhotograph()
    {
        float startTime = Time.time;
        while (Time.time - startTime <= 2.5f) {
            photograph.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), (Time.time - startTime) / 2.5f);
            yield return null;
        }
    }
}
