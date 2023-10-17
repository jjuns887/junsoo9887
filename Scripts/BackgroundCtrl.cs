using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    GameObject Player;
    float startY = 12.0f;       //��׶����� ���� Y ���� ��ġ
    float scroll = 0.2f;        //��׶��尡 ���� �ö󰡴� �ӵ�

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("cat");
    }

    // Update is called once per frame
    void Update()
    {
        float scrollPos = startY - Player.transform.position.y * scroll;
        if (scrollPos > 12.0f)
            scrollPos = 12.0f;
        else if (scrollPos < -12.0f)
           scrollPos = -12.0f;

        transform.position = new Vector3(0.0f, Player.transform.position.y + scrollPos, 0.0f);
    }
}
