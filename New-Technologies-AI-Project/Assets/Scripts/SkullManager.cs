using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkullManager : MonoBehaviour
{
    private Vector2 startAnchor;

    public Texture defaultIcon;

    public List<RawImage> skullIcons;
    public List<Skull> skulls;

    private float bottomValue = -75f;
    private float topValue = 75f;

    private float targetValue;

    bool toasted = false;

    private float maxUptime = 3;
    private float uptime;

    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

    private void Awake()
    {
        startAnchor = RectTransform.anchoredPosition;

        targetValue = RectTransform.anchoredPosition.y;
        uptime = maxUptime;
    }

    public void CollectSkull(Skull skull)
    {
        var index = (int)skull.colour;

        skullIcons[index].texture = skull.icon;

        skull.collected = true;

        targetValue = topValue;
        uptime = maxUptime;
    }
    
    private void Update()
    {
        if (GameManager.gameFinished) return;

        RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, Mathf.Lerp(RectTransform.anchoredPosition.y, targetValue, 0.1f));

        if (uptime <= 0)
            targetValue = bottomValue;
        else
            uptime -= 1 * Time.deltaTime;
    }
}
