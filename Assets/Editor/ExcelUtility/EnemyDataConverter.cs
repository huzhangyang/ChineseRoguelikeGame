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

	[MenuItem("CRGTools/EnemyDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

		DataTable dataTable = excelReader.AsDataSet ().Tables["Enemy"];
		excelReader.Dispose ();		
		if (dataTable != null)
		{
			EnemyDataSet enemyDataSet = ScriptableObject.CreateInstance<EnemyDataSet>();
			ExcelLoader loader = new ExcelLoader(dataTable);
			enemyDataSet.dataSet = loader.CreateDataList<EnemyData>();
			AssetDatabase.CreateAsset(enemyDataSet, PATH_ASSET);
		}
	}
}



