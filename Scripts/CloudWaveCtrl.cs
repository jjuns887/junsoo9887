using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudWaveCtrl : MonoBehaviour
{
    GameObject Player;
    float destroyDistance = 10.0f;  //주인공 아래쪽으로 10m

    public GameObject[] Clouds;
    public GameObject Fish;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("cat");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = Player.transform.position;

        //일정 거리 아래 파괴
        if (transform.position.y < playerPos.y - destroyDistance)
            Destroy(gameObject);
    }

    public void SetHideCloud(int a_Count)
    {   //a_Count 몇개를 감출 건지의 개수

        List<int> active = new List<int>();
        for(int ii = 0; ii < Clouds.Length; ii++)
        {
            active.Add(ii);
        }

        for(int ii = 0; ii < a_Count; ii++)
        {
            int ran = Random.Range(0, active.Count);
            Clouds[ active[ran] ].SetActive(false);

            active.RemoveAt(ran);
        }

        active.Clear();

        //--- 코인 스폰 시키기...
        int range = 10;     //10분의 1확률로 구름위에 아이템 생성

        SpriteRenderer[] a_CloudObj = GetComponentsInChildren<SpriteRenderer>();
        //Active가 활성화 되어 있는 땅 목록만 가져오는 방법
        //매개변수이 디폴트값은 false 이고 false면 활성화 되어 있는 차일드 목록만 받아온다.
        for(int ii = 0; ii < a_CloudObj.Length; ii++)
        {
            if (Random.Range(0, range) == 0)
                SpawnFish(a_CloudObj[ii].transform.position);
        }
        //--- 코인 스폰 시키기...

    }//public void SetHideCloud(int a_Count)

    void SpawnFish(Vector3 a_Pos)
    {
        GameObject go = Instantiate(Fish);
        go.transform.position = a_Pos + Vector3.up * 0.8f;
    }
}
