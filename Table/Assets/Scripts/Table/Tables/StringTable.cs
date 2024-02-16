using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class StringRecord : Record
{   
    public string kor;
    public string eng;
 
    public string GetText(SystemLanguage _language)
    {
        switch (_language)
        {
            case SystemLanguage.Korean:
                return kor;
            default:
                Debug.LogError($"no code {_language}");
                break;
        }

        return kor;
    }
}

[System.Serializable]
public class StringTable : Table<StringRecord>
{
    public static StringTable Instance { get { return TableManager.Instance.GetTable<StringTable>(); } }
    
    public virtual string GetText(int _index, SystemLanguage _language)
    {
        StringRecord _record = Get(_index);
        if (null == _record)
        {
            return "Text: " + _index.ToString();
        }
       
        return _record.GetText(_language);
    }
}
