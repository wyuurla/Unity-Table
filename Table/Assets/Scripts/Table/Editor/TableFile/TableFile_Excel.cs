using System.Collections.Generic;
using UnityEngine;

using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

/**
 * @brief ExcelFile
 * @detail 엑셀 파일을 읽어온다.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableFile_Excel : TableFile
{
    public TableFile_Excel(string _directory, string _fileName, string _sheetName ) : base( _directory, _fileName, _sheetName ) 
    {
    }

    /**
     * 엑셀 데이터를 로드한다.
     */
    public override bool Load()
    {
        m_data = new List<Dictionary<string, string>>();

        int _title_row = 0;
        int _data_row = 1;

        using (FileStream stream = File.Open(m_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            IWorkbook book = GetWorkBook(stream);
            if (null == book)
            {
                Debug.LogError($"[error IWorkbook == null] filename : {m_fileName}, sheet :{m_sheetName}");
                return false;
            }
            ISheet _sheet = book.GetSheet(m_sheetName);
            if (null == _sheet)
            {
                Debug.LogError($"[error GetSheet] filename : {m_fileName}, sheet :{m_sheetName}");
                return false;
            }

            IRow _nameRow = _sheet.GetRow(_title_row);
            if (_nameRow == null)
            {
                Debug.LogError($"[title IRow == null] _title_row : {_title_row}, filename : {m_fileName}, sheet :{m_sheetName}");
                return false;
            }

            var _var = _sheet.GetRowEnumerator();
            while (_var.MoveNext())
            {
                IRow _dataRow = (IRow)_var.Current;
                if (_dataRow.RowNum < _data_row)
                    continue;

                Dictionary<string, string> _parserData = GetReadLine(_nameRow, _dataRow);
                if (null == _parserData)
                    continue;

                m_data.Add(_parserData);
            }
        }

        return true;
    }

    /**
     * 확장자로 IWorkbook를 생성한다.
     */
    protected IWorkbook GetWorkBook(FileStream stream)
    {
        if (Path.GetExtension(m_filePath) == ".xls")
        {
            return new HSSFWorkbook(stream);
        }

        return new XSSFWorkbook(stream);
    }

    /**
     * 라인 한줄씩 읽어서 리턴한다.
     */
    private Dictionary<string, string> GetReadLine(IRow _row_name, IRow _row_data)
    {
        Dictionary<string, string> _readLine = new Dictionary<string, string>();

        for (int i = 0; i < _row_name.LastCellNum; ++i)
        {
            ICell _cell_name = _row_name.GetCell(i);
            if (null == _cell_name)
                continue;

            string _key = _cell_name.StringCellValue.ToLower();
            if (string.IsNullOrEmpty(_key) == true)
                continue;

            ICell _cell_data = _row_data.GetCell(i);
            if (_cell_data == null && i == _row_name.RowNum)
                return null;

            if (null == _cell_data)
            {
                _readLine.Add(_key, "");
                continue;
            }

            CellType _cellType = _cell_data.CellType;
            if (_cellType == CellType.Formula)
            {
                _cellType = _cell_data.CachedFormulaResultType;
            }

            switch (_cellType)
            {
                case CellType.Boolean:
                    _readLine.Add(_key, _cell_data.BooleanCellValue.ToString());
                    break;
                case CellType.Numeric:
                    _readLine.Add(_key, _cell_data.NumericCellValue.ToString());
                    break;
                case CellType.String:
                    _readLine.Add(_key, _cell_data.StringCellValue.ToString());
                    break;
                case CellType.Blank:
                    _readLine.Add(_key, "");
                    break;
                default:
                    Debug.LogErrorFormat("Excel File Load Error {0}, {1} : {2}, Key : {3}, idx : {4}", m_fileName, m_sheetName, _cellType, _key, _cell_data.RowIndex);
                    break;
            }
        }

        return _readLine;
    }
}