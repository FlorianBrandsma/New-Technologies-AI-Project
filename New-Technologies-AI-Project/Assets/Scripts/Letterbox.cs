using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterbox : MonoBehaviour
{
    public RectTransform topBar;
    public RectTransform bottomBar;

    private float topStart;
    private float bottomStart;

    private float topTarget;
    private float bottomTarget;

    private void Awake()
    {
        topStart = topBar.anchoredPosition.y;
        bottomStart = bottomBar.anchoredPosition.y;

        topTarget = topStart;
        bottomTarget = bottomStart;
    }

    private void Update()
    {
        topBar.anchoredPosition = new Vector2(topBar.anchoredPosition.x, Mathf.Lerp(topBar.anchoredPosition.y, topTarget, 0.1f));
        bottomBar.anchoredPosition = new Vector2(bottomBar.anchoredPosition.x, Mathf.Lerp(bottomBar.anchoredPosition.y, bottomTarget, 0.1f));
    }

    public void Activate()
    {
        topTarget = -topStart;
        bottomTarget = -bottomStart;
    }

    public void Deactivate()
    {
        topTarget = topStart;
        bottomTarget = bottomStart;
    }
}
