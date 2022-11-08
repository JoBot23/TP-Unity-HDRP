using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canard : MonoBehaviour
{
    public float MinSoundDelay = 1;
    public float RandomDelay = 4;
    private float NextSoundDelay = 0;
    private AudioSource aSource;

    [Range(0.0f, 5.0f)]
    public float vitesseAvance = 2.5f;
    [Range(0.0f, 360.0f)]
    public float vitesseTourne = 20;

    public AudioClip c1;
    public AudioClip c2;
    public AudioClip c3;
    public List<AudioClip> aClips = new List<AudioClip>();

    private void computeNextSoundDelay()
    {
        NextSoundDelay = MinSoundDelay + Random.Range(0, RandomDelay);
    }

    void Start()
    {
        StartCoroutine(enableReverb());
        aClips.Add(c1);
        aClips.Add(c2);
        aClips.Add(c3);

        aSource = GetComponent<AudioSource>();
        computeNextSoundDelay();
    }

    void Update()
    {
        NextSoundDelay -= Time.deltaTime;
        if (NextSoundDelay <= 0)
        {
            aSource.pitch = Random.Range(0.9f, 1.5f);
            //aSource.clip = aClips[Random.Range(0, 2)];
            aSource.Play();
            computeNextSoundDelay();
        }

        //Faire des ronds dans l'eau
        transform.position += transform.forward * Time.deltaTime * vitesseAvance;
        transform.rotation = Quaternion.AngleAxis(vitesseTourne * Time.deltaTime, Vector3.up) * transform.rotation;
    }

    IEnumerator enableReverb()
    {   
        yield return new WaitForSeconds(14.6f);
        GetComponent<AudioReverbFilter>().enabled = true;
        StartCoroutine(disableReverb());
    }

    IEnumerator disableReverb()
    {
        yield return new WaitForSeconds(3.5f);
        GetComponent<AudioReverbFilter>().enabled = false;
        StartCoroutine(enableReverb());
    }
}
