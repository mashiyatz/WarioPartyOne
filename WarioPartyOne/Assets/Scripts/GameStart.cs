using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStart : MonoBehaviour
{
    public enum GameState { START, PLAY, WIN }
    private GameState currentState;

    public GameObject startPanel;
    public GameObject startText;
    public GameObject winText;
    public GameObject celebUI;
    public GameObject papaUI;

    public GameObject celebPrefab;
    public GameObject paparazziPrefab;

    private PlayerManager celeb;
    private PlayerManager paparazzi;

    public TextMeshProUGUI celebScoreTextbox;
    public TextMeshProUGUI papaScoreTextbox;
    public TextMeshProUGUI celebResourceTextbox;
    public TextMeshProUGUI papaResourceTextbox;
    public GameObject objectGenerator;

    [SerializeField] private Vector3 papaStartPos; 
    [SerializeField] private Vector3 celebStartPos; 

    void Start()
    {
        InstantiatePlayers();

        celeb.enabled = false;
        paparazzi.enabled = false;

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
        paparazzi = Instantiate(paparazziPrefab, papaStartPos, Quaternion.Euler(new Vector3(0, 0, 180))).GetComponent<PlayerManager>();
        celeb = Instantiate(celebPrefab, celebStartPos, Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<PlayerManager>();
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
                objectGenerator.SetActive(true);

                currentState = GameState.PLAY;
            }
        }
        else if (currentState == GameState.PLAY)
        {
            UpdateUI();

            if (celeb.score == 3 || paparazzi.score == 3)
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
        celebScoreTextbox.text = $"Fame: {celeb.score}";
        papaScoreTextbox.text = $"Followers: {paparazzi.score}";
        celebResourceTextbox.text = $"Anonymity: {celeb.resources / 3 * 100:F1}%";
        papaResourceTextbox.text = $"Battery: {paparazzi.resources / 3 * 100:F1}%";

    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
