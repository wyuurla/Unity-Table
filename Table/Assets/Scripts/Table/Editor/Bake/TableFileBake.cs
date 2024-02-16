using System.Collections.Generic;
using UnityEngine;

/**
 * @brief TableFileBake
 * @detail 테이블의 정보를 가지고 클래스 형태로 저장.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public abstract class TableFileBake
{
    protected string m_className;
    protected List<TableFile> m_tableFileList = new List<TableFile>();

    public string className { get { return m_className; } }
    public List<TableFile> tableFileList { get { return m_tableFileList; } }
    
    public TableFileBake(string _className)
    {
        m_className = _className;
    }

    /**
     * 파일 데이터에서 로드한 데이터를 클래스 형태로 가공해서 .bytes로 저장한다.
     */
    public abstract bool BakeData();

    /**
     * 파일 데이터에 KeyName이 존재하면 id와 매칭된 스크립트를 생성한다.
     */
    public virtual bool BakeScript()
    {
        string _keyName = TableTool.keyname.ToLower();
        string _path = string.Format("{0}/{1}_{2}.cs", TableTool.directory_keynameCreate, m_className, _keyName);

        System.Text.StringBuilder _sb = new System.Text.StringBuilder();

        _sb.AppendFormat("public static class {0}_{1} \n", m_className, _keyName);
        _sb.Append("{\n");


        for (int _tableFileIndex = 0; _tableFileIndex < m_tableFileList.Count; ++_tableFileIndex)
        {
            TableFile _tableFile = m_tableFileList[_tableFileIndex];
            List<Dictionary<string, string>> _data = _tableFile.GetData();
            if (null == _data)
            {
                Debug.LogError("no find file : " + _tableFile.fileName + " sheet : " + _tableFile.sheetName);
                continue;
            }

            for (int i = 0; i < _data.Count; ++i)
            {
                if (_data[i].ContainsKey(_keyName) == false)
                {
                    Debug.LogError($"no have {_keyName} : {m_className}");
                    continue;
                }

                _sb.AppendFormat("\tstatic public readonly int {0} = {1};", _data[i][_keyName], _data[i]["id"]);
                _sb.Append("\n");
            }
        }

        _sb.Append("\n}");
        FileUtil.Save(_path, _sb.ToString());

        return true;
    }

    /**
     * 파일 데이터에 keyname이 존재하면 ture, 아니면 false를 리턴한다.
     */
    public abstract bool IsHaveKeyName();

    /**
     * 테이블 파일 리스트를 클리어한다.
     */
    public virtual void ClearFileList()
    {
        m_tableFileList.Clear();
    }

    /**
     * 가져올 파일 데이터를 추가한다.
     */
    public virtual bool AddFile(TableFile _tableFile)
    {
        if( null == _tableFile )
        {
            Debug.LogError("null==TableFile");
            return false;
        }

        if(m_tableFileList.Contains(_tableFile) == true )
        {
            Debug.LogError($"have TableFile : filename : {_tableFile.fileName}, sheetname : {_tableFile.sheetName}");
            return false;
        }

        m_tableFileList.Add(_tableFile);
        return true;
    }
}