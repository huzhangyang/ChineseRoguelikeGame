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
		skillDataSet = ScriptableObject.CreateInstance<SkillDataSet>();
		
		if (excelReader.AsDataSet () != null)
		{
			ExcelData = excelReader.AsDataSet ().Tables["Skill"];
			LoadSkillData();
			ExcelData = excelReader.AsDataSet ().Tables["Buff"];
			LoadBuffData();
			AssetDatabase.CreateAsset(skillDataSet, PATH_ASSET);
		}
		excelReader.Dispose ();
	}
	
	static void LoadSkillData()
	{		
		for (int i = 1; i < ExcelData.Rows.Count; i++)
		{
			SkillData data = new SkillData();
			skillDataSet.skillDataSet.Add(data);

			data.id = ExcelUtility.GetIntCell(ExcelData, i, 0);
			data.name = ExcelUtility.GetCell(ExcelData, i, 1);
			data.description = ExcelUtility.GetCell(ExcelData, i, 2);
			data.skillType = (SkillType)System.Enum.Parse(typeof(SkillType),ExcelUtility.GetCell(ExcelData, i, 3));
			data.targetType = (TargetType)System.Enum.Parse(typeof(TargetType),ExcelUtility.GetCell(ExcelData, i, 4));
			data.ATKMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 5);
			data.ACCMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 6);
			data.CRTMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 7);
			data.preSPDMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 8);
			data.postSPDMultiplier = ExcelUtility.GetFloatCell(ExcelData, i, 9);
			data.buffID = ExcelUtility.GetIntCell(ExcelData, i, 10);

		}
	}

	static void LoadBuffData()
	{		
		for (int i = 1; i < ExcelData.Rows.Count; i++)
		{
			BuffData data = new BuffData();
			skillDataSet.buffDataSet.Add(data);

			data.id = ExcelUtility.GetIntCell(ExcelData, i, 0);
			data.name = ExcelUtility.GetCell(ExcelData, i, 1);
			data.description = ExcelUtility.GetCell(ExcelData, i, 2);
			data.percentage = ExcelUtility.GetIntCell(ExcelData, i, 3);
			data.lastTurns = ExcelUtility.GetIntCell(ExcelData, i, 4);
			data.trigger = (BuffTrigger)System.Enum.Parse(typeof(BuffTrigger),ExcelUtility.GetCell(ExcelData, i, 5));
			data.triggerOps = ExcelUtility.GetCell(ExcelData, i, 6);
			data.addOps = ExcelUtility.GetCell(ExcelData, i, 7);
			data.removeOps = ExcelUtility.GetCell(ExcelData, i, 8);

		}
	}
}
