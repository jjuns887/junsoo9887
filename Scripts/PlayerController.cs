using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 680.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 2.0f;
    float walkSpeed = 3.0f;

    bool IsCloudColl = false;
    int m_ReserveJump = 0;   // 점프 예약 변수

    float hp = 3.0f;
    public Image[] hpImage;
    GameObject m_OverlapBlock = null;   //보상이나 화살 두세번 연속 충돌 방지용 변수

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; //실행 프레임 속도 60프레임으로 고정 시키기.. 코드
        QualitySettings.vSyncCount = 0;
        //모니터 주사율(플레임율)이 다른 컴퓨터일 경우 캐릭터 조작시 빠르게 움직일 수 있다.

        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //점프한다.
        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            m_ReserveJump = 3;
        }

        IsCloudColl = CheckIsCloudColl();
        if( ( -0.1f <= this.rigid2D.velocity.y && this.rigid2D.velocity.y <= 0.1f )
            && IsCloudColl == true )
        {
            if (0 < m_ReserveJump)
            {
                this.animator.SetTrigger("JumpTrigger");
                this.rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0.0f);
                this.rigid2D.AddForce(transform.up * this.jumpForce);

                m_ReserveJump = 0;
            }
        }

        if (0 < m_ReserveJump)
            m_ReserveJump--;

       //좌우 이동
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        //플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        ////스피드 제한
        //if(speedx < this.maxWalkSpeed)
        //{
        //    this.rigid2D.AddForce(transform.right * key * this.walkForce);
        //}

        //캐릭터 이동
        rigid2D.velocity = new Vector2( (key * walkSpeed), rigid2D.velocity.y);

        //움직이는 방향에 따라 이미지 반전 
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }


        //플레이어의 속도에 맞춰 애니메이션 속도를 바꾼다.
        if (this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

        // 플레이어가 화면 밖으로 나갔다면 처음부터
        if(transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }

        //--- 화면 밖으로 못 나가게 하기
        Vector3 pos = transform.position;
        if (pos.x < -2.5f) pos.x = -2.5f;
        if (pos.x > 2.5f) pos.x = 2.5f;
        transform.position = pos;
        //--- 화면 밖으로 못 나가게 하기

    }//void Update()

    //골 도착
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("flag") == true)
        {
            Debug.Log("골");
            SceneManager.LoadScene("ClearScene");
        }
        else if(other.gameObject.name.Contains("WaterRoot") == true)
        {
            Die();
        }
        else if(other.gameObject.name.Contains("arrow") == true)
        {
            if(m_OverlapBlock != other.gameObject)
            {
                hp -= 1.0f;
                HpImgUpdate();
                if(hp <= 0.0f)  //사망처리
                {
                    Die();
                }

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if(other.gameObject.name.Contains("fish") == true)
        {
            if(m_OverlapBlock != other.gameObject)
            {
                hp += 0.5f;  //에너지 조금 회복
                if (3.0f < hp)
                    hp = 3.0f;
                HpImgUpdate();

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }

    }////void OnTriggerEnter2D(Collider2D other)

    void Die()
    {
        //게임 앤드 처리 필요
        SceneManager.LoadScene("GameOverScene");
    }

    bool CheckIsCloudColl()
    { //고양이 발바닥 Circle 충돌체가 구름의 발판 충돌체를 밟고 있는지를 확인하는 함수

        float a_CcSize   = GetComponent<CircleCollider2D>().radius;
        Vector2 a_OffSet = GetComponent<CircleCollider2D>().offset;

        a_CcSize = a_CcSize * transform.localScale.y; //충돌구의 크기

        Vector2 a_CcPos = Vector2.zero;
        a_CcPos.x = transform.position.x + a_OffSet.x;
        a_CcPos.y = transform.position.y + a_OffSet.y;

        Collider2D[] colls = Physics2D.OverlapCircleAll(a_CcPos, a_CcSize);
        //매개변수로 넘긴 좌표와 반지름에 걸리는 모든 콜리터(충돌체)를 가져오는 함수
        foreach(Collider2D coll in colls)
        {
            if(coll.gameObject.name.Contains("CloudColl") == true)
            {
                return true;    // 구름에 하나라도 걸리면 교차 중이라고 판정
            }
        }

        return false;       //구름에 하나도 걸리는 게 없으면 교차가 아니라고 판정
    }


    void HpImgUpdate()
    {
        float a_CacHp = 0.0f;
        for(int ii = 0; ii < hpImage.Length; ii++)
        {
            a_CacHp = hp - (float)ii;
            if (a_CacHp < 0.0f)
                a_CacHp = 0.0f;

            if (1.0f < a_CacHp)
                a_CacHp = 1.0f;

            if (0.45f < a_CacHp && a_CacHp < 0.55f)
                a_CacHp = 0.445f;

            hpImage[ii].fillAmount = a_CacHp;
        }

        //for(int ii = 0; ii < hpImage.Length; ii++)
        //{
        //    if (ii < (int)hp)
        //        hpImage[ii].gameObject.SetActive(true);
        //    else
        //        hpImage[ii].gameObject.SetActive(false);
        //}
    }

}//public class PlayerController
