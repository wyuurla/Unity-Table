using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TableScriptable<T> : ITable where T : ScriptableObject
{
    protected string m_path;
    protected T m_table;

    public T tablge { get { return m_table; } }

    public TableScriptable(string _path)
    {
        m_path = _path;
    }

    public void Load(string _path)
    {
        if( ResUtil.IsHave(m_path) == true )
        {
            m_table = ResUtil.Load<T>(m_path);            
        }

        if (null == m_table)
        {
            m_table = ScriptableObject.CreateInstance<T>();
        }
    }

    public void Save(string _path)
    {
#if UNITY_EDITOR
        if (null == m_table)
            return;

        string _savePath = string.Format("Assets/Resources/{0}.asset", m_path);
        string _fullPath = string.Format("{0}/Resources/{1}", Application.dataPath, m_path);
        string _directory = System.IO.Path.GetDirectoryName(_fullPath);
    
        if (System.IO.Directory.Exists(_directory) == false)
        {
            System.IO.Directory.CreateDirectory(_directory);
        }       
        if (ResUtil.IsHave(m_path) == false)
            AssetDatabase.CreateAsset(m_table, _savePath);

        EditorUtility.SetDirty(m_table);
#endif
    }
}
