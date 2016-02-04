using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

public class SkillDataConverter : MonoBehaviour {

	const string PATH_EXCEL = "/../Documents/ExcelData/SkillData.xlsx";
	const string PATH_ASSET = "Assets/Resources/GameData/SkillData.asset";
	
	[MenuItem("CRGTools/SkillDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		SkillDataSet skillDataSet = ScriptableObject.CreateInstance<SkillDataSet>();
		DataTable dataTable;
		ExcelLoader loader;
		
		if (excelReader.AsDataSet () != null)
		{
			dataTable = excelReader.AsDataSet().Tables["Skill"];
			loader = new ExcelLoader(dataTable);
			skillDataSet.skillDataSet = loader.CreateDataList<SkillData>();
			dataTable = excelReader.AsDataSet().Tables["Buff"];
			loader = new ExcelLoader(dataTable);
			skillDataSet.buffDataSet = loader.CreateDataList<BuffData>();
			dataTable = excelReader.AsDataSet().Tables["SkillEffect"];
			loader = new ExcelLoader(dataTable);
			skillDataSet.effectDataSet = loader.CreateDataList<SkillEffectData>();
			AssetDatabase.CreateAsset(skillDataSet, PATH_ASSET);
		}
		excelReader.Dispose ();
	}
}
