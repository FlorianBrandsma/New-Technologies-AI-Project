using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skull : MonoBehaviour
{
    public enum Colour
    {
        Bronze,
        Silver,
        Gold,
        Diamond
    }

    public Texture icon;

    public Colour colour;

    public bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.warrior.gameObject)
        {
            gameObject.SetActive(false);

            GameManager.gameManager.skullManager.CollectSkull(this);
        }
    }

    public void ResetSkull()
    {
        collected = false;
        gameObject.SetActive(true);
    }
}
