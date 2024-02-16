[System.Serializable]
public class ItemRecord : Record 
{

}
public class ItemTable : Table<ItemRecord> 
{
	public static ItemTable Instance { get { return TableManager.Instance.GetTable<ItemTable>(); } } 

}