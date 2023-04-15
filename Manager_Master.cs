using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


static class Constants
{
    public enum ENEMY {Defender = 0, Attacker, ShortBow, LongBow, Magician, Healer, Buffer, Thief, Summoner, count };
    public enum STATUS {AttackPoint = 0, AttackSpeed, AttackRange, HelthPoint, MovingSpeed, count };
    public enum THEME { Theme1 = 0, Theme2, Theme3, count };

    //value define
    public static byte bAttacker    = 0x01;
    public static byte bDefender    = 0x02;
    public static byte bShortBow    = 0x04;
    public static byte bLongBow     = 0x08;
    public static byte bMagician    = 0x10;
    public static byte bThief       = 0x20;
    public static byte bSummoner    = 0x40;
    public static byte bHealer      = 0x80;

    public static int nArrowCount = 100;
    public static int nMagicEffectCount = 20;
    public static int nCrewDetectRange = 30;
    public static float fAttackRange_Melee = 0.5f;
    public static float fAttackRange_ShortBow = 3.0f;
    public static float fAttackRange_Magician = 2.0f;

    public static Vector3 v3Grave = new Vector3(-10000.0f, -10000.0f, 0);
}

public class Manager_Master : MonoBehaviour
{
    private static Manager_Master instance = null;

    private List<GameObject> listEnemy = new List<GameObject>();
    private List<GameObject> listCrew = new List<GameObject>();
    private List<GameObject> listArrow = new List<GameObject>();
    private List<GameObject> listMagicEffect = new List<GameObject>();

    public int nDefenderStageMaxCount;
    public int nAttackerStageMaxCount;
    public int nShortBowStageMaxCount;
    public int nLongBowStageMaxCount;
    public int nMagicianStageMaxCount;
    public int nHealerStageMaxCount;
    public int nSummonerStageMaxCount;
    public int nTheifStageMaxCount;

    public int nArrowCount_Now = 0;
    public int nMagicEffectCount_Now = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Manager_Master Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    public void ChangeScene(string strSceneName)
    {
        SceneManager.LoadScene(strSceneName);
    }

    //Enemy List
    public void AddtoList_Enemy(GameObject _goEnemy)
    {
        listEnemy.Add(_goEnemy);
    }

    public GameObject ListAt_Enemy(int nIdx)
    {
        if (nIdx < 0)
            return null;
        return listEnemy[nIdx];
    }
    public int GetListSizeEnemy()
    {
        return listEnemy.Count;
    }
    public List<GameObject> GetListEnemy()
    {
        return listEnemy;
    }

    //Crew
    public void AddtoList_Crew(GameObject _goCrew)
    {
        listCrew.Add(_goCrew);
    }
    public int GetListSizeCrew()
    {
        return listCrew.Count;
    }
    public List<GameObject> GetListCrew()
    {
        return listCrew;
    }
    public GameObject GetCrew(int nIdx)
    {
        return listCrew[nIdx];
    }

    //Arrow
    public void AddtoList_Arrow(GameObject _goArrow)
    {
        listArrow.Add(_goArrow);
    }
    public GameObject ListAt_Arrow(int nIdx)
    {
        if (nIdx < 0)
            return null;
        return listArrow[nIdx];
    }
    //MagicEffect
    public void AddtoList_MagicEffect(GameObject _goMagicEffect)
    {
        listMagicEffect.Add(_goMagicEffect);
    }
    public GameObject ListAt_MagicEffect(int nIdx)
    {
        if (nIdx < 0)
            return null;
        return listMagicEffect[nIdx];
    }

    public float GetAngletoTarget(Transform _From, Transform _To)
    {
        //대상을 바라본다
        Vector3 v3Dir = _To.position - _From.position;
        return Mathf.Atan2(v3Dir.y, v3Dir.x) * Mathf.Rad2Deg;
    }
}
