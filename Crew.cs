/// summary
/// NevMesh와 LINQ 메소드를 이용해 가장 가까운 적을 찾고 이동하는 것을 베이스로 둡니다.
/// target에 가까워지면 공격을 메커니즘을 시행합니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Crew : MonoBehaviour
{
    //Scripts
    private Manager_Master MM_Instance = Manager_Master.Instance;


    //Components
    private Animator anim;

    public NavMeshAgent agent;


    //Variable
    private Vector3 v3ChracterScale;
    private bool isAttackable = true;

    public float fAttackPoint;
    public float fAttackSpeed;
    //public float fAttackRange;
    public float fHealthPoint;
    //public float fMovingSpeed;

    private void Start()
    {
#if __DEV__TRUE
        MM_Instance = Manager_Master.Instance;
#endif
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;

        v3ChracterScale = transform.localScale;

        //임시
        fAttackPoint = 1.0f;
        fAttackSpeed = 1.0f;
        fHealthPoint = 20.0f;
    }

    private void Update()
    {
        Vector2 v2NearestCrew = FindNearestObject().transform.position;

        //crew 방향 설정
        transform.localScale = new Vector3(v3ChracterScale.x * (transform.position.x - v2NearestCrew.x > 0 ? -1 : 1), v3ChracterScale.y, v3ChracterScale.z);

        if (Vector2.Distance(transform.position, v2NearestCrew) > 30)
        {   //대상이 너무 멀어졌을 때
            agent.isStopped = true;
            return;
        }
        else if (Vector2.Distance(transform.position, v2NearestCrew) < Constants.fAttackRange_Melee)
        {   //대상이 공격 범위 안에 들어왔을 때
            agent.isStopped = true;
            if (isAttackable)
            {
                //anim.SetTrigger("Attack"); 임시 삭제
                isAttackable = false;

                StartCoroutine(AttakDelay());
            }
        }
        else
        {   //쫓아감
            agent.isStopped = false;
            agent.SetDestination(v2NearestCrew);
        }
    }

    private GameObject FindNearestObject()
    {
        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = MM_Instance.GetListEnemy()
            .OrderBy(obj =>
            {
                return Vector2.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }
    private IEnumerator AttakDelay()
    {
        yield return new WaitForSeconds(fAttackSpeed);
        isAttackable = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            fHealthPoint -= other.transform.parent.GetComponent<Enemy>().fAttackPoint;
        }
        else if (other.CompareTag("Arrow"))
        {
            fHealthPoint -= other.GetComponent<Arrow>().fAttackPoint;
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Magic"))
        {
            fHealthPoint -= other.GetComponent<MagicEffect>().fAttackPoint;
        }

        //체력이 다하면
        if (fHealthPoint <= 0)
        {
            Debug.Log("crew 죽음!");
            gameObject.transform.position = Constants.v3Grave;
            gameObject.SetActive(false);
        }
    }
}
