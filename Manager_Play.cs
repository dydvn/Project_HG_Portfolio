/// summary
/// 각종 대량의 오브젝트들을 미리 생성하고, coroutine을 이용해 특정 주기로 적들을 생산해내는 등 게임 플레이의 중심이 되는 스크립트입니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Manager_Play : MonoBehaviour
{
    protected Manager_Master MM_Instance = Manager_Master.Instance;

    [SerializeField]    private GameObject[] go_Enemy;
    [SerializeField]    private GameObject[] go_SpawnPoint;
    [SerializeField]    private GameObject[] go_Crew;
    [SerializeField]    private GameObject go_Arrow;
    [SerializeField]    private GameObject go_MagicEffect;
    [SerializeField]    private float fSpawnTime;


    public NavMeshSurface2d nav2d;

    private int crewLength = 0; //활성화한 크루의 수


#if __DEV__TRUE
    private void Awake()
    {
        GameObject Manager_Master_Temp = new GameObject("Manager_Master_Temp");
        Manager_Master_Temp.AddComponent<Manager_Master>();
        MM_Instance = Manager_Master.Instance;
    }
#endif

    void Start()
    {
        //navmesh bake
        nav2d.BuildNavMesh();

        //코루틴에 필요한거... 테마, 스테이지, 웨이브 별 등장 적 수,
        //코루틴을 시작하면서 해당 웨이브에 나올 적들의 스텟 계산해주고, 적들은 스폰되면서 그 값을 받아서 사용

        //크루 생성
        CrewInit();
        //~크루 생성

        //Pool 생성
        MakeEnemyPool((int)Constants.THEME.Theme1, 1);  //Enemy                테마, Stage 번호 어플에서 받아다가 넣어줘야됨. 지금은 임시로 정수 때려넣음
        MakeArrowPool();                                //Arrow
        MakeMagicEffectPool();                          //Magic Effect
        //~Pool 생성

        //최초 Spawn 시작
        //StartCoroutine(SpawnEnemy_Attacker((int)Constants.THEME.Theme1, 1, 1));//테마, Stage 번호 어플에서 받아다가 넣어줘야됨. 최초 웨이브니까 웨이브는 1로 고정
        //StartCoroutine(SpawnEnemy_ShortBow((int)Constants.THEME.Theme1, 1, 1));//테마, Stage 번호 어플에서 받아다가 넣어줘야됨. 최초 웨이브니까 웨이브는 1로 고정
        //StartCoroutine(SpawnEnemy_Defender((int)Constants.THEME.Theme1, 1, 1));//테마, Stage 번호 어플에서 받아다가 넣어줘야됨. 최초 웨이브니까 웨이브는 1로 고정
        StartCoroutine(SpawnEnemy_Magician((int)Constants.THEME.Theme1, 1, 1));//테마, Stage 번호 어플에서 받아다가 넣어줘야됨. 최초 웨이브니까 웨이브는 1로 고정
        //StartCoroutine(SpawnEnemy_Thief((int)Constants.THEME.Theme1, 1, 1));//테마, Stage 번호 어플에서 받아다가 넣어줘야됨. 최초 웨이브니까 웨이브는 1로 고정
    }

    private IEnumerator SpawnEnemy_Defender(int _nTheme, int _nStageNum, int _nWaveNum)
    {
        //int _nWaveMax = theme, stage, wave 고려해서 json에서 받아와서 설정;
        int nWaveMax = 20;
        int nDefenderCount = 0;

        yield return new WaitForSeconds(3.0f);

        //여기서 스텟 세팅할 예정
        //float fAttackPoint;
        //float fAttackSpeed;
        //float fAttackRange;
        //float fHealthPoint = SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        //float fMovingSpeed;

        var wfs = new WaitForSeconds(fSpawnTime * 1.3f);

        while (nDefenderCount < nWaveMax)
        {
            int nIdx = Random.Range(0, 8);
            GameObject go_TempEnemy = MM_Instance.ListAt_Enemy(nDefenderCount);
            go_TempEnemy.transform.position = go_SpawnPoint[nIdx].transform.position;
            //Enemy_Defender enemy_Defender = go_TempEnemy.GetComponent<Enemy_Defender>();
            //여기서 스텟 입력할 예정
            //enemy_Defender.fHealthPoint = fEnemy_Status[(int)Constants.ENEMY.Defender * (int)Constants.STATUS.count + (int)Constants.STATUS.AttackPoint];
            //enemy_Defender.fHealthPoint = fEnemy_Status[(int)Constants.ENEMY.Defender * (int)Constants.STATUS.count + (int)Constants.STATUS.AttackRange];
            //enemy_Defender.fHealthPoint = fEnemy_Status[(int)Constants.ENEMY.Defender * (int)Constants.STATUS.count + (int)Constants.STATUS.AttackSpeed];
            //enemy_Defender.fHealthPoint = fHealthPoint;
            //enemy_Defender.fHealthPoint = 8;
            //enemy_Defender.fHealthPoint = fEnemy_Status[(int)Constants.ENEMY.Defender * (int)Constants.STATUS.count + (int)Constants.STATUS.MovingSpeed];

            go_TempEnemy.SetActive(true);
            nDefenderCount++;
            yield return wfs;
        }

        Debug.Log("Defender Spawn 끝!");
    }
    private IEnumerator SpawnEnemy_Attacker(int _nTheme, int _nStageNum, int _nWaveNum)
    {
        //int _nWaveMax = theme, stage, wave 고려해서 json에서 받아와서 설정;
        int nWaveMax = 10;  //한 웨이브에 생성되는 유닛 개수

        yield return new WaitForSeconds(3.0f);

        //여기서 스텟 세팅
        float fHealthPoint = 5;     //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackPoint = 1.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackRange = 0.5f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackSpeed = 2.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fMovingSpeed = 1.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;

        var AttackerDlayTimer = new WaitForSeconds(fSpawnTime);
        while (nWaveMax > 0)
        {
            nWaveMax--;
            int nIdx = Random.Range(0, 8);
            GameObject go_TempEnemy = MM_Instance.ListAt_Enemy(MM_Instance.nDefenderStageMaxCount + nWaveMax);
            go_TempEnemy.transform.position = go_SpawnPoint[nIdx].transform.position;
            Enemy enemy = go_TempEnemy.GetComponent<Enemy>();
            //Enemy_Attacker enemy_Attacker = go_TempEnemy.GetComponent<Enemy_Attacker>();
            //여기서 스텟 입력
            enemy.bEnemyType = Constants.bAttacker;
            enemy.fHealthPoint = fHealthPoint;
            enemy.fAttackPoint = fAttackPoint;
            enemy.fAttackRange = fAttackRange;
            enemy.fAttackSpeed = fAttackSpeed;
            enemy.fMovingSpeed = fMovingSpeed;

            go_TempEnemy.SetActive(true);
            yield return AttackerDlayTimer;
        }

        Debug.Log("Attacker Spawn 끝!");
    }
    private IEnumerator SpawnEnemy_ShortBow(int _nTheme, int _nStageNum, int _nWaveNum)
    {
        //int _nWaveMax = theme, stage, wave 고려해서 json에서 받아와서 설정;
        int nWaveMax = 5;
        int nShortBowCount = 0;

        yield return new WaitForSeconds(3.0f);

        //여기서 스텟 세팅
        float fHealthPoint = 2;     //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackPoint = 2.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackRange = 3.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackSpeed = 1.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fMovingSpeed = 1.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;

        while (nShortBowCount < nWaveMax)
        {
            int nIdx = Random.Range(0, 8);
            GameObject go_TempEnemy = MM_Instance.ListAt_Enemy(MM_Instance.nDefenderStageMaxCount + MM_Instance.nAttackerStageMaxCount + nShortBowCount);
            go_TempEnemy.transform.position = go_SpawnPoint[nIdx].transform.position;
            Enemy enemy = go_TempEnemy.GetComponent<Enemy>();
            //여기서 스텟 입력
            enemy.bEnemyType = Constants.bShortBow;
            enemy.fHealthPoint = fHealthPoint;
            enemy.fAttackPoint = fAttackPoint;
            enemy.fAttackRange = fAttackRange;
            enemy.fAttackSpeed = fAttackSpeed;
            enemy.fMovingSpeed = fMovingSpeed;

            go_TempEnemy.SetActive(true);
            nShortBowCount++;
            yield return new WaitForSeconds(fSpawnTime * 2);
        }

        Debug.Log("ShortBow Spawn 끝!");
    }
    private IEnumerator SpawnEnemy_Magician(int _nTheme, int _nStageNum, int _nWaveNum)
    {
        //int _nWaveMax = theme, stage, wave 고려해서 json에서 받아와서 설정;
        int nWaveMax = 5;
        int nMagicianCount = 0;

        yield return new WaitForSeconds(3.0f);

        //여기서 스텟 세팅
        float fHealthPoint = 3;     //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackPoint = 3.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackRange = 2.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackSpeed = 2.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fMovingSpeed = 1.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;

        while (nMagicianCount < nWaveMax)
        {
            int nIdx = Random.Range(0, 8);
            GameObject go_TempEnemy = MM_Instance.ListAt_Enemy(MM_Instance.nDefenderStageMaxCount + MM_Instance.nAttackerStageMaxCount + MM_Instance.nShortBowStageMaxCount + MM_Instance.nLongBowStageMaxCount + nMagicianCount);
            go_TempEnemy.transform.position = go_SpawnPoint[nIdx].transform.position;
            Enemy enemy = go_TempEnemy.GetComponent<Enemy>();
            //여기서 스텟 입력
            enemy.bEnemyType = Constants.bMagician;
            enemy.fHealthPoint = fHealthPoint;
            enemy.fAttackPoint = fAttackPoint;
            enemy.fAttackRange = fAttackRange;
            enemy.fAttackSpeed = fAttackSpeed;
            enemy.fMovingSpeed = fMovingSpeed;

            go_TempEnemy.SetActive(true);
            nMagicianCount++;
            yield return new WaitForSeconds(fSpawnTime * 2);
        }

        Debug.Log("ShortBow Spawn 끝!");
    }
    private IEnumerator SpawnEnemy_Thief(int _nTheme, int _nStageNum, int _nWaveNum)
    {
        //int _nWaveMax = theme, stage, wave 고려해서 json에서 받아와서 설정;
        int nWaveMax = 5;

        yield return new WaitForSeconds(3.0f);

        //여기서 스텟 세팅
        float fHealthPoint = 10;    //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackPoint = 2.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackRange = 3.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fAttackSpeed = 0.5f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;
        float fMovingSpeed = 1.0f;  //SetEnemyStatus(Constants.ENEMY.Defender, Constants.STATUS.HealthPoint, Stat); //뭐 대충 이런 함수 만들어서 해당 웨이브의 스텟 결정;

        while (nWaveMax > 0)
        {
            nWaveMax--;
            int nIdx = Random.Range(0, 8);
            GameObject go_TempEnemy = MM_Instance.ListAt_Enemy(MM_Instance.nDefenderStageMaxCount + MM_Instance.nAttackerStageMaxCount + MM_Instance.nShortBowStageMaxCount + MM_Instance.nLongBowStageMaxCount + MM_Instance.nMagicianStageMaxCount + MM_Instance.nHealerStageMaxCount + MM_Instance.nSummonerStageMaxCount + nWaveMax);
            go_TempEnemy.transform.position = go_SpawnPoint[nIdx].transform.position;
            Enemy enemy = go_TempEnemy.GetComponent<Enemy>();
            //여기서 스텟 입력
            enemy.bEnemyType = Constants.bThief;
            enemy.fHealthPoint = fHealthPoint;
            enemy.fAttackPoint = fAttackPoint;
            enemy.fAttackRange = fAttackRange;
            enemy.fAttackSpeed = fAttackSpeed;
            enemy.fMovingSpeed = fMovingSpeed;

            go_TempEnemy.SetActive(true);
            yield return new WaitForSeconds(fSpawnTime * 2);
        }

        Debug.Log("Thief Spawn 끝!");
    }

    private void CrewInit()
    {
        crewLength = go_Crew.Length;

        //유저가 활성화시킨 Crew만 List에 삽입할것
        for (int i = 0; i < crewLength; ++i)
        {
            MM_Instance.AddtoList_Crew(go_Crew[i]);
        }

        //크루 능력치 설정
        
    }
    private void MakeEnemyPool(int _nTheme, int _nStageNum)
    {
        /// <summary>
        /// nStageNum 받아와서 그 스테이지에 나오는 적들을 생성될 최대 수만큼 pool에 만들어 놓는다.
        /// 지금은 테스트 코드
        /// </summary>

        //defender
        //MM_Instance.nDefenderStageMaxCount = json에서 받아와서 설정
        MM_Instance.nDefenderStageMaxCount = 30;

        //attacker
        //MM_Instance.nAttackerStageMaxCount = json에서 받아와서 설정;
        MM_Instance.nAttackerStageMaxCount = 20;

        //shortbow
        //MM_Instance.nShortBowStageMaxCount = json에서 받아와서 설정;
        MM_Instance.nShortBowStageMaxCount = 20;

        //magician
        //MM_Instance.nMagicianStageMaxCount = json에서 받아와서 설정;
        MM_Instance.nMagicianStageMaxCount = 20;

        //thief
        //MM_Instance.nTheifStageMaxCount = json에서 받아와서 설정;
        MM_Instance.nTheifStageMaxCount = 20;

        InsertEnemyList((int)Constants.ENEMY.Defender, MM_Instance.nDefenderStageMaxCount);
        InsertEnemyList((int)Constants.ENEMY.Attacker, MM_Instance.nAttackerStageMaxCount);
        InsertEnemyList((int)Constants.ENEMY.ShortBow, MM_Instance.nShortBowStageMaxCount);
        InsertEnemyList((int)Constants.ENEMY.Magician, MM_Instance.nMagicianStageMaxCount);
        InsertEnemyList((int)Constants.ENEMY.Thief, MM_Instance.nTheifStageMaxCount);
        //InsertEnemyList((int)Constants.ENEMY.Attacker, nAttackerCount_StageMax);
        //InsertEnemyList((int)Constants.ENEMY.Attacker, nAttackerCount_StageMax);
        //InsertEnemyList((int)Constants.ENEMY.Attacker, nAttackerCount_StageMax);
        //InsertEnemyList((int)Constants.ENEMY.Attacker, nAttackerCount_StageMax);
        //InsertEnemyList((int)Constants.ENEMY.Attacker, nAttackerCount_StageMax);
        //InsertEnemyList((int)Constants.ENEMY.Attacker, nAttackerCount_StageMax);
    }
    private void MakeArrowPool()
    {
        for (int i = 0; i < Constants.nArrowCount; i++)
        {
            GameObject go_TempArrow = Instantiate(go_Arrow, new Vector3(0, 0, 0), Quaternion.identity);
            go_TempArrow.SetActive(false);
            MM_Instance.AddtoList_Arrow(go_TempArrow);
        }
    }
    private void MakeMagicEffectPool()
    {
        for (int i = 0; i < Constants.nMagicEffectCount; i++)
        {
            GameObject go_TempMagicEffect = Instantiate(go_MagicEffect, new Vector3(0, 0, 0), Quaternion.identity);
            go_MagicEffect.SetActive(false);
            MM_Instance.AddtoList_MagicEffect(go_TempMagicEffect);
        }
    }
    private void InsertEnemyList(int _nEnemyCode, int _nCount)
    {
        for (int i = 0; i < _nCount; i++)
        {
            GameObject go_TempEnemy = Instantiate(go_Enemy[_nEnemyCode], new Vector3(0, 0, 0), Quaternion.identity);
            go_TempEnemy.SetActive(false);
            go_TempEnemy.transform.position = Constants.v3Grave;
            MM_Instance.AddtoList_Enemy(go_TempEnemy);
        }
    }
    private float SetEnemyStatus(byte _enemy, byte _status)
    {
        return 0;
    }
}
