using UnityEngine;

public class DynamicBasePlate : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Vector3 localPosition; // 槽位在底座本地坐标系中的位置
        public string shapeId = "Default";
        public bool isOccupied;
    }

    [Header("Grid Settings")]
    public int rows = 5;         // 行数
    public int columns = 5;      // 列数
    public float gridSpacing = 1.0f; // 槽位间距（单位：米）

    [Header("References")]
    public GameObject slotIndicatorPrefab; // 可选：槽位位置可视化预制体

    public Slot[] slots;         // 动态生成的槽位数组
    public float snapDistance = 0.3f;

    void Start()
    {
        GenerateGrid();
    }

    // 根据行列数生成网格
    public void GenerateGrid()
    {
        slots = new Slot[rows * columns];

        // 计算起始偏移，使网格居中
        float startX = -(columns - 1) * gridSpacing / 2f;
        float startZ = -(rows - 1) * gridSpacing / 2f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                slots[index] = new Slot
                {
                    localPosition = new Vector3(
                        startX + j * gridSpacing,
                        0,
                        startZ + i * gridSpacing
                    ),
                    shapeId = "Default",
                    isOccupied = false
                };

                // 可视化槽位（调试用）
                if (slotIndicatorPrefab)
                {
                    Instantiate(slotIndicatorPrefab,
                        transform.TransformPoint(slots[index].localPosition),
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }

        //AdjustBaseSize(); // 调整底座视觉大小
    }

    // 调整底座物体缩放以匹配网格
    private void AdjustBaseSize()
    {
        // 假设底座原始模型为1x1单位平面
        transform.localScale = new Vector3(
            columns * gridSpacing,
            1,
            rows * gridSpacing
        );
    }

    // 查找最近匹配槽位（修改版）
    public Slot FindNearestMatchingSlot(Vector3 brickWorldPos, string brickShapeId)
    {
        Slot nearestSlot = null;
        float minDistance = float.MaxValue;

        foreach (Slot slot in slots)
        {
            if (slot.isOccupied) continue;

            Vector3 slotWorldPos = transform.TransformPoint(slot.localPosition);
            float distance = Vector3.Distance(slotWorldPos, brickWorldPos);

            if (distance < minDistance && distance <= snapDistance)
            {
                if (slot.shapeId == brickShapeId || slot.shapeId == "Default")
                {
                    minDistance = distance;
                    nearestSlot = slot;
                }
            }
        }
        return nearestSlot;
    }

    // 标记槽位占用
    public void OccupySlot(Slot slot)
    {
        if (slot != null) slot.isOccupied = true;
    }
}