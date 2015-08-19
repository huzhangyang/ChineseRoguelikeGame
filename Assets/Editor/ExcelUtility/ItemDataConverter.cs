using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

public class ItemDataConverter : MonoBehaviour {

	const string PATH_EXCEL = "/../Documents/ExcelData/ItemData.xlsx";
	const string PATH_ASSET = "Assets/Resources/GameData/ItemData.asset";
	static ItemDataSet itemDataSet;
	static DataTable ExcelData;
	
	[MenuItem("CRGTools/ItemDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		ExcelData = excelReader.AsDataSet ().Tables["Weapon"];
		excelReader.Dispose ();
		itemDataSet = ScriptableObject.CreateInstance<ItemDataSet>();
		
		if (ExcelData != null)
		{
			LoadWeaponData();
			AssetDatabase.CreateAsset(itemDataSet, PATH_ASSET);
		}
	}
	
	static void LoadWeaponData()
	{
		for (int i = 1; i < ExcelData.Rows.Count; i++)
		{			
			WeaponData data = new WeaponData();
			itemDataSet.weaponDataSet.Add(data);

			data.id = ExcelUtility.GetIntCell(ExcelData, i, 0);
			data.name = ExcelUtility.GetCell(ExcelData, i, 1);
			data.basicATK = ExcelUtility.GetIntCell(ExcelData, i, 2);
			data.basicSPD = ExcelUtility.GetIntCell(ExcelData, i, 3);
			data.basicACC = ExcelUtility.GetIntCell(ExcelData, i, 4);
			data.basicCRT = ExcelUtility.GetIntCell(ExcelData, i, 5);
			data.skill1ID = ExcelUtility.GetIntCell(ExcelData, i, 6);
			data.skill2ID = ExcelUtility.GetIntCell(ExcelData, i, 7);
			data.skill3ID = ExcelUtility.GetIntCell(ExcelData, i, 8);

			data.type = ItemType.Weapon;
		}
	}
}
