using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float horizontal_input;
    public float vertical_input;

    Vector3 input_value;

    public float move_speed;

    Animator animator;
    bool fall;
    AudioManager audioManager;

    public bool attacking;
    float attackingTimer;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }


    private void Update()
    {
        horizontal_input = Input.GetAxis("Horizontal");
        vertical_input = Input.GetAxis("Vertical");

        input_value = new Vector3(horizontal_input, 0, vertical_input);
        if (input_value.magnitude >=1)
        {
            input_value = input_value.normalized;
        }

        

        if (transform.position.y <=0)
        {
            if (!fall)
            {
                fall = true;
                AudioSource.PlayClipAtPoint(audioManager.characterDropAudioClip, transform.position, 1);
                animator.SetTrigger("fall");
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            attacking = true;
            attackingTimer = 0.3f;
            AudioSource.PlayClipAtPoint(audioManager.dashAudioClip, transform.position, 1);
        }

        if (attacking)
        {
            transform.position += transform.forward * move_speed * Time.deltaTime * 1.5f;
            animator.SetFloat("speed", Mathf.Lerp(animator.GetFloat("speed"), 1, 0.1f));

            attackingTimer -= Time.deltaTime;
            if (attackingTimer <=0)
            {
                attacking = false;
            }
        }
        else
        {
            if (input_value.magnitude > 0.2f)
            {
                animator.SetFloat("speed", Mathf.Lerp(animator.GetFloat("speed"), 0.5f, 0.1f));
                transform.position += input_value * move_speed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(input_value), 0.1f);
            }
            else
            {
                animator.SetFloat("speed", Mathf.Lerp(animator.GetFloat("speed"), 0, 0.1f));
            }
        }
    }

    public void Hitted()
    {
        animator.SetTrigger("hitted");
    }


    

}
