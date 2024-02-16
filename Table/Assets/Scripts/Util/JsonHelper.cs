using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class JsonHelper
{
    [System.Serializable]
    private class WrapperList<T>
    {
        public List<T> items;
    }
    static public List<T> GetList<T>(string _json)
    {
        if (string.IsNullOrWhiteSpace(_json) == true)
            return null;

        try
        {
            WrapperList<T> wrapper = JsonUtility.FromJson<WrapperList<T>>(_json);
            if (null == wrapper)
            {
                Debug.LogErrorFormat("error JsonHelper : {0}", _json);
                return null;
            }
            return wrapper.items;
        }
        catch( System.Exception _e )
        {
            Debug.LogError("JsonHelper::GetList() : " + _e.ToString());
            return null;
        }            
    }
    static public string GetString<T>(List<T> array)
    {
        try
        {
            WrapperList<T> wrapper = new WrapperList<T>();
            wrapper.items = array;
            return JsonUtility.ToJson(wrapper);
        }
        catch (System.Exception _e)
        {
            Debug.LogError("JsonHelper::GetString() : " + _e.ToString());
            return null;
        }
    }
}