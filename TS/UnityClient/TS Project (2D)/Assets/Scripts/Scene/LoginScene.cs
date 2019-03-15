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
        SceneCanvasGroup.alpha = 0;

        StartCoroutine(AnimationSystem.GetInstance().FadeOutAnimation(SceneCanvasGroup, 3));
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
        StartCoroutine(AnimationSystem.GetInstance().FadeInAnimation(SceneCanvasGroup, 3));

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Scene/MainMenu");
    }
}

