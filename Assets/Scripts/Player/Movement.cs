using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public bool isMoving;
    

    public bool hasPowerup;
    int powerMultiplier;
    public int powerupLeft = 5;

    private Vector3 origPos, targetPos;

    [SerializeField]
    float moveTime = 0.2f;

    public LayerMask whatIsBarrier;
    public LayerMask whatIsGoal;
    public LayerMask whatIsPowerup;

    public int movesLeft = 10;


    GameObject spawnPoint;

    [SerializeField]
    GameObject virtualCam;

    [SerializeField]
    GameObject hud;

    [SerializeField]
    int sceneToLoad;
    [SerializeField]
    int sceneToUnload;

    Animator anim;

    SpriteRenderer renderer;

    [SerializeField]
    AudioClip step;
    [SerializeField]
    AudioClip clear;
    [SerializeField]
    AudioClip reset;
    [SerializeField]
    AudioClip powerup;

    AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        virtualCam.SetActive(true);
        StartCoroutine(UnloadLevel());

        anim.SetBool("isIdle", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) && !isMoving)
        {
            if (!Physics2D.OverlapCircle(transform.position + Vector3.up, 0.2f, whatIsBarrier))
            {
                anim.SetBool("isIdle", false);
                anim.SetBool("right", false);
                anim.SetBool("left", false);
                anim.SetBool("down", false);
                anim.SetBool("up", true);


                StartCoroutine(MovePlayer(Vector3.up));
            }
        }

        if (Input.GetKey(KeyCode.A) && !isMoving)
        {
            if (!Physics2D.OverlapCircle(transform.position + Vector3.left, 0.2f, whatIsBarrier))
            {
                anim.SetBool("isIdle", false);
                anim.SetBool("right", false);
                anim.SetBool("left", true);
                anim.SetBool("down", false);
                anim.SetBool("up", false);
                StartCoroutine(MovePlayer(Vector3.left));
            }
        }

        if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            if (!Physics2D.OverlapCircle(transform.position + Vector3.down, 0.2f, whatIsBarrier))
            {
                anim.SetBool("isIdle", false);
                anim.SetBool("right", false);
                anim.SetBool("left", false);
                anim.SetBool("down", true);
                anim.SetBool("up", false);
                StartCoroutine(MovePlayer(Vector3.down));
            }
        }

        if (Input.GetKey(KeyCode.D) && !isMoving)
        {
            if (!Physics2D.OverlapCircle(transform.position + Vector3.right, 0.2f, whatIsBarrier))
            {
                anim.SetBool("isIdle", false);
                anim.SetBool("right", true);
                anim.SetBool("left", false);
                anim.SetBool("down", false);
                anim.SetBool("up", false);
                StartCoroutine(MovePlayer(Vector3.right));
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        audioSource.PlayOneShot(reset);
        spawnPoint = GameObject.FindGameObjectWithTag("Start");
        
        Vector3 spawnPosition = new Vector3(spawnPoint.GetComponent<TilemapCollider2D>().bounds.center.x, spawnPoint.GetComponent<TilemapCollider2D>().bounds.center.y, 0);

        transform.position = spawnPosition;
        movesLeft = 10;

        Powerup[] powerUps = FindObjectsOfType<Powerup>(true);
        for(int i = 0; i < powerUps.Length; i++)
        {
            powerUps[i].gameObject.SetActive(true);
        }
        hasPowerup = false;
        StartCoroutine(Flicker());
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        movesLeft -= 1;

        audioSource.PlayOneShot(step);

        if (hasPowerup)
        {
            powerupLeft -= 1;
            powerMultiplier = 2;
        }
        else
        {
            powerMultiplier = 1;
        }

        float elapsedTime = 0;

        origPos = transform.position;

        if (hasPowerup && Physics2D.OverlapCircle(origPos + direction * powerMultiplier, 0.2f, whatIsBarrier))
        {
            targetPos = origPos + direction;
        }
        else
        {
            targetPos = origPos + direction * powerMultiplier;
        }

        if (powerupLeft <= 0)
        {
            hasPowerup = false;
        }

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        if (movesLeft <= 0 && !Physics2D.OverlapCircle(transform.position, 0.2f, whatIsGoal))
        {
            Respawn();
        }

        if (Physics2D.OverlapCircle(transform.position, 0.2f, whatIsBarrier))
        {
            Respawn();
        }

        if (Physics2D.OverlapCircle(transform.position, 0.2f, whatIsGoal))
        {
            audioSource.PlayOneShot(clear);
            LoadNextLevel();
        }

        if(Physics2D.OverlapCircle(transform.position, 0.2f, whatIsPowerup) && !hasPowerup){
            audioSource.PlayOneShot(powerup);
            GameObject powerUp = Physics2D.OverlapCircle(transform.position, 0.2f, whatIsPowerup).gameObject;
            powerUp.SetActive(false);
            hasPowerup = true;
            powerupLeft = 5;
        }

        isMoving = false;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        hud.SetActive(false);
    }

    IEnumerator UnloadLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = true;
    }
}
