using UnityEngine;

/**
 * @brief FileUtil
 * @detail 파일에 관련된 기능을 모아놓은 클래스.
 * @date 2024-02-01
 * @version 1.0.0
 * @author kij
 */
public static class FileUtil
{        
    static public bool Save(string _path, string _data)
    {
        if( string.IsNullOrWhiteSpace(_path) == true )
        {
            Debug.LogError("FileUtil::Save()[ null == path ]");
            return false;
        }          

        if ( null == _data )
        {
            Debug.LogError($"FileUtil::Save()[null == data] path : {_path}");
            return false;
        }

        try
        {
            string _directory = System.IO.Path.GetDirectoryName(_path);
            if (System.IO.Directory.Exists(_directory) == false)
            {
                System.IO.Directory.CreateDirectory(_directory);
            }
            System.IO.File.WriteAllText(_path, _data);
        }
        catch(System.Exception _e)
        {
            Debug.LogError($"FileUtil::Save() [Exception] path : {_path}, Exception {_e.ToString()}");
            return false;
        }            

        return true;
    }
    static public string Load(string _path)
    {
        string _data = null;

        if( System.IO.File.Exists(_path) == false )
        {
            Debug.LogWarning($"FileUtil::Load() path : {_path}");
            return null;
        }

        try
        {
            _data = System.IO.File.ReadAllText(_path);
        }
        catch (System.Exception _e)
        {
            Debug.LogError($"FileUtil::Load() [Exception] path : {_path}, Exception {_e.ToString()}");
        }

        return _data;
    }

    static public T Load_json<T>(string _path) where T : new()
    {
        string _strdata = FileUtil.Load(_path);
        T  _data = JsonUtility.FromJson<T>(_strdata);
        if (null == _data)
            _data = new T();
        return _data;
    }

    static public bool Save_json<T>(string _path, T _data)
    {
        string _jsondata = JsonUtility.ToJson(_data);
        if (null == _jsondata)
            return false;
        
        return Save(_path, _jsondata);
    }
}
