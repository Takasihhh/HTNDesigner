using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HTNDesigner.Utilies
{
    public static class FileMethodExtension
    {
        public static HashSet<string> GetAllFilesNameByfileExtension(string folderPath,string fileExtension)
        {
            HashSet<string> results = new HashSet<string>();
            FindFilesRecursively(ref results,folderPath,fileExtension);
            return results;
        }
        
        private static void FindFilesRecursively(ref HashSet<string>foundFiles, string folderPath,string fileExtension)
        {
            // 获取当前文件夹中的所有文件
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                // 检查文件扩展名是否匹配
                if (file.EndsWith(fileExtension))
                {
                    string fileName = Path.GetFileName(file);
                    // 将匹配的文件名称添加到列表中
                    foundFiles.Add(fileName);
                }
            }

            // 获取当前文件夹中的所有子文件夹
            string[] subFolders = Directory.GetDirectories(folderPath);

            foreach (string subFolder in subFolders)
            {
                // 递归调用以处理子文件夹
                FindFilesRecursively(ref foundFiles,subFolder,fileExtension);
            }
        }
        
        public static void SaveEnumToFile(Type enumType, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("using System;");
                writer.WriteLine();
                writer.WriteLine("namespace HTNDesigner.Editor.Utilies");
                writer.WriteLine("{");
                writer.WriteLine($"\tpublic enum {enumType.Name}");
                writer.WriteLine("\t{");

                foreach (var name in Enum.GetNames(enumType))
                {
                    writer.WriteLine($"\t    {name} = {(int)Enum.Parse(enumType, name)},");
                }

                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }
        }
    }
}