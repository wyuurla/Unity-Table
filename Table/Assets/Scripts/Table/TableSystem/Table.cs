using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table<T> : ITable where T : Record, new ()
{
    public class CompareRecord : IComparer<T>
    {
        public int Compare(T x, T y) { return x.id.CompareTo(y.id); }
    }

    public List<T> list = new List<T>();
    T m_search = new T();
    CompareRecord m_compare = new CompareRecord();

    public virtual void Sort()
    {
        list.Sort(m_compare);
    }

    public virtual void Clear()
    {
        list.Clear();
    }

    public T Get(int _id, bool _isShowLog = true)
    {
        m_search.id = _id;
        int _searchIndex = list.BinarySearch(m_search, m_compare);
        if( _searchIndex < 0 )
        {
            if( _isShowLog == true )
            {
                Debug.LogError(typeof(T).ToString() + " : " + _id);
            }
            return null;
        }
        return list[_searchIndex];
    }

    public bool IsHas(int _id)
    {
        m_search.id = _id;
        return list.BinarySearch(m_search, m_compare) >= 0;
    }

    public bool Add(T _record)
    {
        if(IsHas(_record.id) == true )
        {
            Debug.LogError(typeof(T).ToString() + "::Add() have id  " + _record.id);
            return false;
        }

        list.Add(_record);
        Sort();
        return true;
    }

    public bool Delete(T _record)
    {
        if (IsHas(_record.id) == false)
        {
            Debug.LogError(typeof(T).ToString() + "::Delete() no have id  " + _record.id);
            return false;
        }

        list.Remove(_record);
        Sort();
        return true;
    }

    public virtual void Save(string _path)
    {
        string _data = JsonHelper.GetString<T>(list);        
        FileUtil.Save(_path, _data);
    }

    public virtual void Load(string _path)
    {        
        string _data = ResUtil.LoadTextAssetString(_path);
        list = JsonHelper.GetList<T>(_data);
        if (null == list)
            list = new List<T>();

        Sort();
    }
}
