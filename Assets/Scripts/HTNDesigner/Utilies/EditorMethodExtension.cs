
using System.Collections.Generic;
using UnityEditor;

namespace HTNDesigner.Utilies
{
    public static class EditorExtension
    {
        [MenuItem("EditorExtension/Enum/GenerateEnumTypeEnum")]
        private static void GenerateEnumTypeEnum()
        {
            string fileName = "EnumTypeEnumration";
            var enumElement = ReflectionMethodExtension.FindEnumTypes();
            List<string> enumName = new List<string>();
            foreach (var ele in enumElement)
            {
                enumName.Add(ele.Name);
            }
            var type = ReflectionMethodExtension.CreateEnum(fileName, enumName);
            FileMethodExtension.SaveEnumToFile(type, $"Assets/Scripts/HTNDesigner/Utilies/Enumrations/EnumTypeEnumration.cs");
        }
    }
}