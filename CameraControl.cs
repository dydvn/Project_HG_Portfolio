/// <summary>
/// 플레이어가 직접 조작할 수 있고, 특정 범위 이상 카메라가 이동하지 못하게 하는 코드
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 0.01f;
    [SerializeField] private float horizontal = 9.5f;
    [SerializeField] private float vertical = 9.5f;

    private Vector3 beginMousePos = Vector3.zero;
    private Vector3 preMousePos = Vector3.zero;
    private Vector3 beginCamPos = Vector3.zero;

    private void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            beginMousePos = Input.mousePosition;
            beginCamPos = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            preMousePos = -(Input.mousePosition - beginMousePos) * cameraMoveSpeed;

            Vector3 newCamPos = beginCamPos + preMousePos;
            newCamPos.x = Mathf.Clamp(newCamPos.x, -vertical, vertical);
            newCamPos.y = Mathf.Clamp(newCamPos.y, -horizontal, horizontal);

            transform.position = newCamPos;
        }
#else
        //여기다가 터치로 조작 구현
#endif
    }
}
