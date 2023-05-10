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

    private Coroutine collisionCoroutine;
    private Coroutine recoverCoroutine;

    // Tile Movement Version
    private Tilemap pathmap; // have to set this before can move
    private bool isMoving = false;

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
        yield return new WaitForSeconds(5f);
        movementSpeed = 2;
        GetComponent<AfterImage>().Activate(false);
    }

    void SetSlider()
    {
        if (gameObject.CompareTag("Paparazzi")) slider = GameObject.Find("CameraTimeoutSlider").GetComponent<Slider>();
        else if (gameObject.CompareTag("Celebrity")) slider = GameObject.Find("ActivationTimeoutSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
    }

    public void SetTilemap(Tilemap tilemap)
    {
        pathmap = tilemap;
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
                actionKey = KeyCode.Q;
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
                actionKey = KeyCode.Q;
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
        if (pathmap.GetTile(pathmap.WorldToCell(transform.position)) != null)
        {
            rb.MovePosition(pathmap.GetCellCenterWorld(pathmap.WorldToCell(transform.position)));
        }
    }

    private void TileBasedMove()
    {
        Vector3Int direction = Vector3Int.zero;

        if (Input.GetKey(upKey)) direction = Vector3Int.up;
        else if (Input.GetKey(downKey)) direction = Vector3Int.down;
        else if (Input.GetKey(leftKey)) direction = Vector3Int.left;
        else if (Input.GetKey(rightKey)) direction = Vector3Int.right;

        if (direction == Vector3Int.zero)
        {
            if (!isMoving)
            {
                ResetPositionToTileCenter();
            }
        } else
        {
            Vector3Int currentTilePos = pathmap.WorldToCell(transform.position);
            Vector3Int nextTilePos = currentTilePos + direction;
            // Debug.Log(pathmap.GetCellCenterWorld(currentTilePos));
            // Debug.Log(pathmap.GetCellCenterWorld(nextTilePos));


            if (pathmap.GetTile(nextTilePos) != null && !isMoving)
            {
                // StartCoroutine(MoveToTile(currentTilePos, nextTilePos));
                StartCoroutine(MoveToNextTile(direction));
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

    IEnumerator MoveToNextTile(Vector3Int direction)
    {
        
        Vector2 moveTowards = transform.position;
        Vector2 nextPosition = moveTowards + new Vector2(direction.x, direction.y) * 0.4f;
        
        isMoving = true;
        float lastDistance = Vector2.Distance(nextPosition, moveTowards);

        // while (Vector2.Distance(nextPosition, moveTowards) <= lastDistance)
        while (moveTowards != nextPosition)
        {
            // METHOD 1
/*            if (!canMove) break;
            // Debug.Log($"distance: {Vector2.Distance(nextPosition, moveTowards)}");
            lastDistance = Vector2.Distance(nextPosition, moveTowards);
            // Debug.Log($"last distance: {lastDistance}");
            moveTowards += movementSpeed * Time.deltaTime * new Vector2(direction.x, direction.y);
            rb.MovePosition(moveTowards);
            yield return null;*/
            //

            //METHOD 2
            if (!canMove) break;
            lastDistance = Vector2.Distance(nextPosition, moveTowards);
            // moveTowards += movementSpeed * Time.deltaTime * new Vector2(direction.x, direction.y);
            moveTowards = Vector2.MoveTowards(moveTowards, nextPosition, movementSpeed * Time.deltaTime);
            rb.MovePosition(moveTowards);
            yield return null;

        }

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

        // switching to tile-based movement:
        // convert tile to world and MovePosition to tile
        // on GetKey move in direction one tile 
        // startcoroutine only when reach destination location



        moveTowards += movementSpeed * Time.deltaTime * direction;
        rb.MovePosition(moveTowards);

/*        slider.transform.position = cam.WorldToScreenPoint(rb.position + 0.25f * Vector2.up);*/
    }

    private void FixedUpdate()
    {
        if (pathmap == null) return;
        if (canMove) TileBasedMove();
        // DirectionBasedMove();   
        if (gameObject.CompareTag("Celebrity"))
        {
            slider.transform.position = cam.WorldToScreenPoint(rb.position + 0.25f * Vector2.up);
        } else
        {
            slider.transform.position = cam.WorldToScreenPoint(rb.position - 0.25f * Vector2.up);
        }
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
        /*canMove = false;
        rb.velocity = Vector2.zero;
        
        // yield return new WaitForSeconds(1.0f);
        if (collider != null)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collider, true);
        }
        ResetPositionToTileCenter();
        // yield return new WaitForSeconds(0.05f);
        canMove = true;*/

        canMove = false;
        rb.velocity = collider.GetComponent<Rigidbody2D>().velocity;
        if (collider != null) Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collider, true);
        yield return new WaitForSeconds(1.0f);
        // rb.velocity = Vector2.zero;
        // ResetPositionToTileCenter();
        if (recoverCoroutine != null) StopCoroutine(Recover());
        recoverCoroutine = StartCoroutine(Recover());
        // canMove = true;
    }

    IEnumerator Recover()
    {
        rb.velocity = Vector2.zero;
        Vector2 moveTowards = transform.position;
        Vector2 nextPosition = pathmap.GetCellCenterWorld(pathmap.WorldToCell(transform.position));
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

    //add code to reset player position if knocked out of bounds??


}
