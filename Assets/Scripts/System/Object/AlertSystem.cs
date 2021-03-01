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
        AlertScale = Vector2.one;
        _alertSprite = Resources.LoadAll("Sprite/Alert",typeof(Sprite));
    }

    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var alert = ResourceManager.GetInstance().Load("Prefabs/Object/Alert").GetComponent<Alert>();
            alert.SetSprite(_alertSprite[i] as Sprite);
            alert.SetParent(transform);
            alert.SetScale(AlertScale);
            alert.AlertShapeTag = (ShapeTag) (i%4);
        
            if (i < 4)
            {
                alert.AlertCardTag = CardTag.N2;
            }
            else if (i < 8)
            {
                alert.AlertCardTag = CardTag.A;
            }
            else
            {
                alert.AlertCardTag = CardTag.Joker + i%8 * 10;
            }
        
            alert.gameObject.SetActive(false);
        
            Alerts.Add(alert);
        }
    }

    public void AddAlerts(CardTag cardIndex, ShapeTag shapeIndex)
    {
        for (int i = 0; i < Alerts.Count; i++)
        {
            if (Alerts[i].AlertCardTag == cardIndex &&
                Alerts[i].AlertShapeTag == shapeIndex)
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
        var vc = Vector2.zero;

        for (int i = 0; i < ActiveAlerts.Count; i++)
        {
            vc.x =  (i - (float)ActiveAlerts.Count/2) + 0.5f;
            vc.y = 2.5f;
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