using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(MyGrid))]
public class CubeEditor : MonoBehaviour 
{
    MyGrid wayPoint;

    private void Awake()
    {
        wayPoint = GetComponent<MyGrid>();
    }
    void Update ()
    {
        SnapToGrid();
        //UpdateLabel();

    }

    private void SnapToGrid()
    {
        int GridSize = wayPoint.GetGridSize();
        transform.position = new Vector3(
            wayPoint.GetGridPos().x * GridSize,
            0f,
            wayPoint.GetGridPos().y * GridSize);
    }

    //private void UpdateLabel()
    //{
    //    TextMesh textMesh =  GetComponentInChildren<TextMesh>();
    //    string labelText = wayPoint.GetGridPos().x + "," + wayPoint.GetGridPos().y;
    //    textMesh.text = labelText;
    //    gameObject.name = "(" + labelText + ")";
    //}
}
