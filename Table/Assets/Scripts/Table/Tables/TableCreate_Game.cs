using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCreate_Game : TableCreate
{
    public override Dictionary<System.Type, ITable> CreateTableList()
    {
        Dictionary<System.Type, ITable> _list = new Dictionary<System.Type, ITable>();
        AddTable<StringTable>(_list);
        return _list;
    }

    public override Dictionary<string, Dictionary<string, List<string>>> CreateEditorTableList()
    {
        Dictionary<string, Dictionary<string, List<string>>> _list = new Dictionary<string, Dictionary<string, List<string>>>();
        AddEditorTable(_list, "StringTable", "StringTable.xlsx", "StringTable");
        return _list;
    }
}
