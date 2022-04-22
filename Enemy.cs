using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 모든 Enemy 공용
    /// </summary>


    //Scripts
    private Manager_Master MM_Instance = Manager_Master.Instance;

    //Component
    private NavMeshAgent agent;
    private Animator anim;

    //Variable
    private Vector3 v3ChracterScale;
    private bool isAttackable = true;

    public byte bEnemyType = 0x00;
    public float fAttackPoint;//오브젝트 생성될 때 외부에서 설정해줌
    public float fAttackSpeed;//                "
    public float fAttackRange;//                "
    public float fHealthPoint;//                "
    public float fMovingSpeed;//                "


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = fMovingSpeed;

        v3ChracterScale = transform.localScale;

        //임시
        //fAttackPoint = 1.0f;
        //fAttackSpeed = 2.0f;
    }


    private void Update()
    {
        GameObject go_Nearest = FindNearestObject();
        Vector2 v2NearestCrew = go_Nearest.transform.position;

        transform.localScale = new Vector3(v3ChracterScale.x * (transform.position.x - v2NearestCrew.x > 0 ? 1 : -1), v3ChracterScale.y, v3ChracterScale.z);

        if (Vector2.Distance(transform.position, v2NearestCrew) > Constants.nCrewDetectRange)
        {//맵안에 적이 없으면 멈춤
            agent.isStopped = true;
            return;
        }
        else if (Vector2.Distance(transform.position, v2NearestCrew) < fAttackRange)
        {//공격범위 안에 적이 들어오면 공격모션 시행
            //공격모션
            agent.isStopped = true;
            if (isAttackable)
            {
                //anim.SetTrigger("Attack"); 임시 삭제
                isAttackable = false;

                if ((bEnemyType & Constants.bAttacker) > 0)
                    StartCoroutine(MeleeAttak());
                else if ((bEnemyType & Constants.bShortBow) > 0)
                    StartCoroutine(ArrowAttak(go_Nearest));
                else if ((bEnemyType & Constants.bMagician) > 0)
                    StartCoroutine(MagicAttak(go_Nearest));
                else if ((bEnemyType & Constants.bThief) > 0)
                    StartCoroutine(ThiefAttak(go_Nearest));
            }
        }
        else
        {//범위안의 적이 제거되면 다시 탐색
            agent.isStopped = false;
            agent.SetDestination(v2NearestCrew);
            if ((bEnemyType & Constants.bThief) > 0)
                fAttackRange = 3.0f;
        }
    }

    private GameObject FindNearestObject()
    {
        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = MM_Instance.GetListCrew()
            .OrderBy(obj =>
            {
                return Vector2.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }

    //근접 공격
    private IEnumerator MeleeAttak()
    {
        yield return new WaitForSeconds(fAttackSpeed);
        isAttackable = true;
    }
    //활쏘는 공격
    private IEnumerator ArrowAttak(GameObject _goTarget)
    {
        //shoot
        GameObject go_Arrow = MM_Instance.ListAt_Arrow(MM_Instance.nArrowCount_Now);
        MM_Instance.nArrowCount_Now++;
        if (MM_Instance.nArrowCount_Now >= Constants.nArrowCount)
            MM_Instance.nArrowCount_Now = 0;
        go_Arrow.transform.position = transform.position;
        go_Arrow.transform.rotation = Quaternion.AngleAxis(MM_Instance.GetAngletoTarget(transform, _goTarget.transform) - 90, Vector3.forward);
        Arrow arrow = go_Arrow.GetComponent<Arrow>();
        arrow.fAttackPoint = fAttackPoint;
        arrow.go_Target = _goTarget;
        go_Arrow.SetActive(true);

        yield return new WaitForSeconds(fAttackSpeed);
        isAttackable = true;
    }
    //마법형 공격
    private IEnumerator MagicAttak(GameObject _goTarget)
    {
        GameObject go_MagicEffect = MM_Instance.ListAt_MagicEffect(MM_Instance.nMagicEffectCount_Now);
        MM_Instance.nMagicEffectCount_Now++;
        if (MM_Instance.nMagicEffectCount_Now >= Constants.nMagicEffectCount)
            MM_Instance.nMagicEffectCount_Now = 0;
        go_MagicEffect.transform.position = _goTarget.transform.position - new Vector3(0, 0.3f, 0);
        MagicEffect magicEffect = go_MagicEffect.GetComponent<MagicEffect>();
        magicEffect.fAttackPoint = fAttackPoint;
        go_MagicEffect.SetActive(true);

        yield return new WaitForSeconds(fAttackSpeed);
        isAttackable = true;
    }
    //도적형
    private IEnumerator ThiefAttak(GameObject _goTarget)
    {
        fAttackRange = 0.5f;
        transform.position = _goTarget.transform.position;
        yield return new WaitForSeconds(fAttackSpeed);
        isAttackable = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Crew"))
        {
            fHealthPoint -= other.transform.parent.GetComponent<Crew>().fAttackPoint;
            //체력이 다하면
            if (fHealthPoint <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        gameObject.transform.position = Constants.v3Grave;
        isAttackable = true;
    }
}
