using UnityEngine;
using UnityEditor;

/**
 * @brief TableEditorDraw_Toolbar
 * @detail 테이블툴 툴바.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableEditorDraw_Toolbar : TableEditorDraw
{
    public TableEditorDraw_Toolbar(TableTool _tool) : base(_tool)
    {
    }

    public override void Draw()
    {
        base.Draw();
        GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();

        GUILayout.Label("auto Bake :", GUILayout.Width(70));
        TableFile_BakeInfoGroup.isAutoBake = EditorGUILayout.Toggle(TableFile_BakeInfoGroup.isAutoBake , GUILayout.Width(20));
       
        if (GUILayout.Button("Open Excel", EditorStyles.toolbarButton, GUILayout.Width(120)))
        {
            if (System.IO.Directory.Exists(TableTool.directory_tableLoad) == false)
            {
                System.IO.Directory.CreateDirectory(TableTool.directory_tableLoad);
            }
            Application.OpenURL(TableTool.directory_tableLoad);
        }

        if (GUILayout.Button("Open bytes", EditorStyles.toolbarButton, GUILayout.Width(120)))
        {
            if (System.IO.Directory.Exists(TableTool.directory_tableSave) == false)
            {
                System.IO.Directory.CreateDirectory(TableTool.directory_tableSave);
            }           
            Application.OpenURL(TableTool.directory_tableSave);
        }

        if ( GUILayout.Button("All Bake", EditorStyles.toolbarButton, GUILayout.Width(100)) )
        {
            AllBake();
        }
        GUILayout.EndHorizontal();
    }
     
    /**
     * 모든 테이블을 변환한다. KeyName 있으면 스크립트도 생성한다.
     */
    public void AllBake()
    {
        if( m_tableTool.tableBake.AllBake() == false )
        {
            EditorUtility.DisplayDialog("failed", "all bake failed", "OK");
            return;
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("", "all bake complete", "OK");
    }
}