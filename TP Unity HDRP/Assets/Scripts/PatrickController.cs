using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PatrickController : MonoBehaviour
{
    public static PatrickController instance;
    [HideInInspector] public AudioSource audio;
    [SerializeField] PlayerController player;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AISight sightSense;
    public List<Path> patrolPaths = new List<Path>();
    public Path path;
    public Animator animator;
    public GameObject target;

    [Header("UI")]
    [SerializeField] Image winImg;
    [SerializeField] Image deathScreen;
    [SerializeField] Image deathText;
    [SerializeField] Button playAgainButton;
    [SerializeField] Button quitButton;
    [SerializeField] float fadeSpeed;

    [Header("Parameters")]
    public float patrollingSpeed;
    public float chasingSpeed;
    public float throughPatrolSpeed;

    [Header("Hearing")]
    [SerializeField] float hearingRangeRunning;
    [SerializeField] float hearingRange;
    [SerializeField] float hearingRangeCrouch;

    [Header("Booleans")]
    public bool chasing;
    public bool canSeePlayer;
    public bool canHearPlayer;
    public bool patrolling = true;
    public float hitCooldown = 1f;
    private float speedBackup;
    bool end = false;
    public bool win = false;

    [Header("Audio")]
    public AudioSource chaseMusic;
    public AudioClip spottedSound;
    public AudioClip[] hitSounds;
    public AudioClip[] catchSounds;
    public AudioClip[] whistleSounds;
    public AudioClip winSound;

    [HideInInspector] public float keys = 0;

    void Awake() 
    {
        instance = this;
        agent = GetComponent<NavMeshAgent>();
        sightSense = GetComponent<AISight>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Sight
        if(!canSeePlayer && PlayerDetected())
        {
            canSeePlayer = true;
            target = PlayerDetected();
        }
        else if(canSeePlayer && !PlayerDetected())
        {
            canSeePlayer = false;
            target = null;
        }

        //Hearing
        if(Vector3.Distance(transform.position, player.transform.position) < hearingRangeRunning && player.isSprinting)
        {
            canHearPlayer = true;
        }
        else if(Vector3.Distance(transform.position, player.transform.position) < hearingRange && !player.isCrouching)
        {
            canHearPlayer = true;
        }
        else if(Vector3.Distance(transform.position, player.gameObject.transform.position) < hearingRangeCrouch)
        {
            canHearPlayer = true;
        }
        else canHearPlayer = false;

        //Death
        if(chasing && Vector3.Distance(transform.position, player.transform.position) < 1.5f && !end && !win)
        {
            agent.Stop();
            player.canMove = false;
            audio.Stop();
            chaseMusic.enabled = false;
            player.GetComponent<AudioSource>().Stop();
            audio.PlayOneShot(catchSounds[Random.Range(0,2)]);

            end = true;
            deathScreen.DOFade(1, 1);
            StartCoroutine(ShowDeathButtons());
        }
    }

    public void PatrollingSight()
    {
        sightSense.sightAngle = 70;
        sightSense.sightRange = 14;
        sightSense.OnValidate();
    }

    public void ChasingSight()
    {
        sightSense.sightAngle = 90;
        sightSense.sightRange = 16;
        sightSense.OnValidate();
    }

    private GameObject PlayerDetected()
    {
        if(sightSense.objects.Count > 0)
            return sightSense.objects[0];
        if(canHearPlayer)
            return player.gameObject;
        
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, hearingRangeRunning);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, hearingRangeCrouch);
    }

    public IEnumerator PatrickHit()
    {
        if(hitCooldown == 1)
        {
            audio.PlayOneShot(hitSounds[Random.Range(0,2)]);

            hitCooldown = 0;
            animator.SetTrigger("HitBySomething");
            canHearPlayer = true;

            agent.speed = agent.speed * 0.5f;
            animator.SetFloat("SlowMul", 0.6f);

            yield return new WaitForSeconds(1.2f); 

            if(chasing) agent.speed = chasingSpeed;
            else agent.speed = patrollingSpeed;

            animator.SetFloat("SlowMul", 1f);
            hitCooldown = 1;
        }
    }

    public void ChangePatrolPath()
    {
        //Find closest path to player 
        float distPath = 5000f;
        foreach(Path p in patrolPaths)
        {
            var dist = Vector3.Distance(p.gameObject.transform.position, player.transform.position);
            if(dist < distPath) 
            {
                distPath = dist;
                path = p;
            }
        }
    }

    IEnumerator ShowDeathButtons()
    {
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        deathText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        quitButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PatrickWhistle()
    {
        if(!audio.isPlaying && Random.Range(0,100) > 33)
        {
            audio.PlayOneShot(whistleSounds[Random.Range(0,2)]);
        }
    }

    public void Win()
    {
        agent.Stop();
        audio.Stop();
        player.GetComponent<AudioSource>().Stop();

        chaseMusic.enabled = true;
        chaseMusic.PlayOneShot(winSound);

        player.canMove = false;
        audio.Stop();

        win = true;
        end = true;
        deathScreen.DOFade(1, 1);
        StartCoroutine(ShowWinButton());
    }

    IEnumerator ShowWinButton()
    {
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        winImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        quitButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
    }

}
