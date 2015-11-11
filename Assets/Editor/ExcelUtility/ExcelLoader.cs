using UnityEngine;
using UnityEditor;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Excel加载解析类
/// </summary>
public class ExcelLoader
{
	// 下面最好是有个结构体处理 而不是多dictionary
	private Dictionary<string, int> dictKey2ColIndex = new Dictionary<string, int>();	        // key与列序号的映射表
	private Dictionary<object, int> dictID2RowIndex = new Dictionary<object, int>();			// ID与行序号的映射表
	private Dictionary<int, string> dictDefaultVal2ColIndex = new Dictionary<int, string>();	// 默认值与列序号的映射表
	private Dictionary<int, string> dictKeyTypeIndex = new Dictionary<int, string>();           // key类型
	private Dictionary<int, string> dictKeyDescriptionIndex = new Dictionary<int, string>();    // key描述

	private DataTable dataTable;		// 从Excel中读取的数据
	
	//----------------------------------------------------------
	//	解析与读取Excel数据
	//----------------------------------------------------------

	public ExcelLoader(DataTable inputData)
	{
		dataTable = inputData;

		ParseKeyAndIdInfo();
	}

	public List<T> CreateDataList<T>() where T : new()
	{
		var list = new List<T>();
		
		T data;
		FieldInfo field;
		System.Type type = typeof(T);

		var rowItor = GetRowEnumerator();
		while (rowItor.MoveNext())
		{
			data = new T();
			var columeItor = GetColumeEnumerator();
			while (columeItor.MoveNext())
			{
				field = type.GetField(columeItor.Current.Key, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
				if (field != null)
					field.SetValue(data, GetData(rowItor.Current.Key, columeItor.Current.Key));//rowItor = ID, columeItor = key
			}
			
			list.Add(data);
		}
		
		return list;
	}

	private Dictionary<object, int>.Enumerator GetRowEnumerator()
	{
		return dictID2RowIndex.GetEnumerator();
	}

	private Dictionary<string, int>.Enumerator GetColumeEnumerator()
	{
		return dictKey2ColIndex.GetEnumerator();
	}

	// 获取数据（根据ID和key）
	private object GetData(object id, string key)
	{
		// 参数检查
		if (!dictID2RowIndex.ContainsKey(id))
		{
			Debug.LogError("错误：不存在的id键值 " + id);
			return null;
		}
		if (!dictKey2ColIndex.ContainsKey(key.ToLower()))
		{
			Debug.LogError("错误：不存在的key键值 " + key.ToLower());
			return null;
		}

		// 获取指定行列的数据
		int rowIndex = dictID2RowIndex[id];
		int colIndex = dictKey2ColIndex[key.ToLower()];
		object ret = GetInternalData(rowIndex, colIndex);

		return ret;

	}

	// 获取指定行列的数据
	private object GetInternalData(int rowIndex, int colIndex)
	{
		// 获取数据类型信息
		int DATA_TYPE_ROW_INDEX = 2;
		string dataType = GetCell(dataTable, DATA_TYPE_ROW_INDEX, colIndex);

		// 返回对象
		object ret = null;

		// 若是默认值
		string content = GetCell(dataTable, rowIndex, colIndex) as string;
		if (string.IsNullOrEmpty(content.Trim()))
		{
			switch (dataType)
			{
				case "INT":
					ret = int.Parse(dictDefaultVal2ColIndex[colIndex]);
					break;
				case "FLOAT":
					ret = float.Parse(dictDefaultVal2ColIndex[colIndex]);
					break;
				case "BOOL":
					ret = bool.Parse(dictDefaultVal2ColIndex[colIndex]);
					break;
				case "STRING":
					ret = dictDefaultVal2ColIndex[colIndex];
					break;
				case "LIST":
					ret = ParseListData(dictDefaultVal2ColIndex[colIndex]);
					break;
				default:
					Debug.LogWarning("警告：未知的数据类型 " + dataType);
					Type type = Assembly.GetExecutingAssembly().GetType(dataType);
					ret = Enum.Parse(type, dictDefaultVal2ColIndex[colIndex]); 
					break;
			}

			return ret;
		}

		// 若是非默认值
		switch (dataType)
		{
			case "INT":
				ret = GetIntCell(dataTable, rowIndex, colIndex);
				break;
			case "FLOAT":
				ret = GetFloatCell(dataTable, rowIndex, colIndex);
				break;
			case "BOOL":
				ret = GetBoolCell(dataTable, rowIndex, colIndex);
				break;
			case "STRING":
				ret = GetCell(dataTable, rowIndex, colIndex);
				break;
			case "LIST":
				string data = GetCell(dataTable, rowIndex, colIndex);
				ret = ParseListData(data);
				break;
			default:
				Debug.LogWarning("警告：未知的数据类型 " + dataType);
				Type type = Assembly.GetExecutingAssembly().GetType(dataType);
				ret = GetEnumCell(dataTable, rowIndex, colIndex, type);
				break;
		}

		return ret;
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

	// 解析List数据
	private List<int> ParseListData(string strData)
	{
		List<int> retList = new List<int>();
		string[] numList = strData.Split(',');
		foreach (string numStr in numList)
		{
			if (string.IsNullOrEmpty(numStr.Trim()))
				continue;

			int num = int.Parse(numStr);
			retList.Add(num);
		}

		return retList;
	}

	#region Parse
	/*---------- 解析Excel的key和ID信息----------*/

	private void ParseKeyAndIdInfo()
	{
		// 解析列key值
		ParseKeyInfo();

		// 解析行ID值
		ParseIdInfo();

		// 解析类型和描述
		ParseHeaderInfo();
	}

	// 解析列key值
	private void ParseKeyInfo()
	{
		for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
		{
			int KEY_ROW_INDEX = 1;

			// 若是空列，跳过
			string key = GetCell(dataTable, KEY_ROW_INDEX, colIndex).Trim().ToLower();	// 去除空格，改为全小写字母
			if (string.IsNullOrEmpty(key))
			{
				continue;
			}

			// 保存key的映射表
			if (!dictKey2ColIndex.ContainsKey(key))
			{
				dictKey2ColIndex[key] = colIndex;
			}
			else
			{
				Debug.LogError("错误：Excel表中有重复的key值，是" + key);
				return;
			}

			// 保存该列的默认值
			int DEFAULT_VAL_ROW_INDEX = 3;
			string defaultVal = GetCell(dataTable, DEFAULT_VAL_ROW_INDEX, colIndex).Trim();
			dictDefaultVal2ColIndex[colIndex] = defaultVal;
		}
	}

	// 解析行ID值
	private void ParseIdInfo()
	{
		int DATA_START_ROW_INDEX = 4;
		//Debug.Log("###行数：" + configData.Rows.Count);
		for (int rowIndex = DATA_START_ROW_INDEX; rowIndex < dataTable.Rows.Count; rowIndex++)
		{
			int ID_COL_INDEX = 0;

			// 若是空行，跳过
			string content = GetCell(dataTable, rowIndex, ID_COL_INDEX).Trim();
			if (string.IsNullOrEmpty(content))
			{
				continue;
			}

			// 不是空行
			//int id = ExcelUtility.GetIntCell(configData, rowIndex, ID_COL_INDEX);
			object id = GetInternalData(rowIndex, ID_COL_INDEX);

			if (!dictID2RowIndex.ContainsKey(id))
			{
				dictID2RowIndex[id] = rowIndex;
			}
			else
			{
				Debug.LogError("错误：Excel表中有重复的id值，是" + id);
				return;
			}
		}
	}

	// other header informations
	private void ParseHeaderInfo()
	{
		// description
		string data;
		const int DESC_START_ROW_INDEX = 0;
		const int TYPE_START_ROW_INDEX = 2;
		for (int colIndex = 0; colIndex < dataTable.Columns.Count; ++colIndex)
		{
			data = GetCell(dataTable, DESC_START_ROW_INDEX, colIndex).Trim().ToLower();
			if (string.IsNullOrEmpty(data))
			{
				continue;
			}

			if (!dictKeyDescriptionIndex.ContainsKey(colIndex))
			{
				dictKeyDescriptionIndex[colIndex] = data;
			}
			else
			{
				Debug.LogError("错误：Excel表中有重复的描述值，是" + data);
				return;
			}

			data = GetCell(dataTable, TYPE_START_ROW_INDEX, colIndex).Trim().ToLower();
			if (string.IsNullOrEmpty(data))
			{
				continue;
			}

			if (!dictKeyTypeIndex.ContainsKey(colIndex))
			{
				dictKeyTypeIndex[colIndex] = data;
			}
			else
			{
				Debug.LogError("错误：Excel表中有重复的类型值，是在" + colIndex);
				return;
			}
		}
	}
	#endregion
}
