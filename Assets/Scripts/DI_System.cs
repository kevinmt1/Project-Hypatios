using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DI_System : MonoBehaviour
{

    [SerializeField] private DamageIndicator damageIndicator;
    [SerializeField] private RectTransform holder;
    public new Camera camera;
    public Transform player;

    private Dictionary<Transform, DamageIndicator> Indicators = new Dictionary<Transform, DamageIndicator>();

    #region Delegates
    public static Action<Transform> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInsight;
    #endregion

    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInsight += Insight;
    }

    private void OnDisable()
    {
        CreateIndicator -= Create;
        CheckIfObjectInsight -= Insight;
    }

    void Create(Transform target)
    {
        if (Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }
        DamageIndicator newIndicator = Instantiate(damageIndicator, holder);
        newIndicator.Register(target, player, new Action( () => { Indicators.Remove(target); }));

        Indicators.Add(target, newIndicator);
    }

    bool Insight(Transform target)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
