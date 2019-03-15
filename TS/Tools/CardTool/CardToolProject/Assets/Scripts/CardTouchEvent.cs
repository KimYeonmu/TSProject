using UnityEngine;
using System.Collections;

public class CardTouchEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 pos = touch.position;

            if (touch.phase == TouchPhase.Began)
                Debug.Log("시작점 =" + pos.x + " " + pos.y);
            else if (touch.phase == TouchPhase.Ended)
                Debug.Log("끝점 = " + pos.x + " " + pos.y);
            else if (touch.phase == TouchPhase.Moved)
                Debug.Log("이동중 = " + pos.x + " " + pos.y);
        }
	}
}
