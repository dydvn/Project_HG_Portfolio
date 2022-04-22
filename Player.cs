using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Player : MonoBehaviour
{
    private Manager_Master MM_Instance = Manager_Master.Instance;
    private NavMeshAgent agent;

    private GameObject go_Target;

    private Vector3 v3ChracterScale;

    void Start()
    {
#if __DEV__TRUE
        MM_Instance = Manager_Master.Instance;
#endif
        go_Target = null;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        v3ChracterScale = transform.localScale;
    }

    void Update()
    {
        if (go_Target)
        {
            transform.localScale = new Vector3(v3ChracterScale.x * (transform.position.x - go_Target.transform.position.x > 0 ? -1 : 1), v3ChracterScale.y, v3ChracterScale.z);
            agent.SetDestination(go_Target.transform.position);
        }
    }



    public void BTN_SetTargetCrew(int nCrewIdx)
    {
        //플레이어의 타겟을 정해주기
        if (nCrewIdx >= MM_Instance.GetListSizeCrew() || nCrewIdx < 0)
        {
            go_Target = null;
            print("Crew index error");
            return;
        }

        go_Target = MM_Instance.GetCrew(nCrewIdx);
    }
}
