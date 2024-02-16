using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief ResUtil
 * @detail Resources 폴더에 들어있는 에셋에 관리하는 기능을 모아놓은 클래스.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public static class ResUtil
{

    public static T Load<T>(string _path) where T : Object
    {
        if (string.IsNullOrWhiteSpace(_path) == true)
        {
            Debug.LogError($"ResUtil::Load() null == path : {_path}");
            return null;
        }

        T _res = Resources.Load<T>(_path);
        if (null == _res)
        {
            Debug.LogError($"ResUtil::Load() No have File : {_path}");
            return null;
        }

        return _res;
    }

    static public string LoadTextAssetString(string _path)
    {
        TextAsset _asset = Load<TextAsset>(_path);
        if (null == _asset)
            return null;

        return _asset.text;
    }

    public static Sprite LoadMultSprite(string _path, int _index)
    {
        if (string.IsNullOrWhiteSpace(_path) == true)
        {
            Debug.LogError($"ResUtil::Load() null == path : {_path}");
            return null;
        }

        Sprite[] _res = null;

        if (null == _res)
        {
            _res = Resources.LoadAll<Sprite>(_path);
        }

        if (null == _res)
        {
            Debug.LogError($"ResUtil::Load() No have File : {_path}");
            return null;
        }

        if (_res.Length <= _index)
        {
            Debug.LogError($"ResUtil::Load() over index : {_path}, index : {_index}");
            return null;
        }

        if (_index < 0)
        {
            Debug.LogError($"ResUtil::Load() over index : {_path}, index : {_index}" );
            return null;
        }

        return _res[_index];
    }

    public static GameObject Create(string _path, Transform _parent)
    {
        GameObject _res = Load<GameObject>(_path);
        if (null == _res)
            return null;

        GameObject _ins = GameObject.Instantiate<GameObject>(_res);
        SetAttach(_parent, _ins.transform, _res.transform);
        return _ins;
    }

    public static T Create<T>(string _path, Transform _parent) where T : Component
    {
        T _res = Load<T>(_path);
        if (null == _res)
            return null;

        T _ins = GameObject.Instantiate<T>(_res);
        if (null == _ins)
        {
            Debug.LogError($"ResUtil::Create() no component : {_path}");
            return null;
        }

        SetAttach(_parent, _ins.transform, _res.transform);
        return _ins;
    }

    public static void SetAttach(Transform _parent, Transform _chield, Transform _res = null)
    {
        if (null == _parent)
            return;

        if (null == _chield)
            return;
        _chield.transform.SetParent(_parent);

        if (null == _res)
        {
            _chield.transform.localPosition = Vector3.zero;
            _chield.transform.localRotation = Quaternion.identity;
            _chield.transform.localScale = Vector3.one;
        }
        else
        {
            _chield.transform.localPosition = _res.transform.localPosition;
            _chield.transform.localRotation = _res.transform.localRotation;
            _chield.transform.localScale = _res.transform.localScale;
        }
    }

    public static bool IsHave(string _path)
    {
        return Resources.Load(_path) != null;
    }

    public static Transform FindName(Transform _trn, string _name)
    {
        if (string.Compare(_trn.name, _name, true) == 0)
            return _trn;

        for (int i = 0; i < _trn.childCount; ++i)
        {
            Transform child = FindName(_trn.GetChild(i), _name);

            if (child != null)
                return child;
        }

        return null;
    }
}
