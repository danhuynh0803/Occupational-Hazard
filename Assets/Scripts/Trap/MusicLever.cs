using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLever : MonoBehaviour {

    private AudioSource audio;
    private SpriteRenderer renderer;
    private bool isPlayingSong;
    private bool isInteractable;
    public Sprite offSprite;
    public Sprite OnSprite;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isPlayingSong)
                {
                    audio.Pause();
                    renderer.sprite = offSprite;
                    isPlayingSong = !isPlayingSong;
                }
                else
                {
                    isPlayingSong = !isPlayingSong;
                    audio.Play();
                    renderer.sprite = OnSprite;
                }
            }
        }

        if(GameController.isGameOver)
        {
            audio.Stop();
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isInteractable = false;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        isInteractable = true;
    }
}
