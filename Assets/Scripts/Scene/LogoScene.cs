using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoScene : IScene
{
    public int BallNum;

    private bool _isAnimation = false;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(720, 1280, false);
        StartCoroutine(BounceBall());
        StartCoroutine(NextSceneAnimation());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override IEnumerator NextSceneAnimation()
    {
        if (_isAnimation == false)
            yield return null;

        yield return new WaitForSeconds(2);

        //StartCoroutine(AnimationSystem.GetInstance().FadeInAnimation(SceneCanvasGroup, 3));
        //StartCoroutine(AnimationSystem.GetInstance().FadeInAnimation(RootObject, 3));
        GameManager.GetInstance().FadeInWhiteImg(0.5f);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("LoginScene");
    }

    public IEnumerator BounceBall()
    {
        int currentBall = 0;

        while (currentBall < BallNum)
        {
            yield return new WaitForSeconds(0.1f);

            GameObject ball = Instantiate(Resources.Load("Prefabs/LoadingBall")) as GameObject;

            ball.transform.localPosition = new Vector3(
                -100 + 300f * (float)(currentBall) / BallNum,
                50, 0);

            ball.transform.SetParent(RootObject.transform);
            currentBall++;
        }

        _isAnimation = true;
    }
}
