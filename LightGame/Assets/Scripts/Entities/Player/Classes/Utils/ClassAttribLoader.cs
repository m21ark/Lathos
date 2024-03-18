using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ClassAttribLoader
{
    public Dictionary<string, Dictionary<string, string>> classAttributesDict = new Dictionary<string, Dictionary<string, string>>();

    public void LoadAttributesFromCSV(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);

        string[] headers = reader.ReadLine().Split(',');

        while (!reader.EndOfStream)
        {
            string[] data = reader.ReadLine().Split(',');

            Dictionary<string, string> attributes = new Dictionary<string, string>();

            for (int i = 1; i < headers.Length; i++)
                attributes.Add(headers[i], data[i]);
            
            classAttributesDict.Add(data[0], attributes); 
        }

        reader.Close();
    }
}
