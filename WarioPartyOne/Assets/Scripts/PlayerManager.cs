using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode actionKey;

    public float rotationSpeed;
    public float movementSpeed;

    public TextMeshProUGUI scoreTextbox;
    public TextMeshProUGUI fundsTextbox;
    public int score;
    public int funds;

    private Rigidbody2D rb;
    public GameObject panel;
    public TextMeshProUGUI winStatus;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        score = 0;
        funds = 3;
        scoreTextbox.text = $"Score: {score}";
        fundsTextbox.text = $"Funds: {funds}";
    }

    public void UpdateScore()
    {
        score += 1;
        scoreTextbox.text = $"Score: {score}";
    }

    public void UpddateFunds(int amt)
    {
        funds += amt;
        fundsTextbox.text = $"Funds: {funds}";
    }

    void Update()
    {
        if (score >= 3)
        {
            panel.SetActive(true);
            winStatus.text = $"{gameObject.name} wins"; 
            winStatus.gameObject.SetActive(true);
            StartCoroutine(RestartGame());
        }

        if (StartPanelAnimation.canMove)
        {
            float angle;
            Vector2 direction = Vector2.zero;
            Vector2 moveTowards = transform.position;

            if (Input.GetKey(upKey)) direction += Vector2.up;
            else if (Input.GetKey(downKey)) direction += Vector2.down;
            else if (Input.GetKey(leftKey)) direction += Vector2.left;
            else if (Input.GetKey(rightKey)) direction += Vector2.right;

            if (direction != Vector2.zero)
            {
                // angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                angle = Vector2.SignedAngle(Vector2.up, direction);
                Vector3 targetRotation = new Vector3(0, 0, angle);
                Quaternion lookTo = Quaternion.Euler(targetRotation);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookTo, rotationSpeed * Time.deltaTime);
            }

            moveTowards += movementSpeed * Time.deltaTime * direction;
            rb.MovePosition(moveTowards);
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
