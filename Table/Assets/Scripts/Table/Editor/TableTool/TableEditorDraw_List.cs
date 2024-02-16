using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/**
 * @brief TableEditorDraw_List
 * @detail 테이블 클래스 리스트를 표시한다.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableEditorDraw_List : TableEditorDraw
{
    protected Vector2 m_scrollbar = Vector2.zero;
    protected string m_className;
    public TableFileBake select; 
    protected string m_strKeyName = "-K";
    protected string m_strNeedBake = "-S";

    public TableEditorDraw_List(TableTool _tool) : base(_tool)
    {
    }

    public override void Draw()
    {
        base.Draw();

        m_scrollbar = GUILayout.BeginScrollView(m_scrollbar, GUILayout.ExpandHeight(true), GUILayout.Width(150));

        Draw_TableScriptCreate();
        Draw_ClassList();
        GUILayout.EndScrollView();
    }

    /**
     * 테이블 스크립트를 생성에디터.
     */
    void Draw_TableScriptCreate()
    {
        GUILayout.BeginHorizontal();
        m_className = EditorGUILayout.TextField(m_className);
        if (GUILayout.Button("CREATE", GUILayout.Width(60)))
        {
            m_tableTool.tableBake.CreateTableScript(m_className);
        }
        GUILayout.EndHorizontal();
    }

    /**
     * 테이블 클래스 리스트.
     */
    void Draw_ClassList()
    {
        for (int i = 0; i < m_tableTool.tableBake.bakeList.Count; ++i)
        {
            TableFileBake _tableFileBake = m_tableTool.tableBake.bakeList[i];
            if (null == _tableFileBake)
                continue;

            bool _isCheck = false;
            if (select != null)
            {
                _isCheck = select == _tableFileBake;
            }
            string _dest = _tableFileBake.className;
            if( _tableFileBake.IsHaveKeyName() == true )
            {
                _dest += m_strKeyName;
            }
            if( m_tableTool.tableBake.IsNeedBake(_tableFileBake))
            {
                _dest += m_strNeedBake;
            }

            bool _isToggleCheck = GUILayout.Toggle(_isCheck, _dest, GUILayout.ExpandWidth(true));
            if (_isCheck == false && _isToggleCheck == true)
            {
                select = _tableFileBake;
                GUIUtility.keyboardControl = 0;
            }
        }
    }
}