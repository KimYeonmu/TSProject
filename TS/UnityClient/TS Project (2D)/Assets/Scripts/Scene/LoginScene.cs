using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoginScene : IScene
{
    void Start()
    {
        Screen.SetResolution(720, 1280, false);
        GameManager.GetInstance().FadeOutWhiteImg(0.5f);

        //SceneCanvasGroup.alpha = 0;
        //
        //StartCoroutine(AnimationSystem.GetInstance().FadeOutAnimation(SceneCanvasGroup, 3));
    }

    public void GoogleBtnPress()
    {
        NetworkSystem.GetInstance().SendServer("GOOGLE-LOGIN");
    }

    public void FaceBookBtnPress()
    {
        NetworkSystem.GetInstance().SendServer("FACEBOOK-LOGIN:");
    }

    public void EcstasyBtnPress()
    {
        NetworkSystem.GetInstance().SendServer("ECSTASY-LOGIN:");
    }

    public void GuestBtnPress()
    {
        NetworkSystem.GetInstance().SendServer("GUEST-LOGIN:");

        StartCoroutine(NextSceneAnimation());
    }

    public override IEnumerator NextSceneAnimation()
    {
        //StartCoroutine(AnimationSystem.GetInstance().FadeInAnimation(SceneCanvasGroup, 3));

        GameManager.GetInstance().FadeInWhiteImg(0.5f);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Scene/MainMenu");
    }
}

