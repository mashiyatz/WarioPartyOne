using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

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
    public GameObject target;

    private Coroutine collisionCoroutine;
    private Coroutine recoverCoroutine;

    // Tile Movement Version
    private Tilemap tileMap;
    private Tilemap obstacleMap;
    private bool isMoving = false;
    private Vector3Int direction;

    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        score = 0;
        resources = 3;
        SetButtonConfig();
        SetSlider();
        target.SetActive(true);
        StartCoroutine(TurnOffTargetAfterSeconds());
    }

    public void StartSpeedUp()
    {
        StopCoroutine(SpeedUp());
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        movementSpeed = 3;
        GetComponent<AfterImage>().Activate(true);
        yield return new WaitForSeconds(ValueSettings.speedBuffTime);
        movementSpeed = 2;
        GetComponent<AfterImage>().Activate(false);
    }

    void SetSlider()
    {
        if (gameObject.CompareTag("Paparazzi")) slider = GameObject.Find("CameraTimeoutSlider").GetComponent<Slider>();
        else if (gameObject.CompareTag("Celebrity")) slider = GameObject.Find("ActivationTimeoutSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
    }

    public void SetPathTilemap(Tilemap tilemap)
    {
        tileMap = tilemap;
    }
    public void SetObstacleTilemap(Tilemap tilemap)
    {
        obstacleMap = tilemap;
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
                actionKey = KeyCode.U;
                break;
            case 2:
                upKey = KeyCode.A;
                downKey = KeyCode.D;
                leftKey = KeyCode.S;
                rightKey = KeyCode.W;
                actionKey = KeyCode.E;
                break;
            case 3:
                upKey = KeyCode.L;
                downKey = KeyCode.J;
                leftKey = KeyCode.I;
                rightKey = KeyCode.K;
                actionKey = KeyCode.U;
                break;
            case 4:
                upKey = KeyCode.K;
                downKey = KeyCode.I;
                leftKey = KeyCode.L;
                rightKey = KeyCode.J;
                actionKey = KeyCode.U;
                break;
            default:
                upKey = KeyCode.W;
                downKey = KeyCode.S;
                leftKey = KeyCode.A;
                rightKey = KeyCode.D;
                actionKey = KeyCode.E;
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

    private void ResetPositionToTileCenter()
    {
        if (tileMap.GetTile(tileMap.WorldToCell(transform.position)) != null)
        {
            rb.MovePosition(tileMap.GetCellCenterWorld(tileMap.WorldToCell(transform.position)));
        }
    }

    private void TileBasedMove()
    {
        direction = Vector3Int.zero;

        if (Input.GetKey(upKey)) direction = Vector3Int.up;
        if (Input.GetKey(downKey)) direction = Vector3Int.down;
        if (Input.GetKey(leftKey)) direction = Vector3Int.left;
        if (Input.GetKey(rightKey)) direction = Vector3Int.right;

        if (direction != Vector3Int.zero)
        {
            Vector3Int currentTilePos = tileMap.WorldToCell(transform.position);
            Vector3Int nextTilePos = currentTilePos + direction;
            // Debug.Log(pathmap.GetCellCenterWorld(currentTilePos));
            // Debug.Log(pathmap.GetCellCenterWorld(nextTilePos));

            if (tileMap.GetTile(nextTilePos) != null)
            {
                if (!isMoving) StartCoroutine(MoveToNextTile(direction));
            }

            lastDirection = new Vector2(direction.x, direction.y);
        }        

        float angle;
        angle = Vector2.SignedAngle(Vector2.up, lastDirection);

        if (gameObject.CompareTag("Paparazzi"))
        {
            Vector3 targetRotation = new Vector3(0, 0, angle);
            Quaternion lookTo = Quaternion.Euler(targetRotation);

            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, lookTo, rotationSpeed * Time.deltaTime);
        }

        if (anim != null)
        {
            float convertedAngle = angle + 180;

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
    }

    IEnumerator MoveToNextTile(Vector3Int dir)
    {
        Vector2 moveTowards = transform.position;
        Vector2 nextPosition = moveTowards + new Vector2(dir.x, dir.y) * 0.4f;

        isMoving = true;

        while (moveTowards != nextPosition)
        {
            if (!canMove) break;

            //// cornering
            if (direction != dir && direction != Vector3Int.zero)
            {
                float dist = Vector2.Distance(moveTowards, nextPosition);
                if (movementSpeed * Time.deltaTime > dist)
                {
                    rb.MovePosition(nextPosition);
                    ResetPositionToTileCenter();

                    Vector2 nextPosHolder = nextPosition + new Vector2(direction.x, direction.y) * 0.4f;

                    if (tileMap.GetTile(tileMap.WorldToCell(nextPosHolder)) != null)
                    {
                        rb.MovePosition(nextPosition + new Vector2(direction.x, direction.y) * (movementSpeed * Time.deltaTime - dist));
                        moveTowards = transform.position;
                        dir = direction;
                        nextPosition += new Vector2(dir.x, dir.y) * 0.4f;
                    } 
                    
                    yield return null;
                }
            }
            ////

            moveTowards = Vector2.MoveTowards(moveTowards, nextPosition, movementSpeed * Time.deltaTime);
            rb.MovePosition(moveTowards);
            yield return null;
        }

        ResetPositionToTileCenter();
        isMoving = false;
    
    }

    private void DirectionBasedMove()
    {
        float angle;
        Vector2 direction = Vector2.zero;
        Vector2 moveTowards = transform.position;

        // direction with normalization
        if (Input.GetKey(upKey)) direction += Vector2.up;
        if (Input.GetKey(downKey)) direction += Vector2.down;
        if (Input.GetKey(leftKey)) direction += Vector2.left;
        if (Input.GetKey(rightKey)) direction += Vector2.right;

        direction.Normalize();
        //

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

            //Debug.Log(convertedAngle);
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
    }

    private void FixedUpdate()
    {
        if (tileMap == null) return;
        if (canMove) TileBasedMove();
        if (gameObject.CompareTag("Celebrity"))
        {
            slider.transform.position = cam.WorldToScreenPoint(rb.position + 0.25f * Vector2.up);
        } else
        {
            slider.transform.position = cam.WorldToScreenPoint(rb.position - 0.25f * Vector2.up);
        }
    }

    IEnumerator TurnOffTargetAfterSeconds()
    {
        yield return new WaitForSeconds(3.0f);
        if (target.activeSelf) target.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            if (collisionCoroutine != null) StopCoroutine(collisionCoroutine);
            collisionCoroutine = StartCoroutine(CarCollisionCooldown(collision.collider));
            
        }
    }

    IEnumerator CarCollisionCooldown(Collider2D collider)
    {
        canMove = false;
        rb.velocity = collider.GetComponent<Rigidbody2D>().velocity;
        if (collider != null) Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collider, true);
        yield return new WaitForSeconds(1.0f);
        if (recoverCoroutine != null) StopCoroutine(Recover());
        recoverCoroutine = StartCoroutine(Recover());
    }

    IEnumerator Recover()
    {
        rb.velocity = Vector2.zero;
        Vector2 moveTowards = transform.position;
        Vector2 nextPosition = tileMap.GetCellCenterWorld(new Vector3Int(-1, 3, 0));

        if (tileMap.GetTile(tileMap.WorldToCell(transform.position)) != null)
        {
            nextPosition = tileMap.GetCellCenterWorld(tileMap.WorldToCell(transform.position));
        } else
        {
            Vector3Int[] dirs = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
            foreach (Vector3Int dir in dirs)
            {
                if (tileMap.GetTile(tileMap.WorldToCell(transform.position) + dir) != null) {
                    nextPosition = tileMap.GetCellCenterWorld(tileMap.WorldToCell(transform.position));
                    break;
                } 
            } 
        }

        float lastDistance = Vector2.Distance(nextPosition, moveTowards);

        while (Vector2.Distance(nextPosition, moveTowards) <= lastDistance)
        {
            lastDistance = Vector2.Distance(nextPosition, moveTowards);
            moveTowards += movementSpeed * Time.deltaTime * (nextPosition - moveTowards).normalized;
            rb.MovePosition(moveTowards);
            yield return null;
        }

        canMove = true;
    }

}
