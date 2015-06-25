using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpreadsheetGear;
using System.Xml;
using System.IO;

/// <summary>
/// Summary description for ExportExcel
/// </summary>
public class ExportExcel
{
    public ExportExcel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Remove invalid charactors from filename
    /// </summary>
    /// <param name="invalidFilename"></param>
    /// <returns></returns>
    public string removeInvalidCharacters(string invalidFilename)
    {
        string invalidCharString = "/:|*\"?<>\\ ";
        string validFilename = "";
        for (int i = 0; i < invalidFilename.Length; i++)
        {
            if (invalidCharString.IndexOf(invalidFilename[i]) == -1)
            {
                validFilename += invalidFilename[i];
            }
        }
        return validFilename;
    }
    # region Make table worksheet
    public IWorksheet getTableWorksheet(IWorksheet tableWorksheet, string dataString, string title)
    {
        dataString = dataString.Replace("&", "*|*|"); // handling for &
        XmlDocument dataDoc = new XmlDocument();
        try
        {
            dataDoc.LoadXml(HttpUtility.HtmlDecode(dataString));
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            //File.WriteAllText(Server.MapPath("../../stock/temp.txt"), ex.Message);
        }
        // Rows Index
        XmlNodeList hearderRowElement = dataDoc.GetElementsByTagName("headerRowIndexes");
        // All Rows & Columns name
        XmlNodeList hearderRowElements = dataDoc.GetElementsByTagName("headerRow");
        // Columns Index
        XmlNodeList hearderColElement = dataDoc.GetElementsByTagName("headerColIndexes");
        
        int rowIndex = 0;        
        int totalRows = hearderRowElement[0].ChildNodes.Count;
        int totalColumns = hearderColElement[0].ChildNodes.Count;
        if (totalRows > 0)
        {
            foreach (XmlNode headerRowNode in hearderRowElement)
            {
                foreach (XmlNode rowIndexNode in headerRowNode.ChildNodes)
                {
                    if (rowIndexNode.Name == "value")
                    {
                        rowIndex++;
                        int index = Int32.Parse(rowIndexNode.InnerText.ToString());
                        XmlNode rowHeaderElement = hearderRowElements[0].ChildNodes[index];
                        tableWorksheet.Cells[totalColumns, rowIndex].Value = rowHeaderElement.InnerText.ToString();
                    }
                }
            }
        }

        rowIndex++;
        if (totalColumns > 0)
        {
            int colIndexNo = totalColumns;
            foreach (XmlNode headerColNode in hearderColElement)
            {
                foreach (XmlNode colIndexNode in headerColNode.ChildNodes)
                {
                    if (colIndexNode.Name == "value")
                    {
                        int index = Int32.Parse(colIndexNode.InnerText.ToString());
                        XmlNode colHeaderElement = hearderRowElements[0].ChildNodes[index];
                        tableWorksheet.Cells[totalColumns - colIndexNo, rowIndex].Value = colHeaderElement.InnerText.ToString();
                        colIndexNo--;
                    }
                }
            }
        }
        // Table Data
        XmlNodeList dataElements = dataDoc.GetElementsByTagName("dataRow");
        List<Dictionary<string, List<string>>> rowDictionaryList = new List<Dictionary<string, List<string>>>();
        List<Dictionary<string, List<string>>> colDictionaryList = new List<Dictionary<string, List<string>>>();

        Dictionary<string, List<string>> rowDictionary = null;
        if (totalRows > 0)
        {
            rowDictionary = new Dictionary<string, List<string>>();
            int rowindex = Int32.Parse(hearderRowElement[0].ChildNodes[0].InnerText.ToString());
            List<string> newList = getUniqueItem(dataElements, rowindex);
            rowDictionary.Add("key", newList);
            rowDictionaryList.Add(rowDictionary);
            if (hearderRowElement[0].ChildNodes.Count > 1)
            {
                int count = 1;
                foreach (XmlNode headerRowNode in hearderRowElement[0].ChildNodes)
                {
                    if (count > 1)
                    {
                        if (headerRowNode.Name == "value")
                        {
                            int index = Int32.Parse(headerRowNode.InnerText.ToString());
                            rowDictionaryList.Add(getItemDictonory(index, rowDictionaryList[rowDictionaryList.Count - 1], dataElements));
                        }
                    }
                    count++;
                }
            }
        }
        Dictionary<string, List<string>> colDictionary = null;
        List<string> sourceRowString = null;
        List<string> sourceColString = null;

        if (totalColumns > 0)
        {
            colDictionary = new Dictionary<string, List<string>>();
            int colindex = Int32.Parse(hearderColElement[0].ChildNodes[0].InnerText.ToString());
            List<string> newColList = getUniqueItem(dataElements, colindex);
            colDictionary.Add("key", newColList);
            colDictionaryList.Add(colDictionary);
            if (hearderColElement[0].ChildNodes.Count > 1)
            {
                int count = 1;
                foreach (XmlNode headerColNode in hearderColElement[0].ChildNodes)
                {
                    if (count > 1)
                    {
                        if (headerColNode.Name == "value")
                        {
                            int index = Int32.Parse(headerColNode.InnerText.ToString());
                            colDictionaryList.Add(getItemDictonory(index, colDictionaryList[colDictionaryList.Count - 1], dataElements));
                        }
                    }
                    count++;
                }
            }
        }
        #region Indexes colllection
        List<int> rowindexCollection = new List<int>();
        List<int> colindexCollection = new List<int>();
        int dataIndex = Int32.Parse(dataDoc.GetElementsByTagName("dataColumnIndex")[0].InnerText.ToString());
        #endregion End Indexes collection
        #region Making Rows including mergecells
        if (totalRows > 0)
        {
            if (rowDictionaryList.Count > 1)
            {
                tableWorksheet = mergeRowCells(tableWorksheet, rowDictionaryList, totalColumns);
            }
            tableWorksheet = fillRowData(tableWorksheet, rowDictionaryList, totalColumns);
            sourceRowString = getSourceString(rowDictionaryList[rowDictionaryList.Count - 1]);
            foreach (XmlNode hearderIndexNode in hearderRowElement)
            {
                foreach (XmlNode indexNode in hearderIndexNode.ChildNodes)
                {
                    int index = Int32.Parse(indexNode.InnerText.ToString());
                    rowindexCollection.Add(index);
                }
            }
        }
        #endregion
        #region Making Columns including mergecells
        if (totalColumns > 0)
        {
            if (colDictionaryList.Count > 1)
            {
                tableWorksheet = mergeColumnCells(tableWorksheet, colDictionaryList, rowDictionaryList.Count + 2, totalColumns);
            }
            tableWorksheet = fillColumnData(tableWorksheet, colDictionaryList, rowDictionaryList.Count + 2, totalColumns);
            sourceColString = getSourceString(colDictionaryList[colDictionaryList.Count - 1]);
            foreach (XmlNode ColIndexNode in hearderColElement)
            {
                foreach (XmlNode indexNode in ColIndexNode.ChildNodes)
                {
                    int index = Int32.Parse(indexNode.InnerText.ToString());
                    colindexCollection.Add(index);
                }
            }
        }
        #endregion
        if (totalRows == 0)
        {
            tableWorksheet = fillDataValuesNoRow(sourceColString, colindexCollection, dataIndex, dataElements, tableWorksheet, totalColumns);
            tableWorksheet.Cells[totalColumns, 1].Value = "Data Value";
            tableWorksheet.Cells[totalColumns, 1].Columns.AutoFit();
            tableWorksheet.Cells[totalColumns, 1].Rows.AutoFit();
        }
        else if (totalColumns == 0)
        {
            tableWorksheet = fillDataValuesNoColumn(sourceRowString, rowindexCollection, dataIndex, dataElements, tableWorksheet, totalColumns);
            tableWorksheet.Cells[totalColumns, rowindexCollection.Count + 1].Value = "Data Value";
            tableWorksheet.Cells[totalColumns, rowindexCollection.Count + 1].Columns.AutoFit();
            tableWorksheet.Cells[totalColumns, rowindexCollection.Count + 1].Rows.AutoFit();
        }
        else
        {
            tableWorksheet = fillDataValues(sourceRowString, sourceColString, rowindexCollection, colindexCollection, dataIndex, dataElements, tableWorksheet, totalColumns);
            tableWorksheet.Cells[totalColumns - colindexCollection.Count, 1, totalColumns - 1, rowindexCollection.Count].Merge();
            tableWorksheet.Cells[totalColumns - colindexCollection.Count, 1, totalColumns - 1, rowindexCollection.Count].Value = "Data Value";
            tableWorksheet.Cells[totalColumns - colindexCollection.Count, 1, totalColumns - 1, rowindexCollection.Count].VerticalAlignment = VAlign.Center;
            tableWorksheet.Cells[totalColumns - colindexCollection.Count, 1, totalColumns - 1, rowindexCollection.Count].HorizontalAlignment = HAlign.Center;
        }
        tableWorksheet.UsedRange.Columns.AutoFit();
        tableWorksheet.UsedRange.Rows.AutoFit();
        tableWorksheet.Cells.Range["A:A"].UnMerge();
        tableWorksheet.Cells.Range["A:A"].Delete();
        //tableWorksheet.Cells[0, 0].Insert(InsertShiftDirection.Down);
        return tableWorksheet;
    }
    #endregion
    private Dictionary<string, List<string>> getItemDictonory(int index, Dictionary<string, List<string>> prevDictionary, XmlNodeList records)
    {
        Dictionary<string, List<string>> newDictionary = new Dictionary<string, List<string>>();
        foreach (string key in prevDictionary.Keys)
        {
            if (key == "key")
            {
                foreach (string item in prevDictionary[key])
                {
                    List<string> newList = new List<string>();
                    foreach (XmlNode record in records)
                    {
                        if (isMatched(item, record))
                        {
                            string itemString = record.ChildNodes[index].InnerText.ToString();                            
                            if (!newList.Contains(itemString))
                                newList.Add(itemString);
                        }
                    }
                    newDictionary.Add(item, newList);
                }
            }
            else
            {
                foreach (string item in prevDictionary[key])
                {
                    List<string> newList = new List<string>();
                    foreach (XmlNode record in records)
                    {
                        if (isMatched(key + "[####]" + item, record))
                        {
                            string itemString = record.ChildNodes[index].InnerText.ToString();                           
                            if (!newList.Contains(itemString))
                                newList.Add(itemString);
                        }
                     }
                    newDictionary.Add(key + "[####]" + item, newList);
                }
            }
        }
        return newDictionary;
    }
    #region Merge row cells
    private IWorksheet mergeRowCells(IWorksheet tableWorksheet, List<Dictionary<string, List<string>>> dictonaryList, int totalColumns)
    {
        Dictionary<string, List<string>> targetDictonary = dictonaryList[dictonaryList.Count - 1];
        for (int i = 1; i < dictonaryList.Count; i++)
        {
            Dictionary<string, List<string>> sourceDictonary = dictonaryList[i];
            int rowNo = totalColumns + 1;
            int mergeCells = totalColumns + 1;
            foreach (string sourcekey in sourceDictonary.Keys)
            {
                int rowCount = 0;                
                foreach (string targetkey in targetDictonary.Keys)
                {
                    if (targetkey != sourcekey)
                    {
                        int index = targetkey.IndexOf(sourcekey);
                        int underscoreIndex = index + sourcekey.Length;
                        if (index == 0)
                        {
                            if (targetkey.Substring(underscoreIndex, 6) == "[####]")
                            {
                                rowCount += targetDictonary[targetkey].Count;
                            }
                            //if (targetkey[underscoreIndex].ToString() == "[####]")
                            //rowCount += targetDictonary[targetkey].Count;
                        }
                    }
                    else
                    {
                        rowCount += targetDictonary[targetkey].Count;
                    }
                }
                rowNo += rowCount;
                tableWorksheet.Cells[mergeCells, i, mergeCells + rowCount - 1, i].Merge();
                tableWorksheet.Cells[mergeCells, i, mergeCells + rowCount - 1, i].VerticalAlignment = VAlign.Top;
                mergeCells = rowNo;
            }
        }
        return tableWorksheet;
    }
    #endregion
    #region Merge col cells
    private IWorksheet mergeColumnCells(IWorksheet tableWorksheet, List<Dictionary<string, List<string>>> dictonaryList, int colNo, int totalColumns)
    {
        int temp = colNo;
        Dictionary<string, List<string>> targetDictonary = dictonaryList[dictonaryList.Count - 1];
        int rowNo = totalColumns - dictonaryList.Count;
        //colNo = temp;
        for (int i = 1; i < dictonaryList.Count; i++)
        {
            colNo = temp;
            int mergeCells = temp;
            Dictionary<string, List<string>> sourceDictonary = dictonaryList[i];
            foreach (string sourcekey in sourceDictonary.Keys)
            {
                int colCount = 0;
                foreach (string targetkey in targetDictonary.Keys)
                {
                    if (targetkey != sourcekey)
                    {
                        int index = targetkey.IndexOf(sourcekey);
                        int underscoreIndex = index + sourcekey.Length;
                        if (index == 0)
                        {
                            if (targetkey.Substring(underscoreIndex, 6) == "[####]")
                            {
                                colCount += targetDictonary[targetkey].Count;
                            }
                            //if (targetkey[underscoreIndex].ToString() == "[####]")
                            //    colCount += targetDictonary[targetkey].Count;
                        }
                    }
                    else
                    {
                        colCount += targetDictonary[targetkey].Count;
                    }                    
                }
                colNo += colCount;
                tableWorksheet.Cells[rowNo, mergeCells, rowNo, mergeCells + colCount - 1].Merge();
                tableWorksheet.Cells[rowNo, mergeCells, rowNo, mergeCells + colCount - 1].HorizontalAlignment = HAlign.Center;
                mergeCells = colNo;
            }
            rowNo++;
        }
        return tableWorksheet;
    }
    #endregion
    private IWorksheet fillRowData(IWorksheet tableWorksheet, List<Dictionary<string, List<string>>> dictonaryList, int totalColumns)
    {
        if (dictonaryList.Count == 1)
        {
            int rowNo = totalColumns + 1;
            Dictionary<string, List<string>> sourceDictonary = dictonaryList[0];
            foreach (string key in sourceDictonary.Keys)
            {
                foreach (string values in sourceDictonary[key])
                {
                    tableWorksheet.Cells[rowNo, 1].Value = ConvertToOriginalString(values);
                    rowNo++;
                }
            }
        }
        else
        {
            for (int i = 0; i < dictonaryList.Count; i++)
            {
                int rowNo = totalColumns + 1;
                Dictionary<string, List<string>> sourceDictonary = dictonaryList[i];
                foreach (string key in sourceDictonary.Keys)
                {
                    foreach (string values in sourceDictonary[key])
                    {
                        //tableWorksheet.Cells[rowNo, i + 1].Value = values;
                        tableWorksheet.Cells[rowNo, i + 1].Range.Value = ConvertToOriginalString(values);
                        if (tableWorksheet.Cells[rowNo, i + 1].MergeCells)
                        {
                            rowNo += tableWorksheet.Cells[rowNo, i + 1].MergeArea.Count;
                        }
                        else
                        {
                            rowNo++;
                        }
                    }
                }
            }
        }
        return tableWorksheet;
    }

    private IWorksheet fillColumnData(IWorksheet tableWorksheet, List<Dictionary<string, List<string>>> dictonaryList, int colNo, int totalColumns)
    {
        if (dictonaryList.Count == 1)
        {
            //int colNo = tableWorksheet.UsedRange.ColumnCount + 1;            
            Dictionary<string, List<string>> sourceDictonary = dictonaryList[0];
            foreach (string key in sourceDictonary.Keys)
            {
                foreach (string values in sourceDictonary[key])
                {
                    tableWorksheet.Cells[totalColumns - 1, colNo].Value = ConvertToOriginalString(values);
                    colNo++;
                }
            }
        }
        else
        {
            int temp = colNo;
            int rowNo = totalColumns - dictonaryList.Count;
            for (int i = 0; i < dictonaryList.Count; i++)
            {
                colNo = temp;
                //int colNo = tableWorksheet.UsedRange.ColumnCount + 1;                
                Dictionary<string, List<string>> sourceDictonary = dictonaryList[i];
                foreach (string key in sourceDictonary.Keys)
                {
                    foreach (string values in sourceDictonary[key])
                    {
                        //tableWorksheet.Cells[rowNo, i + 1].Value = values;
                        tableWorksheet.Cells[rowNo, colNo].Range.Value = ConvertToOriginalString(values);
                        if (tableWorksheet.Cells[rowNo, colNo].MergeCells)
                        {
                            colNo += tableWorksheet.Cells[rowNo, colNo].MergeArea.Count;
                        }
                        else
                        {
                            colNo++;
                        }
                    }
                }
                rowNo++;
            }
        }
        return tableWorksheet;
    }

    private bool isMatched(string matchingStr, XmlNode record)
    {
        bool matchFound = false;
        if (matchingStr!=null)
        {
            string[] items;
            if (matchingStr.IndexOf("[####]") > -1)
            {
                items = matchingStr.Split(new string[] { "[####]" }, StringSplitOptions.None);
                foreach (string item in items)
                {
                    matchFound = false;
                    foreach (XmlNode recordItem in record.ChildNodes)
                    {
                        if (item == recordItem.InnerText.ToString())
                        {
                            matchFound = true;
                        }
                    }
                    if (!matchFound)
                    {
                        break;
                    }
                }
            }
            else
            {
                foreach (XmlNode recordItem in record.ChildNodes)
                {
                    if (matchingStr == recordItem.InnerText.ToString())
                    {
                        matchFound = true;
                    }
                }
            }
        }
        return matchFound;
    }
    private List<string> getUniqueItem(XmlNodeList records, int index)
    {
        List<string> itemList = new List<string>();
        foreach (XmlNode record in records)
        {
            string item = record.ChildNodes[index].InnerText.ToString();            
            if (!itemList.Contains(item))
                itemList.Add(item);
        }
        return itemList;
    }

    private IWorksheet fillDataValues(List<string> rows, List<string> cols, List<int> rowindexCollection, List<int> colindexCollection, int dataColIndex, XmlNodeList dataRowNodeList, IWorksheet tableWorksheet, int totalColumns)
    {
        int row = 0;
        int col = 0;
        int count = 0;
        foreach (XmlNode record in dataRowNodeList)
        {
            for (row = 0; row < rows.Count; row++)
            {
                string dataValue = "";
                bool rowMatched = true;
                string[] rowitems = rows[row].Split(new string[] { "[####]" }, StringSplitOptions.None);
                count = 0;
                foreach (int index in rowindexCollection)
                {
                    String rowItem = record.ChildNodes[index].InnerText.ToString();
                    if (rowitems[0] == "key")
                    {
                        count++;
                    }
                    if (rowItem != rowitems[count])
                    {
                        rowMatched = false;
                        break;
                    }
                    count++;
                }
                if (rowMatched)
                {
                    for (col = 0; col < cols.Count; col++)
                    {
                        bool isRecordMatch = true;
                        string[] colitems = cols[col].Split(new string[] { "[####]" }, StringSplitOptions.None);
                        count = 0;
                        foreach (int index in colindexCollection)
                        {
                            String rowItem = record.ChildNodes[index].InnerText.ToString();
                            if (colitems[0] == "key")
                            {
                                count++;
                            }
                            if (rowItem != colitems[count])
                            {
                                isRecordMatch = false;
                                break;
                            }
                            count++;
                        }
                        if (isRecordMatch)
                        {
                            dataValue = record.ChildNodes[dataColIndex].InnerText.ToString();
                            dataValue = extractDataVale(dataValue);
                            tableWorksheet.Cells[row + totalColumns + 1, col + rowindexCollection.Count + 2].Value = dataValue;
                        }
                    }
                }
            }
        }
        return tableWorksheet;
    }

    private IWorksheet fillDataValuesNoColumn(List<string> rows, List<int> rowindexCollection, int dataColIndex, XmlNodeList dataRowNodeList, IWorksheet tableWorksheet, int totalColumns)
    {
        int row = 0;
        int col = 0;
        int count = 0;
        foreach (XmlNode record in dataRowNodeList)
        {
            for (row = 0; row < rows.Count; row++)
            {
                string dataValue = "";
                bool rowMatched = true;
                string[] rowitems = rows[row].Split(new string[] { "[####]" }, StringSplitOptions.None);
                count = 0;
                foreach (int index in rowindexCollection)
                {
                    String rowItem = record.ChildNodes[index].InnerText.ToString();
                    if (rowitems[0] == "key")
                    {
                        count++;
                    }
                    if (rowItem != rowitems[count])
                    {
                        rowMatched = false;
                        break;
                    }
                    count++;
                }
                if (rowMatched)
                {
                    dataValue = record.ChildNodes[dataColIndex].InnerText.ToString();
                    dataValue = extractDataVale(dataValue);
                    tableWorksheet.Cells[row + totalColumns + 1, col + rowindexCollection.Count + 1].Value = dataValue;
                }
            }
        }
        return tableWorksheet;
    }

    private IWorksheet fillDataValuesNoRow(List<string> cols, List<int> colindexCollection, int dataColIndex, XmlNodeList dataRowNodeList, IWorksheet tableWorksheet, int totalColumns)
    {
        int row = 0;
        int col = 0;
        int count = 0;
        foreach (XmlNode record in dataRowNodeList)
        {
            for (col = 0; col < cols.Count; col++)
            {
                bool isRecordMatch = true;
                string[] colitems = cols[col].Split(new string[] { "[####]" }, StringSplitOptions.None);
                count = 0;
                foreach (int index in colindexCollection)
                {
                    String rowItem = record.ChildNodes[index].InnerText.ToString();
                    if (colitems[0] == "key")
                    {
                        count++;
                    }
                    if (rowItem != colitems[count])
                    {
                        isRecordMatch = false;
                        break;
                    }
                    count++;
                }
                if (isRecordMatch)
                {
                    string dataValue = record.ChildNodes[dataColIndex].InnerText.ToString();
                    dataValue = extractDataVale(dataValue);
                    tableWorksheet.Cells[row + totalColumns, col + 2].Value = dataValue;
                }
            }
        }
        return tableWorksheet;
    }

    private string extractDataVale(string valueString)
    {
        string dataValue = valueString;
        if (valueString.IndexOf("<a") > -1)
        {
            int dataValuePos = valueString.IndexOf('>') + 1;
            string tempString = valueString.Substring(dataValuePos);
            int anchorEngTagPos = tempString.IndexOf("</a>");
            string dataValueString = tempString.Substring(0, anchorEngTagPos);
            dataValue = dataValueString;
        }
        return dataValue;
    }
    private List<string> getSourceString(Dictionary<string, List<string>> rowDictionary)
    {
        List<string> SourceStringCollection = new List<string>();
        foreach (string key in rowDictionary.Keys)
        {
            foreach (string value in rowDictionary[key])
            {
                SourceStringCollection.Add(key + "[####]" + value);
            }
        }
        return SourceStringCollection;
    }

    private string ConvertToOriginalString(string modifiedString)
    {
        string originalString = string.Empty;
        originalString = modifiedString.Replace("*|*|", "&");
        return originalString;
    }
}