using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief TableFileGroup
 * @detail ���̺� ������ ��� ���� Ŭ����.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableFileGroup
{
    Dictionary<int, TableFile> m_fileList = new Dictionary<int, TableFile>();
    string m_directory;

    public TableFileGroup(string _directory)
    {
        m_directory = _directory;
    }

    /**
     * �����̸��� ��Ʈ�̸��� ���� ������ ������ �����Ѵ�.
     */
    public List<Dictionary<string, string>> GetData(string _filename, string _sheetName)
    {
        int _hashcode = (_filename.ToLower() + _sheetName.ToLower()).GetHashCode();
        if (m_fileList.ContainsKey(_hashcode) == false)
            return null;

        return m_fileList[_hashcode].GetData();
    }

    /**
     * �����̸��� ��Ʈ�̸��� ���� ������ ������ ���� �ð��� �����Ѵ�.
     */
    public long GetLastWriteTime(string _filename, string _sheetName)
    {
        int _hashcode = (_filename.ToLower() + _sheetName.ToLower()).GetHashCode();
        if (m_fileList.ContainsKey(_hashcode) == false)
            return 0;

        return m_fileList[_hashcode].GetLastWriteTimeTicks();
    }

    /**
     * ���� ������ �߰��Ѵ�.
     */
    public TableFile AddExcel(string _fileName, string _sheetName)
    {
        _fileName = _fileName.ToLower();
        _sheetName = _sheetName.ToLower();

        int _hashcode = (_fileName + _sheetName).GetHashCode();

        if (m_fileList.ContainsKey(_hashcode) == true)
        {
            return m_fileList[_hashcode];
        }

        TableFile_Excel _tableFile_excel = new TableFile_Excel(m_directory, _fileName, _sheetName);
        m_fileList.Add(_hashcode, _tableFile_excel);
        return _tableFile_excel;
    }

    public void ResetData()
    {
        var _var = m_fileList.GetEnumerator();
        while(_var.MoveNext())
        {
            _var.Current.Value.ResetData();
        }
    }
}
