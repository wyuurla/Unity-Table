[System.Serializable]
public class StringRecord : Record 
{

}
public class StringTable : Table<StringRecord> 
{
	public static StringTable Instance { get { return TableManager.Instance.GetTable<StringTable>(); } } 

}