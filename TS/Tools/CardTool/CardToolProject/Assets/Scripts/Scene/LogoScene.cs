using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScene : MonoBehaviour
{
    public GameObject LoadObject;

    public int BallNum;

    private bool _isAnimation = false;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(BounceBall());
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator LoadScene()
    {
        if (_isAnimation == false)
            yield return null;

        CardSystem.GetInstance().AddAllCard();


        yield return new WaitForSeconds(2);
        yield return StartCoroutine(MoveObjectNextScene());
    }

    public IEnumerator MoveObjectNextScene()
    {
        Debug.Log("asdfasdf");

        Scene sc = SceneManager.GetSceneByName("MainMenu");
        SceneManager.LoadScene("MainMenu");
        SceneManager.MoveGameObjectToScene(LoadObject, sc);

        yield return null;
    }


    public IEnumerator BounceBall()
    {
        int currentBall = 0;

        while (currentBall < BallNum)
        {
            yield return new WaitForSeconds(0.1f);

            GameObject ball = Instantiate(Resources.Load("Prefabs/LoadingBall")) as GameObject;

            ball.transform.localPosition = new Vector3(
                -0.5f + 1.4f * (float)(currentBall) / BallNum,
                0.5f, 0);

            currentBall++;
        }

        _isAnimation = true;
    }
}
