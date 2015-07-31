using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

public class EnemyDataConvertor : MonoBehaviour
{
	const string PATH_EXCEL = "/Res/ExcelData/EnemyData.xls";
	const string PATH_ASSET = "Assets/Res/GameData/EnemyData.asset";
	static EnemyDatas enemyDatas;
	static DataTable ExcelData;

	[MenuItem("CRGTools/EnemyDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		ExcelData = excelReader.AsDataSet ().Tables["Enemy"];
		excelReader.Dispose ();
		enemyDatas = ScriptableObject.CreateInstance<EnemyDatas>();
		enemyDatas.datas = new List<EnemyData>();
		
		if (ExcelData != null)
		{
			LoadData();
			AssetDatabase.CreateAsset(enemyDatas, PATH_ASSET);
		}
	}

	static void LoadData()
	{
		EnemyData data = null;
		
		for (int i = 1; i < ExcelData.Rows.Count; i++)
		{
			int enemyID = ExcelUtility.GetIntCell(ExcelData, i, 0);

			data = new EnemyData(enemyID);
			enemyDatas.datas.Add(data);

			data.name = ExcelUtility.GetCell(ExcelData, i, 1);
			data.maxHP = ExcelUtility.GetIntCell(ExcelData, i, 2);
			data.maxMP = ExcelUtility.GetIntCell(ExcelData, i, 3);
			data.power = ExcelUtility.GetIntCell(ExcelData, i, 4);
			data.skill = ExcelUtility.GetIntCell(ExcelData, i, 5);
			data.agility = ExcelUtility.GetIntCell(ExcelData, i, 6);
			data.toughness = ExcelUtility.GetIntCell(ExcelData, i, 7);
			data.luck = ExcelUtility.GetIntCell(ExcelData, i, 7);
		}
	}
	
}



