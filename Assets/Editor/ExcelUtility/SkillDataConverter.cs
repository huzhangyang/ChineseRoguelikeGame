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
	static SkillDataSet skillDataSet;
	static DataTable ExcelData;
	
	[MenuItem("CRGTools/SkillDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		ExcelData = excelReader.AsDataSet ().Tables["Skill"];
		excelReader.Dispose ();
		skillDataSet = ScriptableObject.CreateInstance<SkillDataSet>();
		
		if (ExcelData != null)
		{
			LoadData();
			AssetDatabase.CreateAsset(skillDataSet, PATH_ASSET);
		}
	}
	
	static void LoadData()
	{		
		for (int i = 1; i < ExcelData.Rows.Count; i++)
		{
			SkillData data = new SkillData();
			skillDataSet.dataSet.Add(data);

			data.id = ExcelUtility.GetIntCell(ExcelData, i, 0);
			data.name = ExcelUtility.GetCell(ExcelData, i, 1);
			data.targetType = (TargetType)ExcelUtility.GetIntCell(ExcelData, i, 2);
			data.ATKMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 3);
			data.ACCMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 4);
			data.CRTMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 5);
			data.preSPDMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 6);
			data.postSPDMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 7);
			data.cooldownTurn = ExcelUtility.GetIntCell(ExcelData, i, 8);
			data.description = ExcelUtility.GetCell(ExcelData, i, 9);
		}
	}
}
