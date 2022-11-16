using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject keyPickupText;
    bool picked = false;
    public void OnPickedUp()
    {
        if(!picked)
        {
            picked = true;
            PatrickController.instance.keys++;
            print(PatrickController.instance.keys);
            if(PatrickController.instance.keys >= 3)
            {
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                PatrickController.instance.Win();
            }
            else
            {
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                keyPickupText.gameObject.SetActive(true);
                StartCoroutine(HideKeyText());
            }
        }
    }

    IEnumerator HideKeyText()
    {
        yield return new WaitForSeconds(4f);
        keyPickupText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
