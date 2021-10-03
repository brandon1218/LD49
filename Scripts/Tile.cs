using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Drop(float delay)
    {
        Invoke("DropDown", delay);

    }


    void DropDown()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, 3);
        AudioSource.PlayClipAtPoint(audioManager.dropAudioClip, transform.position, 1);
    }


}
