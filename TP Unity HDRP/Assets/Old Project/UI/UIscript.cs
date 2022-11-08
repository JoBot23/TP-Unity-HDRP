using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIscript : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject animationButtons;

    public void loadObjet()
    {
        SceneManager.LoadScene("Scenes 1/Perso");

    }

    public void loadEnviro()
    {
        SceneManager.LoadScene("Scenes 1/Environnement");
    }

    public void loadBoids()
    {
        SceneManager.LoadScene("Scenes 1/Boids");
    }

    public void loadAudio()
    {
        SceneManager.LoadScene("Scenes/Audio");
    }

    public void loadController()
    {
        SceneManager.LoadScene("Scenes/Controller");
    }
    
    public void showAnimation()
    {
        mainButtons.SetActive(false);
        animationButtons.SetActive(true);
    }

    public void loadInfiltration()
    {
        SceneManager.LoadScene("Scenes/IA");
    }

    //Animation
    public void loadAscenseur()
    {
        SceneManager.LoadScene("Scenes/Animations");
    }

    public void loadPatrick()
    {
        SceneManager.LoadScene("Scenes/Patrick");
    }
}
