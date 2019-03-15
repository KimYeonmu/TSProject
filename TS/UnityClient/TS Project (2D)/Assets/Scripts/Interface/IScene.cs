using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class IScene : MonoBehaviour
{
    public CanvasGroup SceneCanvasGroup;
    public RectTransform SceneTransform;
    public GameObject RootObject;

    public virtual void Awake()
    {
        SceneSystem.GetInstance().NowScene = this;
        SceneSystem.GetInstance().RootObject = RootObject;
        SceneSystem.GetInstance().SceneUi = SceneTransform;
        SceneSystem.GetInstance().SceneUiGroup = SceneCanvasGroup;
    }

    public abstract IEnumerator NextSceneAnimation();
}

