using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    Vector3 originPosition;

    public Transform fore;

    private Vector3 StickFirstPos;  // 조이스틱의 처음 위치.
    public float Radius;           // 조이스틱 배경의 반 지름.

    public Vector3 movePosition;
    public float angle;

    private void Start()
    {
        originPosition = this.transform.position;
        StickFirstPos = fore.transform.localPosition;
    }
    public void DownPoint()
    {
        this.transform.position = Input.mousePosition;
    }
    public void UpPoint()
    {
        this.transform.position = originPosition;
    }


    public void Drag(BaseEventData _Data)
    {
        PointerEventData Data = _Data as PointerEventData;
        Vector2 startPos = this.transform.position;
        Vector2 value = Data.position - startPos;

        value = Vector2.ClampMagnitude(value, Radius);
        fore.localPosition = value;

        float distance = Vector2.Distance(startPos, fore.position) / Radius;
        value = value.normalized;
        movePosition = (value * distance);

        angle = GetAngle(this.transform.position, Input.mousePosition) + 270;
    }

    // 드래그 끝.
    public void DragEnd()
    {
        fore.localPosition = StickFirstPos; // 스틱을 원래의 위치로.
        movePosition = Vector3.zero;
    }

    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

}
