// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Cloning;
using Duality.Editor;

using SnowyPeak.Duality.Plugin.Data.Properties;

namespace SnowyPeak.Duality.Plugin.Data.Resources
{
    /// <summary>
    /// Allows Row/Column based access to a valid CSV file
    /// </summary>
    [Serializable]
    [EditorHintCategory(typeof(Res), ResNames.CategoryData)]
    [EditorHintImage(typeof(Res), ResNames.ImageCsv)]
    public class CsvData : TextFile
    {
        /// <summary>
        /// A CsvFile Resource file extension.
        /// </summary>
        public new static string FileExt = ".CsvData" + Resource.FileExt;

        private static string[] EmptyData = new string[0];

        private Dictionary<string, int> _colNames;
        private int _columns;
        private string[] _dataMatrix;
        private char _fieldSeparator;
        private bool _isFirstColumnIndex;
        private bool _isFirstRowHeader;
        private bool _isValid;
        private Dictionary<string, int> _rowIndexes;
        private int _rows;
        private char _stringQualifier;

        /// <summary>
        /// Creates a new, empty CsvFile.
        /// </summary>
        public CsvData()
        {
            _fieldSeparator = ';';
            _stringQualifier = '"';

            _rowIndexes = new Dictionary<string, int>();
            _colNames = new Dictionary<string, int>();
        }

        /// <summary>
        /// [GET] The number of Columns present in the file.
        /// </summary>
        public int Columns
        {
            get { return _columns; }
        }
        /// <summary>
        /// [GET / SET] The character used as field separator in the file. Defaults as ';'.
        /// </summary>
        public string FieldSeparator
        {
            get { return Convert.ToString(_fieldSeparator); }
            set { _fieldSeparator = value[0]; Parse(); }
        }
        /// <summary>
        /// [GET / SET] Indicates if the first field of each line in the source file has to be treated as a unique
        /// index that can be used to directly access the relative row of data.
        /// </summary>
        public bool IsFirstColumnIndex
        {
            get { return _isFirstColumnIndex; }
            set { _isFirstColumnIndex = value; Parse(); }
        }
        /// <summary>
        /// [GET / SET] Indicates if the first row of the file has to be treated as column headers so that they can be used
        /// to directly access the realative field of data.
        /// </summary>
        public bool IsFirstRowHeader
        {
            get { return _isFirstRowHeader; }
            set { _isFirstRowHeader = value; Parse(); }
        }
        /// <summary>
        /// [GET] Indicates if the csv file is valid (i.e. all rows are comprised of the same number of values).
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }
        /// <summary>
        /// [GET] The number of rows loaded, including the Header row (if present).
        /// </summary>
        public int Rows
        {
            get { return _rows; }
        }
        /// <summary>
        /// [GET / SET] The string qualifier character is removed from the beginning and the end of all values.
        /// </summary>
        public string StringQualifier
        {
            get { return Convert.ToString(_stringQualifier); }
            set
            {
                _stringQualifier = value.Length > 0 ? value[0] : '"';
                Parse();
            }
        }
        /// <summary>
        /// Access the data by row and column.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [EditorHintFlags(MemberFlags.Invisible)]
        public string this[int row, int column]
        {
            get { return _dataMatrix[row * _rows + column]; }
            private set { _dataMatrix[row * _rows + column] = value; }
        }
        /// <summary>
        /// Access the data by row and field.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        [EditorHintFlags(MemberFlags.Invisible)]
        public string this[int row, string field]
        {
            get { return this[row, GetColumnWithName(field)]; }
        }
        /// <summary>
        /// Access the data by index and column.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [EditorHintFlags(MemberFlags.Invisible)]
        public string this[string index, int column]
        {
            get { return this[GetRowWithIndex(index), column]; }
        }
        /// <summary>
        /// Access the data by index and field.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        [EditorHintFlags(MemberFlags.Invisible)]
        public string this[string index, string field]
        {
            get { return this[GetRowWithIndex(index), GetColumnWithName(field)]; }
        }
        private string this[int? row, int? column]
        {
            get
            {
                if (!row.HasValue || !column.HasValue)
                    return null;
                else
                    return this[row.Value, column.Value];
            }
        }

        /// <summary>
        /// Get a single column of data, accessed by column.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public IEnumerable<string> GetColumn(int column)
        {
            return _dataMatrix.Where((s, i) => (i % _columns) == column);
        }

        /// <summary>
        /// Get a single column of data, accessed by field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IEnumerable<string> GetColumn(string field)
        {
            return GetColumn(GetColumnWithName(field));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string GetDefaultExtension()
        {
            return ".csv";
        }

        /// <summary>
        /// Access the data by row and column and returns it converted to the desired type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public T GetItemAs<T>(int row, int column)
        {
            return (T)Convert.ChangeType(this[row, column], typeof(T));
        }

        /// <summary>
        /// Access the data by index and column and returns it converted to the desired type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public T GetItemAs<T>(string index, int column)
        {
            return GetItemAs<T>(GetRowWithIndex(index), column);
        }

        /// <summary>
        /// Access the data by row and field and returns it converted to the desired type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public T GetItemAs<T>(int row, string field)
        {
            return GetItemAs<T>(row, GetColumnWithName(field));
        }

        /// <summary>
        /// Access the data by index and field and returns it converted to the desired type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public T GetItemAs<T>(string index, string field)
        {
            return GetItemAs<T>(GetRowWithIndex(index), GetColumnWithName(field));
        }

        /// <summary>
        /// Get a single row of data, accessed by row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRow(int row)
        {
            return _dataMatrix.Skip(row * _columns).Take(_columns);
        }

        /// <summary>
        /// Get a single row of data, accessed by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRow(string index)
        {
            return GetRow(GetRowWithIndex(index));
        }

        /// <summary>
        ///
        /// </summary>
        protected override void AfterReload()
        {
            base.AfterReload();
            Parse();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="operation"></param>
        protected override void OnCopyDataTo(object target, ICloneOperation operation)
        {
            base.OnCopyDataTo(target, operation);
            CsvData targetCsv = target as CsvData;

            targetCsv.Parse();
        }

        private IEnumerable<string> GetColumn(int? column)
        {
            if (!column.HasValue)
                return null;
            else
                return GetColumn(column.Value);
        }

        private int? GetColumnWithName(string name)
        {
            if (!_isFirstRowHeader)
            {
                Log.Editor.WriteError("ColumnName access is disabled");
            }

            if (name == null || !_colNames.ContainsKey(name))
            {
                Log.Editor.WriteWarning("Column {0} not found", name);
                return null;
            }
            else
                return _colNames[name];
        }

        private T GetItemAs<T>(int? row, int? column)
        {
            if (!row.HasValue || !column.HasValue)
                return default(T);
            else
                return GetItemAs<T>(row.Value, column.Value);
        }

        private IEnumerable<string> GetRow(int? row)
        {
            if (!row.HasValue)
                return null;
            else
                return GetRow(row.Value);
        }

        private int? GetRowWithIndex(string index)
        {
            if (!_isFirstColumnIndex)
            {
                Log.Editor.WriteError("Index access is disabled");
            }

            if (index == null || !_rowIndexes.ContainsKey(index))
            {
                Log.Editor.WriteWarning("Row {0} not found", index);
                return null;
            }
            else
                return _rowIndexes[index];
        }

        private void Parse()
        {
            int cellsPerLine = -1;
            _isValid = true;

            _rowIndexes.Clear();
            _colNames.Clear();
            _dataMatrix = EmptyData;

            int row = 0;
            string[] lines = _content.Split(TextFile.NewLineSplitter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] lineData = line.Split(_fieldSeparator);

                if (cellsPerLine != -1 && cellsPerLine != lineData.Length)
                {
                    Log.Editor.WriteError("Row {0} has different numbers of records", row);
                    _isValid = false;
                    break;
                }
                else
                {
                    cellsPerLine = lineData.Length;

                    if (_dataMatrix == EmptyData)
                    {
                        _dataMatrix = new string[lines.Length * cellsPerLine];
                        _rows = lines.Length;
                        _columns = cellsPerLine;
                    }

                    if (row == 0 && _isFirstRowHeader)
                    {
                        int firstColumn = _isFirstColumnIndex ? 1 : 0;
                        for (int c = firstColumn; c < lineData.Length; c++)
                        {
                            _colNames.Add(lineData[c].Trim(_stringQualifier), c);
                        }
                    }

                    for (int i = 0; i < lineData.Length; i++)
                    {
                        if (i == 0 && _isFirstColumnIndex)
                        {
                            _rowIndexes.Add(lineData[i].Trim(_stringQualifier), row);
                        }

                        this[row, i] = lineData[i].Trim(_stringQualifier);
                    }

                    row++;
                    _isValid = true;
                }
            }

            if (!_isValid)
            {
                _rows = 0;
                _columns = 0;
            }
        }
    }
}