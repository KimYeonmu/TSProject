using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : IScene
{
    public Sprite ButtonSelectSprite;

    public GameObject[] TopButton;
    public GameObject[] MiddleBtn;
    public GameObject[] GameBtn;
    public GameObject BottomBtn;

    public GameObject Wall2;

    private AsyncOperation _async;

	// Use this for initialization
	void Start ()
	{
        SetCanvasAlpha(BottomBtn,0);

	    for (int i = 0; i < GameBtn.Length; i++)
	    {
            GameBtn[i].SetActive(false);
	        SetCanvasAlpha(GameBtn[i],0);
	    }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(BottomBtn.transform.position.y > Wall2.transform.position.y)
            Wall2.SetActive(true);
    }

    public void ClickTopBtn(int btnIndex)
    {
        for (int i = 0; i < TopButton.Length; i++)
        {
            Image changeimage = TopButton[i].GetComponent<Image>();

            if (i != btnIndex)
            {
                changeimage.color = new Color32(179,179,179,255);
            }
            else
            {
                changeimage.color = Color.black;
            }
        }

        switch (btnIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    public void ClickMiddleBtn(int btnIndex)
    {
        for (int i = 0; i < MiddleBtn.Length; i++)
        {
            Image changeimage = MiddleBtn[i].GetComponent<Image>();
            Text txt = MiddleBtn[i].GetComponentInChildren<Text>();

            if (i != btnIndex)
            {
                changeimage.sprite = null;
                txt.color = Color.black;
            }
            else
            {
                changeimage.sprite = ButtonSelectSprite;
                txt.color = Color.white;
            }
        }

        for (int i = 0; i < GameBtn.Length; i++)
        {
            GameBtn[i].SetActive(false);
            SetCanvasAlpha(GameBtn[i], 0);
        }

        switch (btnIndex)
        {
            case 0:
                GameBtn[0].SetActive(true);
                FadeOutCanvas(GameBtn[0]);                
                break;
            case 1:
                
                break;
            case 2:
                break;
        }
    }

    public void ClickGameBtn(int btnIndex)
    {
        if (Wall2.activeSelf == true)
            return;
            
        Wall2.SetActive(false);

        FadeOutCanvas(BottomBtn);

        Rigidbody2D body = BottomBtn.GetComponent<Rigidbody2D>();
        body.AddForce(Vector2.up * 80, ForceMode2D.Impulse);

        Destroy(Camera.main.GetComponent<AudioListener>());

        StartCoroutine(LoadNewScene());
    }

    public void SetCanvasAlpha(GameObject obj, float alpha)
    {
        Image image = obj.GetComponent<Image>();
        image.canvasRenderer.SetAlpha(alpha);

        Text[] text = obj.GetComponentsInChildren<Text>();

        for (int i = 0; i < text.Length; i++)
        {
            text[i].canvasRenderer.SetAlpha(alpha);
        }
    }

    public void FadeOutCanvas(GameObject obj)
    {
        Image image = obj.GetComponent<Image>();

        image.canvasRenderer.SetAlpha(0);
        image.CrossFadeAlpha(1, 0.5f, false);

        Text[] text = obj.GetComponentsInChildren<Text>();

        for (int i = 0; i < text.Length; i++)
        {
            text[i].canvasRenderer.SetAlpha(0);
            text[i].CrossFadeAlpha(1, 0.5f, false);
        }
    }

    public IEnumerator LoadNewScene()
    {
        WaitForSeconds delay = new WaitForSeconds(5);
        _async = SceneManager.LoadSceneAsync("Assets/Scene/PlayScene.unity",LoadSceneMode.Additive);

        while (!_async.isDone)
        {
            yield return delay;
        }

        CardSystem.GetInstance().AddAllCard();

        yield return delay;

        Rigidbody2D body = BottomBtn.GetComponent<Rigidbody2D>();
        body.AddForce(Vector2.up * 200, ForceMode2D.Impulse);

        yield return StartCoroutine(GameManager.GetInstance().StartScene());
    }
}