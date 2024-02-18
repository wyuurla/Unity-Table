using UnityEngine;

/**
 * @brief MonoSingleton
 * @detail 여러 차레 호출되더라도 실제로 생성되는 객체는 한개만 생성하는 생성패턴.
 * @date 2024-01-30
 * @version 1.0.0
 * @author kij
 * @code 
 * class Sample : MonoSingleton<Sample> {} 
 * @endcode
 */
[DisallowMultipleComponent]
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (null == m_instance)
            {
                T[] _finds = FindObjectsOfType<T>();
                if (_finds.Length > 0)
                {
                    m_instance = _finds[0];
                    DontDestroyOnLoad(m_instance.gameObject);
                }

                if (_finds.Length > 1)
                {                   
                    for (int i = 1; i < _finds.Length; ++i)
                    {
                        Destroy(_finds[i].gameObject);
                    }

                    Debug.LogError($"There is more than one typeof(T).Name in the Scene.");
                }

                if (null == m_instance)
                {
                    GameObject _create = new GameObject(typeof(T).Name);
                    m_instance = _create.AddComponent<T>();
                    DontDestroyOnLoad(_create);
                }
            }

            return m_instance;
        }
    }
}