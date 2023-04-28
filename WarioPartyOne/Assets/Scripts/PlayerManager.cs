using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int buttonConfig;

    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode actionKey;

    public float rotationSpeed;
    public float movementSpeed;

    public int score;
    public float resources;

    private Rigidbody2D rb;
    [SerializeField] private Vector2 lastDirection;

    public Slider slider;
    private Camera cam;

    private Animator anim;
    public string[] animStates;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        score = 0;
        resources = 3;
        SetButtonConfig();
        SetSlider();
    }

    public void StartSpeedUp()
    {
        StopCoroutine(SpeedUp());
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        movementSpeed = 2;
        yield return new WaitForSeconds(5f);
        movementSpeed = 1;
    }

    void SetSlider()
    {
        if (gameObject.CompareTag("Paparazzi")) slider = GameObject.Find("CameraTimeoutSlider").GetComponent<Slider>();
        else if (gameObject.CompareTag("Celebrity")) slider = GameObject.Find("ActivationTimeoutSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
    }

    void SetButtonConfig()
    {
        switch (buttonConfig)
        {
            case 1:
                upKey = KeyCode.I;
                downKey = KeyCode.K;
                leftKey = KeyCode.J;
                rightKey = KeyCode.L;
                actionKey = KeyCode.RightShift;
                break;
            case 2:
                upKey = KeyCode.A;
                downKey = KeyCode.D;
                leftKey = KeyCode.S;
                rightKey = KeyCode.W;
                actionKey = KeyCode.LeftShift;
                break;
            case 3:
                upKey = KeyCode.L;
                downKey = KeyCode.J;
                leftKey = KeyCode.I;
                rightKey = KeyCode.K;
                actionKey = KeyCode.RightShift;
                break;
            default:
                upKey = KeyCode.W;
                downKey = KeyCode.S;
                leftKey = KeyCode.A;
                rightKey = KeyCode.D;
                actionKey = KeyCode.LeftShift;
                break;
        }
    }

    public void UpdateScore()
    {
        score += 1;
        
    }

    public void UpdateResource(int amt)
    {
        float r = resources;
        r = Mathf.Clamp(r + amt, 0, 3);
        resources = r;
    }

    private void FixedUpdate()
    {
        float angle;
        Vector2 direction = Vector2.zero;
        Vector2 moveTowards = transform.position;

        if (Input.GetKey(upKey)) direction += Vector2.up;
        if (Input.GetKey(downKey)) direction += Vector2.down;
        if (Input.GetKey(leftKey)) direction += Vector2.left;
        if (Input.GetKey(rightKey)) direction += Vector2.right;

        direction.Normalize();

        if (direction != Vector2.zero) lastDirection = direction;
        
        // angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        angle = Vector2.SignedAngle(Vector2.up, lastDirection);

        if (gameObject.CompareTag("Paparazzi"))
        {
            Vector3 targetRotation = new Vector3(0, 0, angle);
            Quaternion lookTo = Quaternion.Euler(targetRotation);
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, lookTo, rotationSpeed * Time.deltaTime);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, lookTo, rotationSpeed * Time.deltaTime);
        }

        if (anim != null)
        {
            float convertedAngle = angle + 180;

            Debug.Log(convertedAngle);
            if (convertedAngle >= 315 || convertedAngle < 45) anim.Play(animStates[1]);
            else if (convertedAngle >= 45 && convertedAngle < 135)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                anim.Play(animStates[3]);
            }
            else if (convertedAngle >= 135 && convertedAngle < 225) anim.Play(animStates[5]);
            else if (convertedAngle >= 225 && convertedAngle < 315)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                anim.Play(animStates[3]);
            }
        }

        moveTowards += movementSpeed * Time.deltaTime * direction;
        rb.MovePosition(moveTowards);

        slider.transform.position = cam.WorldToScreenPoint(rb.position + 0.25f * Vector2.up); 
    }
}
