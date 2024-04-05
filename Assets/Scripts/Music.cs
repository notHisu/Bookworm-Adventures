using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource src;
    public AudioClip button,
        playerAttk,
        ememyAttak,
        background;
    public AudioSource backgroudAudio;

    private void Start()
    {
        backgroudAudio.clip = background;
        backgroudAudio.Play();
    }

    // Start is called before the first frame update
    public void buttonAudio()
    {
        src.clip = button;
        src.Play();
    }

    public void PlayerAttck()
    {
        src.clip = playerAttk;
        src.Play();
    }

    public void EnemyAttck()
    {
        src.clip = ememyAttak;
        src.Play();
    }
}
