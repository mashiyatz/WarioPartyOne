using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public enum GameState { START, PLAY, WIN }
    public GameState currentState;
    public int pointsToWin;

    public GameObject startPanel;
    public GameObject startText;
    public GameObject winText;
    public GameObject celebUI;
    public GameObject papaUI;

    public GameObject celebPrefab;
    public GameObject paparazziPrefab;

    private PlayerManager celeb;
    private PlayerManager paparazzi;
    private ActionButton celebAction;
    private ActionButton paparazziAction;

    // text ui version
    public TextMeshProUGUI celebScoreTextbox;
    public TextMeshProUGUI papaScoreTextbox;
    public TextMeshProUGUI celebResourceTextbox;
    public TextMeshProUGUI papaResourceTextbox;

    // graphic ui
    public Transform stars;
    public Transform hearts;
    public Transform battery;

    public GameObject objectGenerator;

    public Tilemap tilemapPath;
    public Image flashPanel;

    [SerializeField] private Vector3Int papaStartPos; 
    [SerializeField] private Vector3Int celebStartPos; 

    void Start()
    {
        InstantiatePlayers();

        celeb.enabled = false;
        paparazzi.enabled = false;
        celebAction.enabled = false;
        paparazziAction.enabled = false;

        startPanel.SetActive(true);
        startText.SetActive(true);
        winText.SetActive(false);
        celebUI.SetActive(false);
        papaUI.SetActive(false);
        objectGenerator.SetActive(false);

        currentState = GameState.START;
    }

    void InstantiatePlayers()
    {
        if (celeb != null) Destroy(celeb.gameObject);
        if (paparazzi != null) Destroy(paparazzi.gameObject);

        Vector3 papaPos = tilemapPath.CellToWorld(papaStartPos) + new Vector3(0.125f, 0.125f, 0);
        Vector3 celebPos = tilemapPath.CellToWorld(celebStartPos) + new Vector3(0.125f, 0.125f, 0);

        paparazzi = Instantiate(paparazziPrefab, papaPos, Quaternion.Euler(new Vector3(0, 0, 180))).GetComponent<PlayerManager>();
        celeb = Instantiate(celebPrefab, celebPos, Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<PlayerManager>();
        paparazziAction = paparazzi.GetComponentInChildren<ActionButton>();
        celebAction = celeb.GetComponentInChildren<ActionButton>();
    }

    void Update()
    {
        if (currentState == GameState.START)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                startPanel.SetActive(false);
                startText.SetActive(false);
                celebUI.SetActive(true);
                papaUI.SetActive(true);

                celeb.enabled = true;
                paparazzi.enabled = true;
                celebAction.enabled = true;
                paparazziAction.enabled = true;
                objectGenerator.SetActive(true);

                currentState = GameState.PLAY;
            }
        }
        else if (currentState == GameState.PLAY)
        {
            UpdateUI();

            if (celeb.score == pointsToWin || paparazzi.score == pointsToWin)
            {
                celeb.enabled = false;
                paparazzi.enabled = false;
                objectGenerator.SetActive(false);

                startPanel.SetActive(true);

                if (celeb.score > paparazzi.score)
                {
                    winText.GetComponent<TextMeshProUGUI>().text = "Celeb wins!";
                } else
                {
                    winText.GetComponent<TextMeshProUGUI>().text = "Paparazzi wins!";
                }

                winText.SetActive(true);

                currentState = GameState.WIN;
            }
        }
        else if (currentState == GameState.WIN)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                /*
                startText.SetActive(true);
                celebUI.SetActive(false);
                papaUI.SetActive(false);

                celeb.enabled = false;
                paparazzi.enabled = false;

                InstantiatePlayers();
                
                currentState = GameState.START;
                 */

                StartCoroutine(RestartGame());
            }
        }

    }

    void UpdateUI()
    {
        /*        celebScoreTextbox.text = $"Fame: {celeb.score}";
                papaScoreTextbox.text = $"Followers: {paparazzi.score}";
                celebResourceTextbox.text = $"Anonymity: {celeb.resources / 3 * 100:F1}%";
                papaResourceTextbox.text = $"Battery: {paparazzi.resources / 3 * 100:F1}%";*/
        UpdateBattery();
        UpdateHearts();
        UpdateStars();
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
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FlashCamera()
    {
        StartCoroutine(PlayCameraFlash());
        flashPanel.gameObject.GetComponent<AudioSource>().Play();
    }

    IEnumerator PlayCameraFlash()
    {
        float startTime = Time.time;
        while (Time.time - startTime <= 0.2f)
        {
            flashPanel.color = Color.Lerp(flashPanel.color, new Color(1, 1, 1, 1), (Time.time - startTime) / 0.2f);
            yield return null;
        }
        startTime = Time.time;
        while (Time.time - startTime <= 0.2f)
        {
            flashPanel.color = Color.Lerp(flashPanel.color, new Color(1, 1, 1, 0), (Time.time - startTime) / 0.2f);
            yield return null;
        }
    }
}
