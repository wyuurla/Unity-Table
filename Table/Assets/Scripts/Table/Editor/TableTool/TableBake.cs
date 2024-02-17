using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * @brief TableBake
 * @detail 테이블툴에서 사용하는 테이블 Bake 기능을 관리한다.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableBake
{
    private TableFileGroup m_fileGroup;
    private TableFile_BakeInfoGroup m_bakeInfoGroup;
    private List<TableFileBake> m_bakeList = new List<TableFileBake>();
    private TableCreate m_tableCreate;
    private float m_autoTime = 0f;
    private float m_maxAutoTime = 1f;

    public List<TableFileBake> bakeList { get { return m_bakeList; } }

    public TableFileGroup fileGroup
    {
        get
        {
            if (null == m_fileGroup)
            {
                m_fileGroup = new TableFileGroup(TableTool.directory_tableLoad);
            }
            return m_fileGroup;
        }
    }

    public TableCreate tableCreate
    {
        get
        {
            if (null == m_tableCreate)
            {
                m_tableCreate = new TableCreate_Game();
            }
            return m_tableCreate;
        }
    }

    public TableFile_BakeInfoGroup bakeInfoGroup
    {
        get
        {
            if (m_bakeInfoGroup == null)
            {
                m_bakeInfoGroup = FileUtil.Load_json<TableFile_BakeInfoGroup>(string.Format("{0}/TableFile_BakeInfoGroup.bytes", Application.persistentDataPath));
            }

            return m_bakeInfoGroup;
        }
    }

    public TableBake()
    {
        m_bakeList.Clear();
        Dictionary<string, Dictionary<string, List<string>>> _list = tableCreate.CreateEditorTableList();
        var _var_classname = _list.GetEnumerator();
        while (_var_classname.MoveNext())
        {
            var _var_filename = _var_classname.Current.Value.GetEnumerator();
            while (_var_filename.MoveNext())
            {
                TableFileBake _builder = new TableFileBake_Bytes(_var_classname.Current.Key);
                TableFileBake _find = m_bakeList.Find(item => item.className == _var_classname.Current.Key);
                if (_find != null )
                {
                    Debug.LogError("tableFileBakeList have key : " + _var_classname.Current.Key);
                    continue;
                }

                m_bakeList.Add(_builder);
                for (int i = 0; i < _var_filename.Current.Value.Count; ++i)
                {
                    _builder.AddFile(fileGroup.AddExcel(_var_filename.Current.Key, _var_filename.Current.Value[i]));
                }
            }
        }
    }

    /**
     * 모든 테이블의 데이터와 스크립트를 Bake한다.
     */
    public bool AllBake()
    {
        fileGroup.ResetData();
        for(int i=0;i< m_bakeList.Count; ++i)
        {
            BakeData(m_bakeList[i]);
            BakeKeyNameScript(m_bakeList[i]);
        }
        return true;
    }

    /**
    * 테이블의 데이터를 Bake한다.
    */
    public bool BakeData(TableFileBake _select)
    {
        if (null == _select)
            return false;

        fileGroup.ResetData();

        if (_select.BakeData() == false)
            return false;

        for( int i=0; i< _select.tableFileList.Count; ++i )
        {
            bakeInfoGroup.SetFile(_select.tableFileList[i].fileName, _select.tableFileList[i].GetLastWriteTimeTicks());
        }
        return true;
    }

    /**
    * 테이블의 KeyName스크립트를 Bake한다.
    */
    public bool BakeKeyNameScript(TableFileBake _select)
    {
        if (null == _select)
            return false;

        if (_select.IsHaveKeyName() == false)
            return false;

        fileGroup.ResetData();

        if (_select.BakeScript() == false )
            return false;

        return true;
    }

    /**
    * 파일이 마지막으로 수정된 시간과 Bake한 시간을 비교해서 같으면 false 다르면 true를 리턴한다.
    */
    public bool IsNeedBake(TableFileBake _bake)
    {
        for( int i=0; i< _bake.tableFileList.Count; ++i )
        {
            TableFile _file = _bake.tableFileList[i];
            if (bakeInfoGroup.GetLastWriteTimeTicks(_file.fileName) != _file.GetLastWriteTimeTicks())
                return true;
        }

        return false;
    }

    /**
     * 테이블 스크립트를 만든다.
     */
    public bool CreateTableScript(string _className)
    {
        if( string.IsNullOrWhiteSpace(_className) == true )
        {
            EditorUtility.DisplayDialog("error", "null == className", "OK");
            return false;
        }
        string _path = string.Format("{0}/{1}Table.cs", TableTool.directory_tableScriptCreate, _className);

        if( System.IO.File.Exists(_path) == true )
        {
            EditorUtility.DisplayDialog("error", $"exist file {_path}", "OK");
            return false;
        }

        System.Text.StringBuilder _sb = new System.Text.StringBuilder();

        _sb.Append("[System.Serializable]\n");
        _sb.AppendFormat("public class {0}Record : Record \n", _className);
        _sb.Append("{\n");
        _sb.Append("\n}");
        _sb.Append("\n");
        _sb.AppendFormat("public class {0}Table : Table<{0}Record> \n", _className);
        _sb.Append("{\n");
        _sb.AppendFormat("\tstatic {0}Table m_instance;\n", _className);
        _sb.AppendFormat("\tpublic static {0}Table Instance {{ get {{ if(null == m_instance) {{return TableManager.Instance.GetTable<{0}Table>();}} return m_instance; }} }} \n", _className);        
        _sb.Append("\n}");

        if (FileUtil.Save(_path, _sb.ToString()) == false)
            return false;

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("success", "create table script", "OK");

        return true;
    }

 
    public void UpdateLogic()
    {
        if (m_autoTime < m_maxAutoTime)
            m_autoTime += Time.deltaTime;

        if (TableFile_BakeInfoGroup.isAutoBake == true && m_autoTime>= m_maxAutoTime )
        {
            m_autoTime = 0f;
            fileGroup.ResetData();
            for (int i = 0; i < m_bakeList.Count; ++i)
            {
                if(IsNeedBake(m_bakeList[i]) == true )
                {
                    BakeData(m_bakeList[i]);
                    BakeKeyNameScript(m_bakeList[i]);
                }
            }
        }
    }
}
