using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ExcelUtility
{
    public static List<string> GetFilesByExtension(string path, string extension)
    {
        List<string> result = new List<string>();
        if (!Directory.Exists(path))
        {
            return result;
        }

        string[] files = Directory.GetFiles(path);
        foreach (string filepath in files)
        {
            FileInfo file = new FileInfo(filepath);
            if (file.Extension.Equals(extension))
            {
                result.Add(file.Name);
            }
        }
        return result;
    }

	public static Dictionary<string, string> LoadKeyPairTXT(string filepath)
	{
		FileStream fs = new FileStream(filepath, FileMode.Open);
		
		StreamReader m_streamReader = new StreamReader(fs);
		
		m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

		Dictionary<string,string> dict = new Dictionary<string, string>();
		string strLine = "";

		try
		{
			do 
			{
				strLine = m_streamReader.ReadLine();
				if (strLine != null)
				{
					if (!string.IsNullOrEmpty(strLine.Trim()))
					{
						strLine = strLine.Replace("||","\n");
						strLine = strLine.Replace("|+|","\n\n");
						string[] temp = strLine.Split('\t');
						if (temp.Length >= 2)
						{
							if (!dict.ContainsKey(temp[0].Trim()))
								dict.Add(temp[0].Trim(), temp[1].Trim());
							else
								Debug.LogWarning("key " + temp[0].Trim() + " already exsit");
						}
					}				 
				}
			}while(strLine != null);
		}
		catch(System.Exception ex)
		{
			Debug.LogError(ex.ToString());
		}

		m_streamReader.Close();

		return dict;
	}


    public DataTable LoadExcel(string filePath, string sheetName)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError(filePath + " not exist!");
            return null;
        }

        string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq=" + filePath + ";";
        string query = "SELECT * FROM [" + sheetName +"$]"; 

        OdbcConnection oCon = new OdbcConnection(con);
        OdbcCommand oCmd = new OdbcCommand(query, oCon);

        DataTable sheetData = new DataTable("SheetData");

        oCon.Open();
        OdbcDataReader rData = null;
        try
        {
            rData = oCmd.ExecuteReader();
            sheetData.Load(rData);
            rData.Close();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        oCon.Close();

        return sheetData;
    }

    public static object GetEnumCell(DataTable excelData, int row, int column, Type enumType)
    {
        try
        {
            return Enum.Parse(enumType, GetCell(excelData, row, column));
        }
        catch(Exception ex)
        {
			Debug.LogError(ex.ToString());
            Debug.LogError("convert enum cell error at [" + row + "," + column + "], value : " + GetCell(excelData, row, column));
            throw null; 
        }
        
    }

    public static bool GetBoolCell(DataTable excelData, int row, int column)
    {
        try
        {
            return Convert.ToBoolean(GetCell(excelData, row, column));
        }
        catch (Exception ex)
        {
			Debug.LogError(ex.ToString());
            Debug.LogError("convert bool cell error at [" + row + "," + column + "], value : " + GetCell(excelData, row, column));
            throw null;
        }
    }

    public static float GetFloatCell(DataTable excelData, int row, int column)
    {
        try
        {
            return Convert.ToSingle(GetCell(excelData, row, column));
        }
        catch (Exception ex)
        {
			Debug.LogError(ex.ToString());
            Debug.LogError("convert float cell error at [" + row + "," + column + "], value : " + GetCell(excelData, row, column));
            throw null;
        }
    }

    public static int GetIntCell(DataTable excelData, int row, int column, bool handleEmpty = false, int emptyValue = 0)
    {
        if (handleEmpty)
        {
            string str = GetCell(excelData, row, column);
            if (str.Equals(string.Empty))
            {
                return emptyValue;
            }
            else
            {
                try
                {
                    return Convert.ToInt32(str);
                }
                catch (Exception ex)
                {
					Debug.LogError(ex.ToString());
                    Debug.LogError("convert int cell error at [" + row + "," + column + "], value : " + GetCell(excelData, row, column));
                    throw null;
                }
                
            }
        }
        else
        {
            try
            {
                return Convert.ToInt32(GetCell(excelData, row, column));
            }
            catch (Exception ex)
            {
				Debug.LogError(ex.ToString());
                Debug.LogError("convert int cell error at [" + row + "," + column + "], value : " + GetCell(excelData, row, column));
                throw null;
            }
        }
    }

    public static string GetCell(DataTable excelData, int row, int column)
    {
        string cellValue = excelData.Rows[row][excelData.Columns[column].ColumnName.ToString()].ToString().Trim();

        if (cellValue.StartsWith("#"))
        {
            return cellValue.Substring(1);
        }
        else
        {
            return cellValue;
        }
    }

}
