using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private const float maxTime = 3f;
    float time = maxTime;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }


    private RectTransform rect = null;
    protected RectTransform Rect
    {
        get
        {
            if (rect = null)
            {
                rect = GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }

    public Transform Target { get; protected set; } = null;

    public Transform player = null;

    private IEnumerator RemoveIndicator = null;
    private Action unregistered = null;

    Quaternion targetRot = Quaternion.identity;
    Vector3 targetPos = Vector3.zero;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Restart()
    {
        time = maxTime;
        startTimer();
    }

    public void Register(Transform target, Transform player, Action unregistered)
    {
        this.Target = target;
        this.player = player;
        this.unregistered = unregistered;

        StartCoroutine(Rotate());
        startTimer();
    }

    void startTimer()
    {
        if (RemoveIndicator != null)
        {
            StopCoroutine(RemoveIndicator);
        }
        RemoveIndicator = Countdown();
        StartCoroutine(RemoveIndicator);
    }

    IEnumerator Countdown()
    {
        while(CanvasGroup.alpha < 1f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        while(time > 0)
        {
            time--;
            yield return new WaitForSeconds(1);
        }
        while(CanvasGroup.alpha > 0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        unregistered();
        Destroy(gameObject);
    }

    IEnumerator Rotate()
    {
        while (enabled)
        {
            if (Target)
            {
                targetPos = Target.position;
                targetRot = Target.rotation;
            }
            Vector3 dir = player.position - targetPos;

            targetRot = Quaternion.LookRotation(dir);
            targetRot.z = -targetRot.y;
            targetRot.x = 0;
            targetRot.y = 0;

            Vector3 northDir = new Vector3(0f, 0f, player.eulerAngles.y);
            Rect.localRotation = targetRot * Quaternion.Euler(northDir);

            yield return null;
        }
    }
}
