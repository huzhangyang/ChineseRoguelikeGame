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
		DataSet dataSet = excelReader.AsDataSet();
		ExcelLoader loader;

		if (excelReader != null)
		{
			EnemyDataSet enemyDataSet = ScriptableObject.CreateInstance<EnemyDataSet>();
			loader = new ExcelLoader(dataSet.Tables["Enemy"]);
			enemyDataSet.enemyDataSet = loader.CreateDataList<EnemyData>();
			loader = new ExcelLoader(dataSet.Tables["AI"]);
			enemyDataSet.aiDataSet = loader.CreateDataList<AIData>();
			AssetDatabase.CreateAsset(enemyDataSet, PATH_ASSET);
		}
		excelReader.Dispose ();		
	}
}



