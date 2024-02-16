using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : ClassSingleton<TableManager>
{   
    private Dictionary<System.Type, ITable> m_tableList;
    
    public TableManager()
    {
        Load();
    }

    /**
     * 테이블을 로드한다.
     */
    public void Load()
    {     
        m_tableList = new TableCreate_Game().CreateTableList();
        var _var = m_tableList.GetEnumerator();
        while(_var.MoveNext())
        {
            _var.Current.Value.Load(string.Format("Table/{0}", _var.Current.Key.Name));
        }
    }

    /**
     * Table을 리턴한다.
     */
    public T GetTable<T>() where T : class, ITable
    {
        System.Type _type = typeof(T);

        ITable _find = null;
        if(m_tableList.TryGetValue(_type, out _find) == false )
        {
            Debug.LogError(this.ToString() + "::GetTable() : " + _type.ToString());
            return null;
        }

        return _find as T;
    }
}
