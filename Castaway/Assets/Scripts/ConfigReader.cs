﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public static class ConfigReader
{
    private static string applicationPath = Application.dataPath.ToString();

    private static T StandardDeserialization<T>(string xmlPath, string rootElementName)
    {
        string absolutePath = applicationPath + "/_Data/" + xmlPath;
        XmlSerializer mySerializer = new XmlSerializer(typeof(T), new XmlRootAttribute { ElementName = rootElementName });
        if (File.Exists(absolutePath))
        {
            using (FileStream myFileStream = new FileStream(absolutePath, FileMode.Open))
            {
                T result;
                result = (T)mySerializer.Deserialize(myFileStream);
                return result;
            }
        }
        else
            throw new FileNotFoundException("Could not find personality file: " + absolutePath + "!");
    }

    public static List<PersonalEmotion> ReadEmotionData (string personalXMLPath)
    {
        return StandardDeserialization<List<PersonalEmotion>>(personalXMLPath, "Emotions");
    }
    

    public static List<Action> ReadActionList(string actionListXMLPath)
    {
        return StandardDeserialization<List<Action>>(actionListXMLPath, "Actions");
    }

    /*
    public static List<Response> ReadResponseList(string responseListXMLPath)
    {
        return StandardDeserialization<List<Response>>(responseListXMLPath, "Responses");
    }
    */

    /*
    public static List<Dialog> ReadDialogList(string dialogListXMLPath)
    {
        return StandardDeserialization<List<Dialog>>(dialogListXMLPath, "Dialogs");
    }
    */

    public static List<Goal> ReadGoals(string goalsXMLPath)
    {
        return StandardDeserialization<List<Goal>>(goalsXMLPath, "Goals");
    }
}
