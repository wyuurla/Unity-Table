using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief TableFile_BakeInfo
 * @detail 파일이 마지막으로 변경된 시간을 저장한다. Bake한 시간 아님 주의. 
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
[System.Serializable]
public class TableFile_BakeInfo
{
    public string fileName;
    public long lastWriteTimeTicks;
}

/**
 * @brief TableFile_BakeInfoGroup
 * @detail TableFile_BakeInfo 그룹
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
[System.Serializable]
public class TableFile_BakeInfoGroup
{
    static public bool isAutoBake
    {
        get
        {
            return PlayerPrefs.GetInt("table_tool_autobake", 0) != 0;
        }
        set
        {
            PlayerPrefs.SetInt("table_tool_autobake", value == true ? 1 : 0);
        }
    }
  
    public List<TableFile_BakeInfo> fileList = new List<TableFile_BakeInfo>();

    /**
     * Bake된 파일 이름의 마지막으로 수정된 시간을 리턴한다.
     */
    public long GetLastWriteTimeTicks(string _fileName)
    {
        _fileName = _fileName.ToLower();
        TableFile_BakeInfo _find = fileList.Find(item => item.fileName == _fileName);
        if (null == _find)
            return 0;

        return _find.lastWriteTimeTicks;
    }

    /**
     * Bake 된 파일의 마지막으로 수정된 시간을 입력한다.
     */
    public void SetFile(string _fileName, long _ticks)
    {
        TableFile_BakeInfo _find = fileList.Find(item => item.fileName == _fileName);
        if (null == _find)
        {
            fileList.Add(_find = new TableFile_BakeInfo());
            _find.fileName = _fileName;
        }

        _find.lastWriteTimeTicks = _ticks;
        Save();
    }

    /**
     * 변경된 정보를 파일로 저장한다.
     */
    public void Save()
    {
        FileUtil.Save_json<TableFile_BakeInfoGroup>(string.Format("{0}/TableFile_BakeInfoGroup.bytes", Application.persistentDataPath), this);
    }
}