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

	// Use this for initialization
	void Start ()
	{
        Screen.SetResolution(720, 1280, false);
        //SceneCanvasGroup.alpha = 0;
        //
        //StartCoroutine(AnimationSystem.GetInstance().FadeOutAnimation(SceneCanvasGroup, 3));

        GameManager.GetInstance().FadeOutWhiteImg(0.5f);
        ClickMiddleBtn(0);
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
            case 0:     //game
                break;
            case 1:     //skin
                break;
            case 2:     //profile
                break;
            case 3:     //social
                break;
            case 4:     //setting
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

        //NetworkSystem.GetInstance().SendServer("FIND-ROOM:"+PlayerSystem.GetInstance().MyPlayerId);

        StartCoroutine(NextSceneAnimation());
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

    public override IEnumerator NextSceneAnimation()
    {
        /*
        SceneManager.LoadSceneAsync("Assets/Scene/PlayScene.unity",LoadSceneMode.Additive);
        yield return new AsyncOperation();

        yield return new WaitForSeconds(5);

        SceneSystem.GetInstance().SceneEvent(SceneEventTag.SCENE_EVENT_OUT);
        SceneSystem.GetInstance().SceneEvent(SceneEventTag.SCENE_EVENT_SCALE_DOWN);

        Rigidbody2D body = BottomBtn.GetComponent<Rigidbody2D>();
        body.AddForce(Vector2.up * 200, ForceMode2D.Impulse);
        */

        yield return new WaitForSeconds(3);
        //GameManager.GetInstance().FadeInWhiteImg(0.5f);

        //StartCoroutine(AnimationSystem.GetInstance().FadeInAnimation(SceneCanvasGroup, 3));
        //StartCoroutine(AnimationSystem.GetInstance().FadeInAnimation(RootObject, 3));

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("PlayScene");
    }
}