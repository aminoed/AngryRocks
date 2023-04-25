using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] bonusPrefabs;
    public float timeBetweenSpawns = 1f;
    public float timeBetweenWaves = 1f;
    public List<GameObject> enemyList;
    public List<GameObject> bonusList;

    public int[] bonusTimes = { 5, 62, 87, 120, 140, 190, 202, 224, 268, 301, 356, 388, 420, 430, 489, 510, 535, 585 };
    public int[] bonusTypes = { 1, 1, 1, 3, 1, 2, 1, 1, 2, 2, 2, 3, 1, 3, 2, 1, 1, 3 };
    public int bonusWaves = 0;

    private Vector2 defaultPosition = new Vector2(4.2f, 0.45f);
    public int hitNum = 0;
    public int waveNum = 1;
    //每一波敌人数量
    public int enemyNum = 1;

    public int bonusScore = 0;
    WaitForSeconds waitTimeBetweenWaves;
    WaitForSeconds waitTimeBetweemSpawns;

    private Vector2 genPoint;
    public bool spawnEnemy = true;

    private Coroutine coroutine;
    public GameObject gameControl;


    private string sceneName;
    public bool isLowTU;

    void Awake()
    {
        enemyList = new List<GameObject>();
        bonusList = new List<GameObject>();
        waitTimeBetweemSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
    }
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "LowUncertainty")
        {
            isLowTU = true;
        }
        else if (sceneName == "HighUncertainty")
        {
            isLowTU = false;
        }
        StartCoroutine(nameof(RandomlySpawnEnemyCoroutine));
    }
    IEnumerator RandomlySpawnEnemyCoroutine()
    {
        yield return waitTimeBetweenWaves;
        while (spawnEnemy)
        {
            if (bonusWaves < 18 && waveNum >= (int)(bonusTimes[bonusWaves] / 2 + 1))
            {
                for (int i = 0; i < enemyNum; i++)
                {
                    Quaternion targetQuaternion = Quaternion.Euler(0, 0, Random.Range(0, 360.0f));
                    Vector2 dir = defaultPosition - new Vector2(0, 0.45f);
                    genPoint = targetQuaternion * dir;
                    GameObject bonus = ObjectPool.Instance.GetObject(bonusPrefabs[bonusTypes[bonusWaves] - 1]);
                    if (isLowTU)
                    {
                        genPoint = SetLowUncertaintyGen();
                    }
                    bonus.transform.position = genPoint;
                    bonus.GetComponent<Red>().SetSpeed();

                    bonusList.Add(bonus);
                    yield return waitTimeBetweemSpawns;
                }
                waveNum++;
                // bonusNum++;
                bonusWaves++;
            }
            else
            {
                // bonusNum = 0;
                for (int i = 0; i < enemyNum; i++)
                {
                    Quaternion targetQuaternion = Quaternion.Euler(0, 0, Random.Range(0, 360.0f));
                    Vector2 dir = defaultPosition - new Vector2(0, 0.45f);

                    genPoint = targetQuaternion * dir;
                    GameObject ene = ObjectPool.Instance.GetObject(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
                    if (isLowTU)
                    {
                        genPoint = SetLowUncertaintyGen();
                    }
                    ene.transform.position = genPoint;
                    ene.GetComponent<Red>().SetSpeed();

                    enemyList.Add(ene);
                    yield return waitTimeBetweemSpawns;
                }
                waveNum++;
            }

            yield return waitTimeBetweenWaves;
        }
    }

    private Vector2 SetLowUncertaintyGen()
    {
        Vector3[] angles = new Vector3[] {
            new Vector3(0f, 0f, 45f),
            new Vector3(0f, 0f, 135f),
            new Vector3(0f, 0f, 225f),
            new Vector3(0f, 0f, 315f)
        };

        Quaternion[] rotations = new Quaternion[angles.Length];
        for (int i = 0; i < angles.Length; i++)
        {
            rotations[i] = Quaternion.Euler(angles[i]);
        }

        Vector2 dir = defaultPosition - new Vector2(0, 0.45f);
        Vector2 genPointLU = rotations[Random.Range(0, 3)] * dir;
        return genPointLU;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveFromList(GameObject obj, bool isBullet)
    {
        if (isBullet)
        {
            bonusScore = obj.GetComponent<Red>().score;
            coroutine = StartCoroutine(gameControl.GetComponent<ControlGame>().ShowScore(bonusScore));
        }
        else
        {
            bonusScore = -1;
        }
        if (bonusList.Count > 0)
        {
            bonusList.Remove(obj);
            if (isBullet)
            {
                switch (obj.tag)
                {
                    case "Green":
                        hitNum += bonusScore;
                        break;
                    case "Red":
                        hitNum += bonusScore;
                        break;
                    case "Blue":
                        hitNum += bonusScore;
                        break;
                    default:
                        break;
                }

            }
        }
        if (enemyList.Count > 0 && obj.tag == "Bird")
        {
            enemyList.Remove(obj);
            if (isBullet)
            {
                hitNum += bonusScore;
            }
        }
    }
}
