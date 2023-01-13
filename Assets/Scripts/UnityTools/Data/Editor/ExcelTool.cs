using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class ExcelTool
{
    public static string EXCEL_PATH = Application.dataPath + "/Excel/";
    public static string CLASS_DATA_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";
    public static string CONTAINER_DATA_PATH = Application.dataPath + "/Scripts/ExcelData/DataContainer/";

    public static int START_LOAD = 4;

    [MenuItem("GameTool/生成Excel表中的数据")]
    private static void GenerateExcelInfo()
    {
        DirectoryInfo directoryInfo = Directory.CreateDirectory(EXCEL_PATH);
        FileInfo[] files = directoryInfo.GetFiles();

        DataTableCollection tableCollection;
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" && files[i].Extension != ".xls")
                continue;

            using (FileStream fileStream = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                tableCollection = excelDataReader.AsDataSet().Tables;
                fileStream.Close();
            }

            foreach (DataTable table in tableCollection)
            {
                GenerateExcelDataClass(table);
                GenerateExcelDataContainer(table);
                GenerateExcelDataBinary(table);
            }
        }
    }

    private static void ClearExcelInfo()
    {

    }

    private static void GenerateExcelDataClass(DataTable table)
    {
        DataRow rowName = GetVariableNameRow(table);
        DataRow royType = GetVariableTypeRow(table);

        if (!Directory.Exists(CLASS_DATA_PATH))
            Directory.CreateDirectory(CLASS_DATA_PATH);

        string text = "public class " + table.TableName + "\n{\n";
        for (int i = 0; i < table.Columns.Count; i++)
        {
            text += "    public " + royType[i].ToString() + " " + rowName[i].ToString() + ";\n";
        }
        text += "}";

        File.WriteAllText(CLASS_DATA_PATH + table.TableName + ".cs", text);

        AssetDatabase.Refresh();
    }

    private static void GenerateExcelDataContainer(DataTable table)
    {
        int keyIndex = GetKeyIndex(table);
        DataRow rowType = GetVariableTypeRow(table);

        if (!Directory.Exists(CONTAINER_DATA_PATH))
            Directory.CreateDirectory(CONTAINER_DATA_PATH);

        string text = "using System.Collections.Generic;\n";
        text += "public class " + table.TableName + "Container" + "\n{\n";
        text += "    ";
        text += "public Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">";
        text += "dataDic = new " + "Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">();\n";
        text += "}";

        File.WriteAllText(CONTAINER_DATA_PATH + table.TableName + "Container.cs", text);

        AssetDatabase.Refresh();
    }

    private static void GenerateExcelDataBinary(DataTable table)
    {
        if (!Directory.Exists(BinaryDataManager.BINARY_DATA_PATH))
            Directory.CreateDirectory(BinaryDataManager.BINARY_DATA_PATH);

        using (FileStream fileStream = new FileStream(BinaryDataManager.BINARY_DATA_PATH + table.TableName + ".table", FileMode.OpenOrCreate, FileAccess.Write))
        {
            fileStream.Write(BitConverter.GetBytes(table.Rows.Count - 4), 0, 4);

            string keyName = GetVariableNameRow(table)[GetKeyIndex(table)].ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(keyName);

            fileStream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            fileStream.Write(bytes, 0, bytes.Length);

            DataRow row;
            DataRow rowType = GetVariableTypeRow(table);
            for (int i = START_LOAD; i < table.Rows.Count; i++)
            {
                row = table.Rows[i];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    switch (rowType[j].ToString())
                    {
                        case "int":
                            fileStream.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "float":
                            fileStream.Write(BitConverter.GetBytes(float.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "bool":
                            fileStream.Write(BitConverter.GetBytes(bool.Parse(row[j].ToString())), 0, 1);
                            break;
                        case "string":
                            bytes = Encoding.UTF8.GetBytes(row[j].ToString());
                            fileStream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                            fileStream.Write(bytes, 0, bytes.Length);
                            break;
                    }
                }
            }

            fileStream.Close();
        }

        AssetDatabase.Refresh();
    }

    private static DataRow GetVariableNameRow(DataTable table) => table.Rows[2];
    private static DataRow GetVariableTypeRow(DataTable table) => table.Rows[3];
    private static int GetKeyIndex(DataTable table)
    {
        DataRow row = table.Rows[0];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key")
                return i;
        }

        return 0;
    }


}
