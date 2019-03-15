using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AlertSystem : SingletonBase<AlertSystem>
{
    [HideInInspector] public Vector2 AlertScale;
    [HideInInspector] public List<Alert> Alerts;
    [HideInInspector] public List<Alert> ActiveAlerts;

    private object[] _alertSprite;

    public void Awake()
    {
        _alertSprite = Resources.LoadAll("Sprite/Alert",typeof(Sprite));
    }

    public void Start()
    {
        //Alert alert;
        //
        //for (int i = 0; i < 10; i++)
        //{
        //    alert = gameObject.AddComponent<Alert>();
        //    alert.SetSprite(_alertSprite[i] as Sprite);
        //    alert.SetParent(transform);
        //    alert.SetScale(AlertScale);
        //    alert.AlertShapeTag = (ShapeTag) (i%4);
        //
        //    if (i < 4)
        //    {
        //        alert.AlertCardTag = CardTag.N2;
        //    }
        //    else if (i < 8)
        //    {
        //        alert.AlertCardTag = CardTag.A;
        //    }
        //    else
        //    {
        //        alert.AlertCardTag = CardTag.Joker + i%8 * 10;
        //    }
        //
        //    alert.gameObject.SetActive(false);
        //
        //    Alerts.Add(alert);
        //}
    }

    public void AddAlerts(Card card)
    {
        for (int i = 0; i < Alerts.Count; i++)
        {
            if (Alerts[i].AlertCardTag == card.GetCardIndex() &&
                Alerts[i].AlertShapeTag == card.GetShapeIndex())
            {
                ActiveAlerts.Add(Alerts[i]);
                Alerts[i].gameObject.SetActive(true);
                RepositionAlerts();
                break;
            }
        }
    }

    public void RepositionAlerts()
    {
        Vector2 vc = Vector2.zero;

        for (int i = 0; i < ActiveAlerts.Count; i++)
        {
            vc.x =  (i - (float)ActiveAlerts.Count/2) + 0.5f;
            vc.y = 5.5f;
            ActiveAlerts[i].SetPosition(vc);
        }
    }

    public void ClearAlert()
    {
        for (int i = 0; i < ActiveAlerts.Count; i++)
        {
            ActiveAlerts[i].gameObject.SetActive(false);
        }

        ActiveAlerts.Clear();
    }
}