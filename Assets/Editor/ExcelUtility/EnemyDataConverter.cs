using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

public class EnemyDataConvertor : MonoBehaviour
{
	const string PATH_EXCEL = "/../Documents/ExcelData/EnemyData.xls";
	const string PATH_ASSET = "Assets/Resources/GameData/EnemyData.asset";
	static EnemyDataSet enemyDatas;
	static DataTable ExcelData;

	[MenuItem("CRGTools/EnemyDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		ExcelData = excelReader.AsDataSet ().Tables["Enemy"];
		excelReader.Dispose ();
		enemyDatas = ScriptableObject.CreateInstance<EnemyDataSet>();
		enemyDatas.dataSet = new List<EnemyData>();
		
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
			enemyDatas.dataSet.Add(data);

			data.name = ExcelUtility.GetCell(ExcelData, i, 1);
			data.maxHP = ExcelUtility.GetIntCell(ExcelData, i, 2);
			data.power = ExcelUtility.GetIntCell(ExcelData, i, 3);
			data.agility = ExcelUtility.GetIntCell(ExcelData, i, 4);
			data.toughness = ExcelUtility.GetIntCell(ExcelData, i, 5);
			data.insight = ExcelUtility.GetIntCell(ExcelData, i, 6);
			data.skill = ExcelUtility.GetIntCell(ExcelData, i, 7);
			data.luck = ExcelUtility.GetIntCell(ExcelData, i, 8);
			data.eloquence = ExcelUtility.GetIntCell(ExcelData, i, 9);
			data.talentID = ExcelUtility.GetIntCell(ExcelData, i, 10);
			data.natureID = ExcelUtility.GetIntCell(ExcelData, i, 11);
			data.battleType = (BattleType)ExcelUtility.GetIntCell(ExcelData, i, 12);
			data.weaponID = ExcelUtility.GetIntCell(ExcelData, i, 13);
			data.ring1ID = ExcelUtility.GetIntCell(ExcelData, i, 14);
			data.ring2ID = ExcelUtility.GetIntCell(ExcelData, i, 15);

			data.currentHP = data.maxHP;
		}
	}
	
}



