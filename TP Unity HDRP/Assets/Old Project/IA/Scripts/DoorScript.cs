using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    public Canvas needKeyCanvas;
    public bool haveKey = false;
    public Canvas loseCanvas;
    public Canvas wonCanvas;

    public AudioSource audioS1;
    public AudioSource audioS2;
    public AudioClip musicWon;
    public AudioClip wonSound;
    public AudioClip keySound;
    public AudioClip defeatSound;

    bool defeatSoundPlay = true;
    bool audioEnd = false;
    bool end = false;
    bool againAbool = true;

    private void Update()
    {
        if (!audioEnd && audioS2.isPlaying == false && end)
        {
            audioEnd = true;
            GameObject.Find("Camera").GetComponent<AudioSource>().enabled = false;
            audioS1.PlayOneShot(wonSound);
        }
    }

    public void PlayKeyPickupSound()
    {
        audioS1.PlayOneShot(keySound);
    }

    public void PlayDefeatSound()
    {
        if (defeatSoundPlay)
        {
            defeatSoundPlay = false;
            audioS2.volume = 0.15f;
            GameObject.Find("Camera").GetComponent<AudioSource>().enabled = false;
            audioS2.PlayOneShot(defeatSound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!haveKey)
                needKeyCanvas.gameObject.SetActive(true);
            else if (haveKey && againAbool)
            {
                againAbool = false;
                end = true;
                audioS2.volume = 1f;
                audioS2.PlayOneShot(musicWon);
                wonCanvas.gameObject.SetActive(true);
            }
        }
    }

    public void PlayAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
