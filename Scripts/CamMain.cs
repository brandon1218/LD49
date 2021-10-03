using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMain : MonoBehaviour
{
    Player player;
    Vector3 offset;

    private void Awake()
    {
        player = FindObjectOfType<Player>();

        offset = transform.position - player.transform.position;

    }
    private void Update()
    {
        if (player.transform.position.y > -3)
        {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, 0.02f);

        }


    }




}
