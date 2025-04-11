using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGridScaler : MonoBehaviour
{
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private int padding = 10;
    [SerializeField] private float spacing = 10;

    private GridLayoutGroup grid;

    private void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        UpdateGridLayout();
    }

    private void UpdateGridLayout()
    {
        int totalChildren = grid.transform.childCount;
        if (totalChildren == 0) return;

        float containerWidth = gridContainer.rect.width;
        float containerHeight = gridContainer.rect.height;

        int columnCount = Mathf.CeilToInt(Mathf.Sqrt(totalChildren));
        int rowCount = Mathf.CeilToInt((float)totalChildren / columnCount);

        float totalSpacingX = spacing * (columnCount - 1);
        float totalSpacingY = spacing * (rowCount - 1);

        float cellWidth = (containerWidth - totalSpacingX - 2 * padding) / columnCount;
        float cellHeight = (containerHeight - totalSpacingY - 2 * padding) / rowCount;

        grid.cellSize = new Vector2(cellWidth, cellHeight);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columnCount;
        grid.spacing = new Vector2(spacing, spacing);
        grid.padding = new RectOffset(padding, padding, padding, padding);
    }
}

