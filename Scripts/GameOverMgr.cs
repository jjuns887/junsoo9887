using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMgr : MonoBehaviour
{
    public Text HighScoreText;
    public Text CurrentScoreText;
    public Button RestartBtn;
    public Button ClearDataBtn;

    // Start is called before the first frame update
    void Start()
    {
        if(GameMgr.m_BestHeight < GameMgr.m_CurBHeight)
        {
            GameMgr.m_BestHeight = GameMgr.m_CurBHeight;
            GameMgr.Save();
        }

        if (HighScoreText != null)
            HighScoreText.text = "최고기록 : " + GameMgr.m_BestHeight.ToString("N2");

        if (CurrentScoreText != null)
            CurrentScoreText.text = "이번기록 : " + GameMgr.m_CurBHeight.ToString("N2");

        if (RestartBtn != null)
            RestartBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });

        if (ClearDataBtn != null)
            ClearDataBtn.onClick.AddListener(CD_BtnClick);

    }//void Start()

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) == true)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    void CD_BtnClick()
    {
        PlayerPrefs.DeleteAll();        //저장 값 모두 초기화 하기
        GameMgr.m_CurBHeight = 0.0f;

        GameMgr.Load();

        if (HighScoreText != null)
            HighScoreText.text = "최고기록 : " + GameMgr.m_BestHeight.ToString("N2");

        if (CurrentScoreText != null)
            CurrentScoreText.text = "이번기록 : " + GameMgr.m_CurBHeight.ToString("N2");
    }
}
