using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEA_FindGameMenues : MonoBehaviour
{
    private enum State 
    {
        Down,
        Up,
        Wait
    }

    State state = State.Wait;


    private float speed = 1000f;
    private readonly float maxHeight = 295f;
    private RectTransform rt;
    private Vector2 size = Vector2.zero;
    private bool isShowing = false;

    public GameObject[] findGames;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        size = rt.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        //print(state);
        switch (state)
        {
            case State.Down:
                Show();
                break;
            case State.Up:
                Hide();
                break;
            case State.Wait:
                if (isShowing && Input.GetMouseButtonDown(0))
                {
                    CheckMouseClickPos();
                }
                break;
        }
    }

    public void OnClickFindGameMenues()
    {
        if(state == State.Wait)
        {
            if (isShowing)
            {
                state = State.Up;
            }
            else
            {
                state = State.Down;
            }
        }
    }

    private void Show()
    {
        size.y += Time.deltaTime * speed;
        
        if(size.y >= maxHeight)
        {
            size.y = maxHeight;
            state = State.Wait;
            isShowing = true;
        }

        rt.sizeDelta = size;
    }

    private void Hide()
    {
        size.y -= Time.deltaTime * speed;

        if (size.y <= 0)
        {
            size.y = 0;
            state = State.Wait;
            isShowing = false;
        }

        rt.sizeDelta = size;
    }

    private void CheckMouseClickPos()
    {
        GameObject g = EventSystem.current.currentSelectedGameObject;
        for(int i = 0; i < findGames.Length; i++)
        {
            if(g == findGames[i])
            {
                break;
            }
        }

        state = State.Wait;
    }


}
