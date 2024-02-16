
/**
 * @brief ClassSingleton
 * @detail 여러 차레 호출되더라도 실제로 생성되는 객체는 한개만 생성하는 생성패턴.
 * @date 2024-01-30
 * @version 1.0.0
 * @author kij
 * @code 
 * class Sample : ClassSingleton<Sample> {} 
 * @endcode
 */
public class ClassSingleton<T> where T : class, new()
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = new T();
            }
            return m_instance;
        }
    }
}