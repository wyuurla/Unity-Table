using UnityEngine;
using UnityEditor;

/**
 * @brief TableEditorDraw_Info
 * @detail 선택한 테이블의 정보를 표시한다.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableEditorDraw_Info : TableEditorDraw
{    
    Vector2 m_scrollbar = Vector2.zero;

    public TableEditorDraw_Info(TableTool _tool) : base(_tool)
    {
        m_tableTool = _tool;
    }

    public override void Draw()
    {
        base.Draw();  
        m_scrollbar = GUILayout.BeginScrollView(m_scrollbar, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        TableFileBake _select = m_tableTool.classList.select;
        if (_select != null)
        {
            GUILayout.Label("class name : " + _select.className);
            bool _isChange = false;
            for (int i = 0; i < _select.tableFileList.Count; ++i)
            {
                TableFile _fileBake = _select.tableFileList[i];
                long _bakeTime = m_tableTool.tableBake.bakeInfoGroup.GetLastWriteTimeTicks(_fileBake.fileName);
                long _fileTime = _fileBake.GetLastWriteTimeTicks();
                if (_bakeTime != _fileTime)
                {
                    _isChange = true;
                }
                System.DateTime _dateTime_bakeTime = new System.DateTime(_bakeTime);
                System.DateTime _dateTime_fileTime = new System.DateTime(_fileTime);
                GUILayout.Label("-------");
                GUILayout.Label("file name : " + _fileBake.fileName);
                GUILayout.Label("sheet name : " + _fileBake.sheetName);
                GUILayout.Label("load file date : " + _dateTime_fileTime.ToLongDateString() + ", " + _dateTime_fileTime.ToLongTimeString());
                GUILayout.Label("bake Time : " + _dateTime_bakeTime.ToLongDateString() + ", " + _dateTime_bakeTime.ToLongTimeString());
            }

            GUIStyle style = new GUIStyle("Button");
            if (_isChange == true)
            {
                style.normal.textColor = Color.red;
            }
            else
            {
                style.normal.textColor = Color.white;
            }

            Draw_BakeData(_select, style);
            Draw_BakeKeyNameScript(_select);
        }

        GUILayout.EndScrollView();
    }    

    /**
     * 테이블 Bake 버튼.
     */
    void Draw_BakeData(TableFileBake _select, GUIStyle _style)
    {
        if (GUILayout.Button("Data Bake", _style))
        {
            m_tableTool.tableBake.BakeData(_select);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("", "bake complete", "OK");
        }
    }

    /**
     * keyname 스크립트 생성 버튼.
     */
    void Draw_BakeKeyNameScript(TableFileBake _select)
    {
        if (_select.IsHaveKeyName() == true && GUILayout.Button("KeyName Script Bake"))
        {
            m_tableTool.tableBake.BakeKeyNameScript(_select);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("", "bake script complete", "OK");
        }
    }
}

