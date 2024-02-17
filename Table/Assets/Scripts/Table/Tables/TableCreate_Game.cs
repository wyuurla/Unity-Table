using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCreate_Game : TableCreate
{
    public override Dictionary<System.Type, ITable> CreateTableList()
    {
        Dictionary<System.Type, ITable> _list = new Dictionary<System.Type, ITable>();
        return _list;
    }

    public override Dictionary<string, Dictionary<string, List<string>>> CreateEditorTableList()
    {
        Dictionary<string, Dictionary<string, List<string>>> _list = new Dictionary<string, Dictionary<string, List<string>>>();       
        return _list;
    }
}
