using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ControlGame : MonoBehaviour
{
    private GameObject cursor;
    private GameObject enemyController;
    public Text scoreText;
    public Text modeText;
    public Text bonusText;
    private int autoScore = 0;
    private int manualScore = 0;
    private int totalScore = 0;
    public int point;
    public bool isDisplayed = false;
    public float timer = 1f;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        cursor = GameObject.Find("Cursor");
        enemyController = GameObject.Find("EnemyGenerator");
    }
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(ShowScore());
        // bonusText = GameObject.Find("Canvas/pointText").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.O))
        {
            if (modeText.text == "自动模式")
            {
                modeText.text = "手动模式";
                cursor.GetComponent<player>().isAuto = false;
            }
            else if (modeText.text == "手动模式")
            {
                modeText.text = "自动模式";
                cursor.GetComponent<player>().isAuto = true;
            }
        }
        totalScore = enemyController.GetComponent<GenerateEnemies>().hitNum;
        scoreText.text = totalScore + "";

        // point = enemyController.GetComponent<GenerateEnemies>().bonusScore;

        // isDisplayed = enemyController.GetComponent<GenerateEnemies>().isHit;
        //         Debug.Log("=======isDisplayed: " + enemyController.GetComponent<GenerateEnemies>().isHit);
        // if (isDisplayed)
        // {
        //     timer -= Time.deltaTime;
        //     if (timer > 0 && point>0)
        //     {
        //         bonusText.text = "+" + point;
        //     }
        //     else
        //     {
        //         // Debug.Log("=======time: " + timer);
        //         bonusText.text = null;
        //         isDisplayed=false;
        //         timer = 1.0f;
        //     }
        // }


    }

    public void StartOtherCoroutine()
    {
        MonoBehaviour monoBehaviour = new MonoBehaviour();
        // monoBehaviour.StartCoroutine(ShowScore());
    }
    public IEnumerator ShowScore(int score)
    {
        
        bonusText.gameObject.SetActive(true); // 显示Text组件
        bonusText.text = "+" + score;
        yield return new WaitForSeconds(1f); // 等待1秒

        bonusText.gameObject.SetActive(false); // 隐藏Text组件
    }
}
