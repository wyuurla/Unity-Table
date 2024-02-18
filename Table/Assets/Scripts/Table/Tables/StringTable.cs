[System.Serializable]
public class StringRecord : Record 
{
	public string kor;
	public string eng;
}
public class StringTable : Table<StringRecord> 
{
	public static StringTable Instance { get { return TableManager.Instance.GetTable<StringTable>(); } }

	public string GetText(int _id, UnityEngine.SystemLanguage _language)
	{
		StringRecord _record = Get(_id);
		if (null == _record)
		{
			return $"no have id {_id}";
		}

		switch (_language)
		{
			case UnityEngine.SystemLanguage.Korean:
				return _record.kor;
			case UnityEngine.SystemLanguage.English:
				return _record.eng;
			default:
				UnityEngine.Debug.LogError($"no code {_language}");
				break;
		}

		return _record.eng;
	}
}