using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomDataRecord
{
    public List<bool> factor_bool = new List<bool>();
    public List<float> factor_float = new List<float>();
    public List<Vector3> factor_vector = new List<Vector3>();
    public List<string> factor_string = new List<string>();
    public List<int> factor_int = new List<int>();

    public CustomDataRecord() { }
    public CustomDataRecord(CustomDataRecord _other)
    {
        SetData(_other);
    }

    public bool IsSameData(CustomDataRecord _other)
    {
        // bool
        if (factor_bool.Count != _other.factor_bool.Count)
            return false;

        for (int i = 0; i < _other.factor_bool.Count; ++i)
        {
            if (factor_bool[i] != _other.factor_bool[i])
                return false;
        }

        //float
        if (factor_float.Count != _other.factor_float.Count)
            return false;

        for (int i = 0; i < _other.factor_float.Count; ++i)
        {
            if (factor_float[i] != _other.factor_float[i])
                return false;
        }

        //vector
        if (factor_vector.Count != _other.factor_vector.Count)
            return false;

        for (int i = 0; i < _other.factor_vector.Count; ++i)
        {  
            if (factor_vector[i] != _other.factor_vector[i])
                return false;
        }

        //string
        if (factor_string.Count != _other.factor_string.Count)
            return false;

        for (int i = 0; i < _other.factor_string.Count; ++i)
        {
            if (factor_string[i] != _other.factor_string[i])
                return false;
        }


        //int
        if (factor_int.Count != _other.factor_int.Count)
            return false;

        for (int i = 0; i < _other.factor_int.Count; ++i)
        {
            if (factor_int[i] != _other.factor_int[i])
                return false;
        }

        return true;
    }

    public void SetData(CustomDataRecord _other)
    {
        factor_bool.Clear();
        for (int i = 0; i < _other.factor_bool.Count; ++i)
        {         
            factor_bool.Add(_other.factor_bool[i]);
        }

        factor_float.Clear();
        for (int i = 0; i < _other.factor_float.Count; ++i)
        {          
            factor_float.Add(_other.factor_float[i]);
        }

        factor_vector.Clear();
        for (int i = 0; i < _other.factor_vector.Count; ++i)
        {
            Vector3 _pos = _other.factor_vector[i];
            factor_vector.Add(new Vector3(_pos.x, _pos.y, _pos.z));
        }

        factor_string.Clear();
        for (int i = 0; i < _other.factor_string.Count; ++i)
        {            
            if( string.IsNullOrWhiteSpace(_other.factor_string[i]) == false )
            {
                factor_string.Add(string.Copy(_other.factor_string[i]));
            }
            else
            {
                factor_string.Add("");
            }          
        }

        factor_int.Clear();
        for (int i = 0; i < _other.factor_int.Count; ++i)
        {
            factor_int.Add(_other.factor_int[i]);
        }
    }

    public bool boolValue
    {
        get
        {
            if (factor_bool.Count <= 0)
                factor_bool.Add(false);
            return factor_bool[0];
        }
        set
        {
            if (factor_bool.Count <= 0)
            {
                factor_bool.Add(value);
            }
            else
            {
                factor_bool[0] = value;
            }
        }
    }

    public float floatValue
    {
        get
        {
            if (factor_float.Count <= 0)
                factor_float.Add(0f);
            return factor_float[0];
        }
        set
        {
            if (factor_float.Count <= 0)
            {
                factor_float.Add(value);
            }
            else
            {
                factor_float[0] = value;
            }
        }
    }

    public Vector3 vectorValue
    {
        get
        {
            if (factor_vector.Count <= 0)
                factor_vector.Add(Vector3.zero);
            return factor_vector[0];
        }
        set
        {
            if (factor_vector.Count <= 0)
            {
                factor_vector.Add(value);
            }
            else
            {
                factor_vector[0] = value;
            }
        }
    }

    public string stringValue
    {
        get
        {
            if (factor_string.Count <= 0)
                factor_string.Add(string.Empty);
            return factor_string[0];
        }
        set
        {
            if (factor_string.Count <= 0)
            {
                factor_string.Add(value);
            }
            else
            {
                factor_string[0] = value;
            }
        }
    }

    public int intValue
    {
        get
        {
            if (factor_int.Count <= 0)
                factor_int.Add(0);
            return factor_int[0];
        }
        set
        {
            if (factor_int.Count <= 0)
            {
                factor_int.Add(value);
            }
            else
            {
                factor_int[0] = value;
            }
        }
    }
}
