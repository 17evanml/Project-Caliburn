using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerAI : EnemyController
{
    public AIBrain aI;
    public CharacterController cc; // Character Controller Component
    public enum attackType { SideSlash, DownSlash, SpinSlash };
    public Animator animator;
    public bool isAttacking = false;
    public bool invincible = false;
    public GameObject[] dismemberable;
    public GameObject[] drops;
    public int[] dropRates;
    private float iFrames = 1;
    bool dead = false;
    int decisioncounter = 0;
    int[] pastDecisions = new int[3];
    int HP = 2;
    float strafeDir;
    GameObject player;

    bool strafeB = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < pastDecisions.Length; i++)
        {
            pastDecisions[i] = -1;
        }
        aI = GameObject.Find("AIBrain").GetComponent<AIBrain>();
        player = GameObject.Find("Player");
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (HP > 0)
        {
            if (!cc.isGrounded)
            { // Gravity
                cc.Move(Vector3.up * -1);
            }
            enemyLogic();

            // Manages isAttacking  // HOLY CRAP THIS IS UGLY
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Enter")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Enter 0")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Enter 1")) { isAttacking = true; }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Exit")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Exit 0")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Exit 1")) { isAttacking = false; }
        }
    }

    /// <summary> Moves the Enemy a distance of moveVector units. </summary>
    /// <param name="moveVector"> Distance the Enemy should move. </param>
    public void move(Vector2 moveVector)
    {
        Vector3 moveVector3 = new Vector3(moveVector.x, 0f, moveVector.y);
        StartCoroutine(moveCoroutine(moveVector3, 1));
    }

    /// <summary> Moves the Enemy a distance of moveVector units. </summary>
    /// <param name="moveVector"> Distance the Enemy should move. </param>
    public void move(Vector3 moveVector)
    {
        StartCoroutine(moveCoroutine(moveVector, 1));
    }

    /// <summary> Moves the Enemy a distance of moveVector units at a specified speed. </summary>
    /// <param name="moveVector"> Distance the Enemy should move. </param>
    /// <param name="speed"> Movement Speed of the Enemy. </param>
    public void move(Vector3 moveVector, float speed)
    {
        StartCoroutine(moveCoroutine(moveVector, speed));
    }

    /// <summary> Plays a Random Attack Animation. </summary>
    public void attack()
    {
        int attack = Random.Range(0, 3);
        switch (attack)
        {
            case 0:
                animator.SetTrigger("SideSlash");
                break;
            case 1:
                animator.SetTrigger("DownSlash");
                break;
            case 2:
                animator.SetTrigger("SpinSlash");
                break;
            default:
                animator.SetTrigger("SideSlash");
                Debug.Log("ERROR");
                break;
        }
    }

    public void cancelattack()
    {
        animator.ResetTrigger("SideSlash");
        animator.ResetTrigger("DownSlash");
        animator.ResetTrigger("SpinSlash");
    }

    /// <summary> Plays a specified Attack Animation. </summary>
    /// <param name="attackType"> Which Attack to be used. </param>
    public void attack(attackType attackType)
    {
        int attack = (int)attackType;
        switch (attack)
        {
            case 0:
                animator.SetTrigger("SideSlash");
                break;
            case 1:
                animator.SetTrigger("DownSlash");
                break;
            case 2:
                animator.SetTrigger("SpinSlash");
                break;
            default:
                animator.SetTrigger("SideSlash");
                Debug.Log("ERROR");
                break;
        }
    }

    /// <summary> Rotates the Enemy a set Rotation. </summary>
    /// <param name="rotation"> Amount the enemy should be rotated. </param>
    public void rotate(float rotation)
    {
        StartCoroutine(rotateCoroutine(rotation, 1f));
    }

    /// <summary> Rotates the Enemy a set Rotation at a specified speed. </summary>
    /// <param name="rotation"> Amount the enemy should be rotated. </param>
    /// <param name="speed"> Rotation Speed of the Enemy. </param>
    public void rotate(float rotation, float speed)
    {
        StartCoroutine(rotateCoroutine(rotation, speed));
    }

    /// <summary> Moves the Enemy a distance of moveVector units at a specified speed. </summary>
    /// <param name="moveVector"> Distance the Enemy should move. </param>
    /// <param name="speed"> Movement Speed of the Enemy. </param>
    public IEnumerator moveCoroutine(Vector3 moveVector, float speed)
    {
        Vector3 totalVector = Vector3.zero;
        Vector3 localVector = (transform.forward * moveVector.normalized.z) + (transform.right * moveVector.normalized.x);

        animator.SetFloat("SpeedX", moveVector.normalized.x);
        animator.SetFloat("SpeedY", moveVector.normalized.z);

        while (totalVector.magnitude < moveVector.magnitude)
        {
            localVector = (transform.forward * moveVector.normalized.z) + (transform.right * moveVector.normalized.x);
            totalVector += (moveVector.normalized * speed);
            cc.Move(localVector * speed);
            Debug.Log("100");
            yield return 0;
        }
        animator.SetFloat("SpeedX", 0);
        animator.SetFloat("SpeedY", 0);
    }

    public void moveUpdate(Vector3 moveVector, float speed)
    {
        Vector3 totalVector = Vector3.zero;
        Vector3 localVector = (transform.forward * moveVector.normalized.z) + (transform.right * moveVector.normalized.x);

        animator.SetFloat("SpeedX", moveVector.normalized.x);
        animator.SetFloat("SpeedY", moveVector.normalized.z);

        while (totalVector.magnitude < moveVector.magnitude)
        {
            localVector = (transform.forward * moveVector.normalized.z) + (transform.right * moveVector.normalized.x);
            totalVector += (moveVector.normalized * speed);
            cc.Move(localVector * speed);
            Debug.Log("100");
            //yield return 0;
        }

    }

    /// <summary> Rotates the Enemy a set Rotation at a specified speed. </summary>
    /// <param name="rotation"> Amount the enemy should be rotated. </param>
    /// <param name="speed"> Rotation Speed of the Enemy. </param>
    public IEnumerator rotateCoroutine(float rotation, float speed)
    {
        float totalRotation = 0;

        while (totalRotation < rotation)
        {
            totalRotation += speed;
            gameObject.transform.Rotate(new Vector3(0f, speed, 0f));
            yield return 0;
        }
    }

    public void takeDamage(int damage, GameObject hitPoint)
    {
        if (!invincible)
        {
            StartCoroutine(wasHit());
            HP -= damage;
            if (HP <=0)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                StartCoroutine(Die());
                dead = true;
                //Destroy(gameObject.GetComponent<CharacterController>());
                Destroy(gameObject.GetComponentInChildren<Animator>());

                foreach (GameObject g in dismemberable)
                {
                    if (hitPoint.Equals(g))
                    {
                        hitPoint.AddComponent<DelayedDeath>();
                        hitPoint.transform.SetParent(null);
                        hitPoint.AddComponent<Rigidbody>();
                    }
                }
            }
            //Destroy(gameObject);
        }
        else if (HP <= 0)
        {
            foreach (GameObject g in dismemberable)
            {
                if (hitPoint.Equals(g))
                {
                    hitPoint.AddComponent<DelayedDeath>();
                    hitPoint.transform.SetParent(null);
                    hitPoint.AddComponent<Rigidbody>();
                }
            }
        }
    }

    IEnumerator wasHit()
    {
        invincible = true;
        yield return new WaitForSeconds(iFrames);
        invincible = false;
    }

    void lookAtPlayer()
    {
        this.transform.LookAt(player.transform);
    }

    void approachPlayer()
    {
        moveUpdate(Vector3.forward * 0.1f, 0.1f);
    }

    float distToPlayer()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        return dir.magnitude;
    }

    void strafe()
    {
        if (!strafeB)
        {
            strafeDir = Random.Range(-1, 1);
            StartCoroutine(pickStrafe());
        }
        if (strafeDir >= 0)
        {
            moveUpdate(Vector3.left * 0.2f, 0.05f);
        }
        else
        {
            moveUpdate(Vector3.right * 0.2f, 0.05f);

        }
    }

    void enemyLogic()
    {
        //int dec = aI.decision(player.transform.position, player.GetComponent<ComboManager>().attacking, pastDecisions[1], pastDecisions[2], pastDecisions[3]);
        if (!isAttacking)
        {
            if (distToPlayer() > 6 && player.GetComponent<ComboManager>().attacking)
            {
                lookAtPlayer();
                approachPlayer();
                strafe();
                cancelattack();
            }
            else if (distToPlayer() > 6.5)
            {
                cancelattack();
                lookAtPlayer();
                approachPlayer();
            }
            else if (player.GetComponent<ComboManager>().attacking)
            {
                lookAtPlayer();
                strafe();
                attack();
            }
            else if (distToPlayer() <= 6.5)
            {
                approachPlayer();
                attack();
            }
            else
            {
                lookAtPlayer();
                attack();
                animator.SetFloat("SpeedX", 0);
                animator.SetFloat("SpeedY", 0);
            }
        }
    }

    IEnumerator pickStrafe()
    {
        strafeB = true;
        yield return new WaitForSeconds(2);
        strafeB = false;
    }

    IEnumerator Die()
    {
        if (dead == false)
        {
            drop();
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }

    void drop()
    {
        int roll = Random.Range(0, 100);
        double lowerBound = 0;
        double upperBound = 0;
        print(roll);
        for(int i = 0; i < dropRates.Length; i++)
        {
            lowerBound = upperBound;
            upperBound += dropRates[i];
            if(roll >= lowerBound && roll < upperBound)
            {
                GameObject localDrop = Instantiate(drops[i], transform.position, Quaternion.identity);
                localDrop.AddComponent<Collectible>();
                localDrop.GetComponentInChildren<Collider>().isTrigger = true;
            }
        }
    }
}
