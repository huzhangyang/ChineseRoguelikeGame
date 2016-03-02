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
	
	[MenuItem("CRGTools/ItemDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		ItemDataSet itemDataSet = ScriptableObject.CreateInstance<ItemDataSet>();
		DataTable dataTable;
		ExcelLoader loader;
		
		if (excelReader.AsDataSet () != null)
		{
			dataTable = excelReader.AsDataSet ().Tables["Item"];
			loader = new ExcelLoader(dataTable);
			itemDataSet.itemDataSet = loader.CreateDataList<ItemData>();
			dataTable = excelReader.AsDataSet ().Tables["Weapon"];
			loader = new ExcelLoader(dataTable);
			itemDataSet.weaponDataSet = loader.CreateDataList<WeaponData>();
			dataTable = excelReader.AsDataSet ().Tables["Magic"];
			loader = new ExcelLoader(dataTable);
			itemDataSet.magicDataSet = loader.CreateDataList<MagicData>();

			foreach(WeaponData weapon in itemDataSet.weaponDataSet)
			{
				weapon.type = ItemType.Weapon;
			}
			foreach(MagicData magic in itemDataSet.magicDataSet)
			{
				magic.type = ItemType.Magic;
			}

			AssetDatabase.CreateAsset(itemDataSet, PATH_ASSET);
		}
		excelReader.Dispose ();
	}
}
