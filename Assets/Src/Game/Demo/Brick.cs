using UnityEngine;
using static DynamicBasePlate;

public class Brick : MonoBehaviour
{
    public string shapeId;              // 形状标识（需与底座槽位匹配）
    private bool isDragging = false;
    private DynamicBasePlate currentBasePlate;
    private DynamicBasePlate.Slot targetSlot;
    private Vector3 originalPosition;

    void Update()
    {
        if (isDragging)
        {
            // 鼠标拖拽跟随（需替换为实际输入逻辑）
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            // 检测附近底座和槽位
            CheckForSnap();
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        originalPosition = transform.position;
        GetComponent<Rigidbody>().isKinematic = true; // 拖拽时禁用物理
    }

    void OnMouseUp()
    {
        isDragging = false;
        GetComponent<Rigidbody>().isKinematic = false;

        // 如果存在目标槽位，吸附到准确位置
        if (targetSlot != null)
        {
            SnapToSlot(targetSlot);
        }
        else
        {
            // 返回原位或自由掉落
            transform.position = originalPosition;
        }
    }

    void CheckForSnap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider col in colliders)
        {
            DynamicBasePlate plate = col.GetComponent<DynamicBasePlate>();
            if (plate != null)
            {
                // 查找匹配的槽位
                targetSlot = plate.FindNearestMatchingSlot(transform.position, shapeId);
                if (targetSlot != null)
                {
                    currentBasePlate = plate;
                    return;
                }
            }
        }
        targetSlot = null;
    }

    // 在Brick脚本的SnapToSlot方法中添加角度对齐
    void SnapToSlot(Slot slot)
    {
        Vector3 targetPos = currentBasePlate.transform.TransformPoint(slot.localPosition);
        Quaternion targetRot = currentBasePlate.transform.rotation;

        //LeanTween.move(gameObject, targetPos, 0.2f).setEase(LeanTweenType.easeOutBack);
        //LeanTween.rotate(gameObject, targetRot.eulerAngles, 0.2f);
    }
}