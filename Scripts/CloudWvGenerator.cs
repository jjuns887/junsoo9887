using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudWvGenerator : MonoBehaviour
{
    public GameObject cloudWave;

    GameObject Player;
    float createHeight = 10.0f; //주인공으로부터 머리위로 10.0m 위까지만 구름층을 생성하겠다는 뜻
    float recentHeight = -2.5f; //마지막 생성된 구름층의 높이

    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("cat");

        //for(int ii = 0; ii < 50; ii++)
        //{
        //    SpawnCloudWave(recentHeight);
        //    recentHeight += 2.5f;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = this.Player.transform.position;

        //일정 높이에 구름층 생성
        if(recentHeight < playerPos.y + createHeight)
        {
            SpawnCloudWave(recentHeight);
            recentHeight += 2.5f;
        }
        //일정 높이에 구름층 생성
    }

    void SpawnCloudWave(float a_Height)
    {
        int a_Level = (int)(a_Height / 15.0f);

        int a_HideCount = 0;
        if (a_Level <= 0)
            a_HideCount = 0;
        else if (a_Level == 1)
            a_HideCount = Random.Range(0, 2);   // 0 ~ 1
        else if (a_Level == 2)
            a_HideCount = Random.Range(0, 3);   // 0 ~ 2
        else if (a_Level == 3)
            a_HideCount = Random.Range(1, 3);   // 1 ~ 2
        else if (a_Level == 4)
            a_HideCount = Random.Range(1, 4);   // 1 ~ 3
        else
            a_HideCount = Random.Range(2, 4);   // 2 ~ 3

        GameObject go = Instantiate(cloudWave) as GameObject;
        go.transform.position = new Vector3(0.0f, a_Height, 0.0f);
        go.GetComponent<CloudWaveCtrl>().SetHideCloud(a_HideCount);
    }
}
