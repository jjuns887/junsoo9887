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
    int m_ReserveJump = 0;   // ���� ���� ����

    float hp = 3.0f;
    public Image[] hpImage;
    GameObject m_OverlapBlock = null;   //�����̳� ȭ�� �μ��� ���� �浹 ������ ����

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; //���� ������ �ӵ� 60���������� ���� ��Ű��.. �ڵ�
        QualitySettings.vSyncCount = 0;
        //����� �ֻ���(�÷�����)�� �ٸ� ��ǻ���� ��� ĳ���� ���۽� ������ ������ �� �ִ�.

        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //�����Ѵ�.
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

       //�¿� �̵�
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        //�÷��̾� �ӵ�
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        ////���ǵ� ����
        //if(speedx < this.maxWalkSpeed)
        //{
        //    this.rigid2D.AddForce(transform.right * key * this.walkForce);
        //}

        //ĳ���� �̵�
        rigid2D.velocity = new Vector2( (key * walkSpeed), rigid2D.velocity.y);

        //�����̴� ���⿡ ���� �̹��� ���� 
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }


        //�÷��̾��� �ӵ��� ���� �ִϸ��̼� �ӵ��� �ٲ۴�.
        if (this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

        // �÷��̾ ȭ�� ������ �����ٸ� ó������
        if(transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }

        //--- ȭ�� ������ �� ������ �ϱ�
        Vector3 pos = transform.position;
        if (pos.x < -2.5f) pos.x = -2.5f;
        if (pos.x > 2.5f) pos.x = 2.5f;
        transform.position = pos;
        //--- ȭ�� ������ �� ������ �ϱ�

    }//void Update()

    //�� ����
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("flag") == true)
        {
            Debug.Log("��");
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
                if(hp <= 0.0f)  //���ó��
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
                hp += 0.5f;  //������ ���� ȸ��
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
        //���� �ص� ó�� �ʿ�
        SceneManager.LoadScene("GameOverScene");
    }

    bool CheckIsCloudColl()
    { //����� �߹ٴ� Circle �浹ü�� ������ ���� �浹ü�� ��� �ִ����� Ȯ���ϴ� �Լ�

        float a_CcSize   = GetComponent<CircleCollider2D>().radius;
        Vector2 a_OffSet = GetComponent<CircleCollider2D>().offset;

        a_CcSize = a_CcSize * transform.localScale.y; //�浹���� ũ��

        Vector2 a_CcPos = Vector2.zero;
        a_CcPos.x = transform.position.x + a_OffSet.x;
        a_CcPos.y = transform.position.y + a_OffSet.y;

        Collider2D[] colls = Physics2D.OverlapCircleAll(a_CcPos, a_CcSize);
        //�Ű������� �ѱ� ��ǥ�� �������� �ɸ��� ��� �ݸ���(�浹ü)�� �������� �Լ�
        foreach(Collider2D coll in colls)
        {
            if(coll.gameObject.name.Contains("CloudColl") == true)
            {
                return true;    // ������ �ϳ��� �ɸ��� ���� ���̶�� ����
            }
        }

        return false;       //������ �ϳ��� �ɸ��� �� ������ ������ �ƴ϶�� ����
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
