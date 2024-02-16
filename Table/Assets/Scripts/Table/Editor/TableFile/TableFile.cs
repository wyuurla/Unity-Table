using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/**
 * @brief TableFile
 * @detail 테이블 파일 상위 클래스.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public abstract class TableFile
{   
    protected string m_fileName;
    protected string m_sheetName;
    protected string m_filePath;
    protected int m_filenName_HashCode;
    protected FileInfo m_fileInfo;
    protected List<Dictionary<string, string>> m_data;

    public string fileName { get { return m_fileName; } }
    public string sheetName { get { return m_sheetName; } }

    public TableFile( string _directory, string _fileName, string _sheetName )
    {
        m_fileName = _fileName.ToLower();
        m_sheetName = _sheetName.ToLower();
        m_filePath = string.Format("{0}/{1}", _directory, _fileName);
        m_filenName_HashCode = (m_fileName + m_sheetName).GetHashCode();
    }

    #region - abstract
    /**
     * 파일에서 데이터를 로드한다.
     */
    public abstract bool Load();

    #endregion

    #region - virtual 
    /**
     * 데이터를 초기화한다.
     */
    public virtual void ResetData()
    {
        m_data = null;
        m_fileInfo = null;
    }

    /**
     * 파일 이름의 해쉬코드를 리턴한다.
     */
    public virtual int GetFileHashCode()
    {
        return m_filenName_HashCode;
    }

    /**
     * 파일 정보가 들어있는 FileInfo를 리턴한다.
     */
    public virtual FileInfo GetFileInfo()
    {
        if (null == m_fileInfo)
        {
            m_fileInfo = new FileInfo(m_filePath);
        }

        return m_fileInfo;
    }

    /**
     * 파일 마지막 수정시간을 리턴한다.
     */
    public virtual long GetLastWriteTimeTicks()
    {
        return GetFileInfo().LastAccessTime.Ticks;
    }

    /**
     * 파일에서 가져온 데이터를 리턴한다.
     */
    public virtual List<Dictionary<string, string>> GetData()
    {
        if( null == m_data )
        {
            Load();
        }
        return m_data;
    }
    #endregion
}
