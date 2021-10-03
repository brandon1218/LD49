using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  

    public enum EnemyEnum
    {
        IdleState,
        Patrol,
        MovingToPlayer,
        Attacking,
    }
    public EnemyEnum this_EnemyEnum;
    public float idleTimer, patrilTimer, movingToPlayerTimer, attackingTimer;

    public Vector3 movingTargetPos;
    public float move_speed;
    public GameObject nearestEnemy;

    Animator animator;
    AudioManager audioManager;

    bool fall;
    private void Awake()
    {
        this_EnemyEnum = EnemyEnum.IdleState;
        idleTimer = Random.Range(-1.0f, 2.0f);
        //animator = GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    bool dash;
    private void Update()
    {
        CheckNearestEnemy();

        if (nearestEnemy == null)
        {
            return;
        }

        switch (this_EnemyEnum)
        {
            case EnemyEnum.IdleState:
                idleTimer += Time.deltaTime;
                if (idleTimer > 2)
                {
                    this_EnemyEnum = EnemyEnum.Patrol;
                    movingTargetPos = transform.position + new Vector3(Random.Range(-1.0f,1.0f), 0, Random.Range(-1.0f,1.0f)).normalized * 3;
                    idleTimer = 0;
                }
              //  animator.SetFloat("speed", 0);
                SetTargetSpeed(0);
                break;
            case EnemyEnum.Patrol:

                if (Vector3.Distance(transform.position,movingTargetPos)>0.5f)
                {
                    transform.position += (movingTargetPos - transform.position).normalized * Time.deltaTime * move_speed;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movingTargetPos - transform.position), 0.1f);
                    //animator.SetFloat("speed", 0.5f);
                    SetTargetSpeed(0.5f);

                    if (Vector3.Distance(transform.position,nearestEnemy.transform.position)< 5)
                    {
                        this_EnemyEnum = EnemyEnum.Attacking;
                    }

                }
                else
                {
                    if (Vector3.Distance(transform.position,nearestEnemy.transform.position)<2)
                    {
                        this_EnemyEnum = EnemyEnum.Attacking;
                    }
                    else
                    {
                        this_EnemyEnum = EnemyEnum.IdleState;
                    }

                }



                break;
            case EnemyEnum.MovingToPlayer:
                break;
            case EnemyEnum.Attacking:
                attackingTimer += Time.deltaTime;
                if (attackingTimer >0 && attackingTimer <1)
                {
                    transform.position += (nearestEnemy.transform.position - transform.position).normalized * Time.deltaTime * move_speed;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nearestEnemy.transform.position - transform.position), 0.1f);
                    //animator.SetFloat("speed", 0.5f);
                    SetTargetSpeed(0.5f);
                }
                else if (attackingTimer >1 )
                {
                    transform.position += (nearestEnemy.transform.position - transform.position).normalized * move_speed * 5 * Time.deltaTime;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nearestEnemy.transform.position - transform.position), 0.1f);

                    if (!dash)
                    {
                        dash = true;
                        AudioSource.PlayClipAtPoint(audioManager.dashAudioClip, transform.position, 1);
                    }

                    SetTargetSpeed(1);
                    if (attackingTimer > 2f)
                    {
                        attackingTimer = -2;
                        SetTargetSpeed(0);
                        dash = false;
                    }
                }
                else if (attackingTimer < 0)
                {
                    SetTargetSpeed(0);

                    if (Vector3.Distance(transform.position,nearestEnemy.transform.position)<2)
                    {
                        // 
                    }
                    else
                    {
                        // enter  patrol state
                    //    this_EnemyEnum = EnemyEnum.Patrol;
                      //  movingTargetPos = transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * 3;
                    }
                }


                break;
            default:
                break;
        }


        if (transform.position.y < -15f)
        {
            Destroy(gameObject);
            Debug.Log("Killed");
        }
        if (transform.position.y < -0)
        {
            if (!fall)
            {
                fall = true;
                animator.SetTrigger("fall");
                AudioSource.PlayClipAtPoint(audioManager.characterDropAudioClip, transform.position, 1);
                AudioSource.PlayClipAtPoint(audioManager.manFallClips[Random.Range(0,audioManager.manFallClips.Length)], transform.position, 1);
            }
        }

    }

    public GameObject [] enemies;

    void CheckNearestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Character");
        if (enemies.Length ==1)
        {
            nearestEnemy = null;
            return;
        }

        nearestEnemy = enemies[0];
        if (nearestEnemy == gameObject)
        {
            nearestEnemy = enemies[1];
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position,enemies[i].transform.position) < Vector3.Distance(transform.position,nearestEnemy.transform.position))
            {
                if (enemies[i] != gameObject)
                {
                    nearestEnemy = enemies[i];
                }

            }
        }


    }


    void SetTargetSpeed(float targetSpeed )
    {
        animator.SetFloat("speed", Mathf.Lerp(animator.GetFloat("speed"), targetSpeed, 0.1f));
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(movingTargetPos, 0.2f);
    }
    public void Hitted()
    {
        animator.SetTrigger("hitted");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (this_EnemyEnum == EnemyEnum.Attacking)
            {
                if (attackingTimer >0)
                {
                    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 300 * Time.deltaTime + (other.transform.position - transform.position).normalized * 450 * Time.deltaTime;
                    if (other.gameObject.GetComponent<Enemy>())
                    {
                       other.gameObject.GetComponent<Enemy>().Hitted();
                    }

                    if (other.gameObject.GetComponent<Player>())
                    {
                        other.gameObject.GetComponent<Player>().Hitted();
                    }

                    AudioSource.PlayClipAtPoint(audioManager.hitAudioClip, transform.position, 1);
                    SetTargetSpeed(0);
                }
            }
            attackingTimer = -2;

            if (other.gameObject.GetComponent<Player>())
            {
                if (other.gameObject.GetComponent<Player>().attacking)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.up * 300 * Time.deltaTime + (transform.position - other.transform.position).normalized * 450 * Time.deltaTime;
                    AudioSource.PlayClipAtPoint(audioManager.hitAudioClip, transform.position, 1);
                    Hitted();
                    
                }
            }


        }
    }

}
