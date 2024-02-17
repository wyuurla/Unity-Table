[System.Serializable]
public class StringRecord : Record 
{

}
public class StringTable : Table<StringRecord> 
{
	static StringTable m_instance;
	public static StringTable Instance { get { if(null == m_instance) {return TableManager.Instance.GetTable<StringTable>();} return m_instance; } } 

}