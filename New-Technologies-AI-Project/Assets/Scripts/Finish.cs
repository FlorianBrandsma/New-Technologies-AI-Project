using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.warrior.gameObject)
        {
            StartCoroutine(GameManager.gameManager.FinishGame());

            GameManager.gameManager.letterbox.Activate();
        }
    }
}
