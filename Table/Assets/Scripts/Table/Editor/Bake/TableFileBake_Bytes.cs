using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/**
 * @brief TableFileBake_Bytes
 * @detail 테이블의 정보를 가지고 클래스 형태의 Bytes파일로 저장한다.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableFileBake_Bytes : TableFileBake
{
    public TableFileBake_Bytes(string _className) : base(_className)
    {
    }

    public override bool IsHaveKeyName()
    {
        string _keyName = TableTool.keyname.ToLower();
        for (int i = 0; i < m_tableFileList.Count; ++i)
        {
            TableFile _tableFile = m_tableFileList[i];

            List<Dictionary<string, string>> _data = _tableFile.GetData();
            if (null == _data)
            {
                continue;
            }

            if (_data.Count <= 0)
            {
                continue;
            }

            if (_data[0].ContainsKey(_keyName) == true)
                return true;
        }

        return false;
    }

    public override bool BakeData()
    {
        System.Type _systemType = GetTypeFromAssemblies(m_className);
        object asset = Activator.CreateInstance(_systemType);
        
        var _classFields = _systemType.GetFields();

        for (int _fieldCnt = 0; _fieldCnt < _classFields.Length; ++_fieldCnt)
        {
            var _classField = _classFields[_fieldCnt];
            Type fieldType = _classField.FieldType;
            if (!fieldType.IsGenericType || (fieldType.GetGenericTypeDefinition() != typeof(List<>)))
                continue;

            Type[] _argument = fieldType.GetGenericArguments();
            if (_argument.Length >= 2)
            {
                Debug.LogError("_argument.Length >= 2");
                return false;
            }

            Type _recordType = _argument[0];
            Type listType = typeof(List<>).MakeGenericType(_recordType);
            MethodInfo listAddMethod = listType.GetMethod("Add", new Type[] { _recordType });
            object list = Activator.CreateInstance(listType);
            
            for( int _tableFileIndex=0; _tableFileIndex < m_tableFileList.Count; ++_tableFileIndex)
            {
                TableFile _tableFile = m_tableFileList[_tableFileIndex];               
                List<Dictionary<string, string>> _data = _tableFile.GetData();
                if (null == _data)
                {
                    Debug.LogError($"[no find file] filename : {_tableFile.fileName}, sheetName : {_tableFile.sheetName}");
                    return false;
                }

                for (int i = 0; i < _data.Count; ++i)
                {
                    var entity = GetRecordFromRow(_data[i], _recordType);
                    listAddMethod.Invoke(list, new object[] { entity });
                }
            }

            _classField.SetValue(asset, list);
        }

        string _directory = TableTool.directory_tableSave;
        if (System.IO.Directory.Exists(_directory) == false)
        {
            System.IO.Directory.CreateDirectory(_directory);
        }

        string _path = string.Format("{0}/{1}.bytes", _directory, m_className);
        MethodInfo _method = _systemType.GetMethod("Save");
        _method.Invoke(asset, new object[] { _path });

        return true;
    }

    object GetRecordFromRow(Dictionary<string, string> _data, Type _recordType)
    {
        var _record = Activator.CreateInstance(_recordType);
        var _recordFields = _recordType.GetFields();
        string _showClassName = _recordType.ToString();

        for (int _recordFieldCnt = 0; _recordFieldCnt < _recordFields.Length; ++_recordFieldCnt)
        {
            var _recordField = _recordFields[_recordFieldCnt];

            object _recordValue = null;
            if (false == GetRecordValue(ref _recordValue, _showClassName, _recordField, _recordField.Name, _data))
            {
                Type _classType = _recordField.FieldType;

                if (_classType.IsGenericType && (_classType.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    Type[] _argument = _classType.GetGenericArguments();
                    if (_argument.Length >= 2)
                    {
                        Debug.LogError("_argument.Length >= 2");
                    }
                    else
                    {
                        Type _argumentType = _argument[0];
                        var _argumentFields = _argumentType.GetFields();
                        Type listType = typeof(List<>).MakeGenericType(_argumentType);
                        MethodInfo listAddMethod = listType.GetMethod("Add", new Type[] { _argumentType });
                        _recordValue = Activator.CreateInstance(listType);

                        object _argumentObject = GetRecordList(_showClassName, _argumentType, _recordField.Name, _data);
                        if (null != _argumentObject)
                        {
                            ArrayList _temp = (ArrayList)_argumentObject;
                            for (int i = 0; i < _temp.Count; ++i)
                            {
                                listAddMethod.Invoke(_recordValue, new object[] { _temp[i] });
                            }
                        }
                        else
                        {
                            int _index = 1;
                            while (true)
                            {
                                string _excelName = string.Format("{0}_{1}", _recordField.Name, _index);
                                _argumentObject = GetClassListFieldObject(_showClassName, _argumentType, _excelName, _data);
                                if (null == _argumentObject)
                                    break;
                                listAddMethod.Invoke(_recordValue, new object[] { _argumentObject });
                                ++_index;
                            }
                        }
                    }
                }
                else
                {
                    _recordValue = GetRecordList(_showClassName, _classType, _recordField.Name, _data);
                }
            }
            _recordField.SetValue(_record, _recordValue);
        }

        return _record;
    }

    bool GetRecordValue(ref object _recordValue, string _showClassName, FieldInfo _fieldInfo, string _sheetName, Dictionary<string, string> _data)
    {
        if (_fieldInfo.FieldType == typeof(int))
        {
            _recordValue = Get<int>(_showClassName, _data, _sheetName);
        }
        else if (_fieldInfo.FieldType == typeof(string))
        {
            _recordValue = Get<string>(_showClassName, _data, _sheetName);
        }
        else if (_fieldInfo.FieldType == typeof(float))
        {
            _recordValue = Get<float>(_showClassName, _data, _sheetName);
        }
        else if (_fieldInfo.FieldType == typeof(long))
        {
            _recordValue = Get<long>(_showClassName, _data, _sheetName);
        }
        else if (_fieldInfo.FieldType == typeof(bool))
        {
            _recordValue = Get<bool>(_showClassName, _data, _sheetName);
        }
        else if (_fieldInfo.FieldType == typeof(double))
        {
            _recordValue = Get<double>(_showClassName, _data, _sheetName);
        }
        else if (_fieldInfo.FieldType.IsEnum)
        {
            try
            {
                string _parser = Get<string>(_showClassName, _data, _sheetName);
                if (false == string.IsNullOrEmpty(_parser))
                {
                    _recordValue = System.Enum.Parse(_fieldInfo.FieldType, _parser);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(_showClassName + "::" + _fieldInfo.FieldType + ", sheetname : " + _sheetName + " : " + e.ToString());
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    object GetRecordList(string _showClassName, Type _type, string _sheetName, Dictionary<string, string> _data)
    {
        ArrayList _list = null;

        if (_type == typeof(int))
        {
            _list = new ArrayList();
            _list.AddRange(GetList<int>(_showClassName, _data, _sheetName));
        }
        else if (_type == typeof(string))
        {
            _list = new ArrayList();
            _list.AddRange(GetList<string>(_showClassName, _data, _sheetName));
        }
        else if (_type == typeof(float))
        {
            _list = new ArrayList();
            _list.AddRange(GetList<float>(_showClassName, _data, _sheetName));
        }
        else if (_type == typeof(long))
        {
            _list = new ArrayList();
            _list.AddRange(GetList<long>(_showClassName, _data, _sheetName));
        }
        else if (_type == typeof(bool))
        {
            _list = new ArrayList();
            _list.AddRange(GetList<bool>(_showClassName, _data, _sheetName));
        }
        else if (_type == typeof(double))
        {
            _list = new ArrayList();
            _list.AddRange(GetList<double>(_showClassName, _data, _sheetName));
        }

        Debug.LogError("no code");
        return _list;
    }

    object GetClassListFieldObject(string _showClassName, Type _classType, string _excelClassName, Dictionary<string, string> _data)
    {
        FieldInfo[] _classFields = _classType.GetFields();
        object _recordValue = Activator.CreateInstance(_classType);
        for (int _classFieldIdx = 0; _classFieldIdx < _classFields.Length; ++_classFieldIdx)
        {
            var _classField = _classFields[_classFieldIdx];
            string _excelName = string.Format("{0}_{1}", _excelClassName, _classField.Name).ToLower();

            if (_data.ContainsKey(_excelName) == false)
            {
                return null;
            }

            object _clasValue = null;
            if (false == GetRecordValue(ref _clasValue, _showClassName, _classField, _excelName, _data))
            {
                return null;
            }

            if (_classFieldIdx == 0 && _clasValue == null)
            {
                return null;
            }

            _classField.SetValue(_recordValue, _clasValue);
        }

        return _recordValue;
    }

    T Get<T>(string _class, string _key, string _value, T def = default(T))
    {
        if (string.IsNullOrEmpty(_value) == true)
        {
            return def;
        }

        try
        {
            if (true == typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), _value, true);
            }
            else if (typeof(int) == typeof(T))
            {
                return (T)((object)int.Parse(_value));
            }
            else if (typeof(long) == typeof(T))
            {
                return (T)((object)long.Parse(_value));
            }
            else if (typeof(float) == typeof(T))
            {
                return (T)((object)float.Parse(_value));
            }
            else if (typeof(bool) == typeof(T))
            {
                return (T)((object)bool.Parse(_value));
            }
            else if (typeof(double) == typeof(T))
            {
                return (T)((object)double.Parse(_value));
            }
            else if (typeof(string) == typeof(T))
            {
                return (T)((object)_value);
            }
            else
            {
                Debug.LogErrorFormat(_class + "Get<{0}> - [{1}] : {2}", typeof(T).ToString(), _key, _value);
            }
        }
        catch
        {
            Debug.LogErrorFormat(_class + "Get catch <{0}> - [{1}] : {2}", typeof(T).ToString(), _key, _value);
        }

        return def;
    }

    T Get<T>(string _class, Dictionary<string, string> _data, string _key, T def = default(T))
    {
        _key = _key.ToLower();

        if (false == _data.ContainsKey(_key))
        {
            Debug.LogErrorFormat(_class + "::Get<{0}>[not find] : {1}", typeof(T).ToString(), _key);
            return def;
        }
        return Get<T>(_class, _key, _data[_key], def);
    }

    List<T> GetList<T>(string _class, Dictionary<string, string> _data, string _key)
    {
        List<T> _list = new List<T>();

        string _value = Get<string>(_class, _data, _key);
        if (string.IsNullOrEmpty(_value) == true)
            return _list;

        int _idx_firset = Mathf.Clamp(_value.IndexOf('[') + 1, 0, _value.Length - 1);
        int _idx_end = Mathf.Clamp(_value.IndexOf(']') - 1, 0, _value.Length - 1);
        _value = _value.Substring(_idx_firset, _idx_end);
        _value = _value.Replace(" ", "");
        string[] _valueList = _value.Split(',');

        for (int i = 0; i < _valueList.Length; i++)
        {
            if (true == string.IsNullOrEmpty(_valueList[i]))
            {
                continue;
            }

            _list.Add(Get<T>(_class, _key, _valueList[i]));
        }
        return _list;
    }
  
    System.Type GetTypeFromAssemblies(string TypeName)
    {
        var type = System.Type.GetType(TypeName);
        if (type != null)
            return type;

        var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (var assemblyName in referencedAssemblies)
        {
            var assembly = System.Reflection.Assembly.Load(assemblyName);
            if (assembly != null)
            {
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;
            }
        }
        return null;
    }
}
