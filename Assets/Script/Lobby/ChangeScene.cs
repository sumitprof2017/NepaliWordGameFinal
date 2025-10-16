using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    GameObject PlayButton_b;
    public AudioSource audioSource_play;
    public AudioClip audioClip_play;
    public GameObject bg;
    void Start()
    {
        PlayButton_b = GameObject.Find("PlayButton");
        PlayButton_b.GetComponent<Button>().onClick.AddListener(PlaySound);
        bg.GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void ChangeSceneToScrable()
    {
        
        SceneManager.LoadScene("ReshuffleForSallu");
    }
    void PlaySound()
    {
        audioSource_play.PlayOneShot(audioClip_play);
        ChangeSceneToScrable();
    }
}
