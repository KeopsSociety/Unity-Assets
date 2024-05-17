using UnityEngine;
using UnityEditor;
using System;

public enum BlockColors { blank, red, blue, green };

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Scriptable Objects/Level Info")]
public class LevelInfo : ScriptableObject
{
    public bool showBoard;
    public int columns = 0;
    public int rows = 0;
    public BlockColumn[] board;
    private void Awake()
    {
        Init();
    }


    [ContextMenu("Refresh board (Destroy previous data)")]
    public void Init ()
    {
        board = new BlockColumn[columns];
        for (int i = 0; i < columns; i ++)
        {
            board[i] = new BlockColumn();
            board[i].rows = new BlockColors[rows];
        }
    }
}

[Serializable]
public class BlockColumn
{
    public BlockColors[] rows = new BlockColors[8];
}

[CustomEditor(typeof(LevelInfo))]
public class LevelEditor : Editor
{

    public bool showLevel = true;

    public override void OnInspectorGUI()
    {
        LevelInfo level = (LevelInfo)target;
        EditorGUILayout.Space();

        level.columns = EditorGUILayout.IntField("Number of columns:", level.columns);
        level.rows = EditorGUILayout.IntField("Number of rows:", level.rows);

        showLevel = EditorGUILayout.Foldout(showLevel, "Board (" + level.columns + " x " + level.rows + ")");
        if (showLevel)
        {
            EditorGUI.indentLevel = 0;

            GUIStyle tableStyle = new GUIStyle("box");
            tableStyle.padding = new RectOffset(10, 10, 10, 10);
            tableStyle.margin.left = 32;

            GUIStyle headerColumnStyle = new GUIStyle();
            headerColumnStyle.fixedWidth = 35;

            GUIStyle columnStyle = new GUIStyle();
            columnStyle.fixedWidth = 65;

            GUIStyle rowStyle = new GUIStyle();
            rowStyle.fixedHeight = 25;

            GUIStyle rowHeaderStyle = new GUIStyle();
            rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

            GUIStyle columnHeaderStyle = new GUIStyle();
            columnHeaderStyle.fixedWidth = 30;
            columnHeaderStyle.fixedHeight = 25.5f;

            GUIStyle columnLabelStyle = new GUIStyle();
            columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
            columnLabelStyle.alignment = TextAnchor.MiddleCenter;
            columnLabelStyle.fontStyle = FontStyle.Bold;

            GUIStyle cornerLabelStyle = new GUIStyle();
            cornerLabelStyle.fixedWidth = 42;
            cornerLabelStyle.alignment = TextAnchor.MiddleRight;
            cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
            cornerLabelStyle.fontSize = 14;
            cornerLabelStyle.padding.top = -5;

            GUIStyle rowLabelStyle = new GUIStyle();
            rowLabelStyle.fixedWidth = 25;
            rowLabelStyle.alignment = TextAnchor.MiddleRight;
            rowLabelStyle.fontStyle = FontStyle.Bold;

            GUIStyle enumStyle = new GUIStyle("popup");
            rowStyle.fixedWidth = 65;

            EditorGUILayout.BeginHorizontal(tableStyle);
            for (int x = -1; x < level.columns; x++)
            {
                EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
                for (int y = -1; y < level.rows; y++)
                {
                    if (x == -1 && y == -1)
                    {
                        EditorGUILayout.BeginVertical(rowHeaderStyle);
                        EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (x == -1)
                    {
                        EditorGUILayout.BeginVertical(columnHeaderStyle);
                        EditorGUILayout.LabelField(y.ToString(), rowLabelStyle);
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (y == -1)
                    {
                        EditorGUILayout.BeginVertical(rowHeaderStyle);
                        EditorGUILayout.LabelField(x.ToString(), columnLabelStyle);
                        EditorGUILayout.EndHorizontal();
                    }

                    if (x >= 0 && y >= 0)
                    {
                        EditorGUILayout.BeginHorizontal(rowStyle);

                        try
                        {
                            level.board[x].rows[y] = (BlockColors)EditorGUILayout.EnumPopup(level.board[x].rows[y], enumStyle);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Debug.LogWarning("Cannot draw board because columns or rows didn't match! Please, select \"Init board\" in inspector options");
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}