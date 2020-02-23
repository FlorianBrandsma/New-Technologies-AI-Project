using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private bool started;
    static public bool gameFinished;
    private bool finishFinished; //Nice naming

    private float articifialDelay = 1;

    public RawImage fadeImage;
    public RectTransform testToast;

    private List<GameObject> drakes;

    static public GameManager gameManager;

    static public Warrior warrior;

    public SkullManager skullManager;
    public Letterbox letterbox;

    private void Awake()
    {
        gameManager = this;

        drakes = GameObject.FindGameObjectsWithTag("Drake").ToList();
    }

    private void StartGame()
    {
        StopAllCoroutines();

        started = true;
        StartCoroutine(Fade(fadeImage, 0, 2));
    }

    public IEnumerator FinishGame()
    {
        gameFinished = true;

        TouchControls.LockControls();

        warrior.Agent.destination = new Vector3(-15, -1.5f, 170);

        //Small velocity boost to trigger the walking animation
        warrior.Agent.velocity = new Vector3(0f, 0f, 0.01f);

        yield return new WaitForSeconds(2);

        StartCoroutine(Fade(fadeImage, 0.75f, 2));

        yield return new WaitForSeconds(2);

        skullManager.RectTransform.anchoredPosition = new Vector2((Screen.width / 2), Screen.height / 2);

        finishFinished = true;
    }

    public IEnumerator ResetGame()
    {
        finishFinished = false;

        TouchControls.LockControls();

        StartCoroutine(Fade(fadeImage, 1f, 1));

        yield return new WaitForSeconds(1);

        letterbox.Deactivate();

        yield return new WaitForSeconds(1);
        
        gameFinished = false;
        TouchControls.controlsLocked = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (articifialDelay > 0)
            articifialDelay -= 1 * Time.deltaTime;
        else if (!started)
            StartGame();

        if (!finishFinished) return;

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(ResetGame());
        }
    }

    public IEnumerator Fade(RawImage fadeObject, float aValue, float aTime)
    {
        float alpha = fadeObject.color.a;

        if (aValue == 0)
            alpha = 1f;

        for (float t = 0.0f; t < 1.25f; t += Time.deltaTime / (aTime * Time.timeScale))
        {
            Color newColor = new Color(fadeObject.color.r, fadeObject.color.g, fadeObject.color.b, Mathf.Lerp(alpha, aValue, t));

            fadeObject.color = newColor;

            yield return null;
        }
    }
}
