using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Movement player;
    bool isMoving;
    private Vector3 origPos, targetPos;

    [SerializeField]
    Vector3 direction;
    Vector3 originalDirection;

    [SerializeField]
    float moveTime = 0.2f;

    [SerializeField]
    int moveAmount = 1;

    [SerializeField]
    LayerMask whatIsBarrier;

    Vector3 startingPos;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isIdle", false);
        startingPos = transform.position;
        originalDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<Movement>();

        anim.SetFloat("xDirection", direction.x);
        anim.SetFloat("yDirection", direction.y);

        if (player.isMoving && !isMoving)
        {
            if (Physics2D.OverlapCircle(transform.position + direction, 0.2f, whatIsBarrier))
            {
                direction *= -1;
            }
            StartCoroutine(MoveEnemy(direction));
        }

        if(player.movesLeft == 10)
        {
            transform.position = startingPos;
            direction = originalDirection;
        }
    }

    private IEnumerator MoveEnemy(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + (direction * moveAmount);

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

    }
}
