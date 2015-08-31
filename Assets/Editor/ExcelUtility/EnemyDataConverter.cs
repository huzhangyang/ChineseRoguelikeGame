using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

public class EnemyDataConvertor : MonoBehaviour
{
	const string PATH_EXCEL = "/../Documents/ExcelData/EnemyData.xlsx";
	const string PATH_ASSET = "Assets/Resources/GameData/EnemyData.asset";
	static EnemyDataSet enemyDataSet;
	static DataTable ExcelData;

	[MenuItem("CRGTools/EnemyDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		ExcelData = excelReader.AsDataSet ().Tables["Enemy"];
		excelReader.Dispose ();
		enemyDataSet = ScriptableObject.CreateInstance<EnemyDataSet>();
		
		if (ExcelData != null)
		{
			LoadData();
			AssetDatabase.CreateAsset(enemyDataSet, PATH_ASSET);
		}
	}

	static void LoadData()
	{		
		for (int i = 1; i < ExcelData.Rows.Count; i++)
		{
			EnemyData data = new EnemyData();
			enemyDataSet.dataSet.Add(data);

			int j = 0;

			data.id = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.name = ExcelUtility.GetCell(ExcelData, i, j++);
			data.isBoss = ExcelUtility.GetBoolCell(ExcelData, i, j++);
			data.maxHP = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.power = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.agility = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.toughness = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.insight = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.skill = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.luck = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.eloquence = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.talentID = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.natureID = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.battleType = (BattleType)ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.weaponID = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.ring1ID = ExcelUtility.GetIntCell(ExcelData, i, j++);
			data.ring2ID = ExcelUtility.GetIntCell(ExcelData, i, j++);
		}
	}
	
}



