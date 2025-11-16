using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// A simple parser for IXML file types. 
/// Has the ability to load a file from the disk and convert it into a Model data structure
/// that can be stored and processed by the application.
/// </summary>
public class IXMLFileParser
{
    /// <summary>
    /// Called from UIController. Calls both the file loading and parsing methods.
    /// </summary>
    /// <param name="filename"></param>
    public Model LoadAndParseFile(string filename)
    {
        // 1. Read text file from disk
        string fileContents = ReadFileFromDisk(Application.streamingAssetsPath, filename);

        // 2. Parse file into a data form
        if (!string.IsNullOrEmpty(fileContents))
        {
            Model newModel = ParseFileToModel(fileContents);

            // 3. Print the debug data to the console and return the new loaded model
            if (newModel != null)
            {
                newModel.PrintModelDataToConsole();
                return newModel;
            }         
        }

        return null;
    }



    /// <summary>
    /// Loads and reads a text file from the disk. 
    /// Returns its contents as a singular string if they are valid, otherwise an empty string.
    /// </summary>
    /// <param name="directory">The directory path to load from </param>
    /// <param name="filename"> Name of the file to read </param>
    /// <returns></returns>
    public string ReadFileFromDisk(string directory, string filename)
    {
        Debug.Log($"-- Attempting to load {filename} from path:{directory} --");

        try
        {
            // Create an instance of StreamReader to read from a file.
            using (StreamReader sr = new StreamReader( directory + "/" + filename))
            {
                string fileContents = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(fileContents))
                {
                    Debug.Log("Success!");               
                }

                return fileContents;
            }
        }
        catch (Exception e)
        {
            // Returning empty
            return "";
        }

                        
       
    }

    /// <summary>
    /// Attempts to parse an IXFML file type into a Model structure.
    /// Returns a new Model instance upon success or null if the parsing the file was invalid.
    /// </summary>
    /// <param name="fileContents"> The contents of an IXMFL file type as a string. </param>    
    private Model ParseFileToModel(string fileContents)
    {
        Model newImportedModel = new Model();

        Debug.Log("Attempting to parse text file into data...");

        // 1. Identify brick "blocks" of data
        string brickListPattern = "<Brick(?:.*?|\n)*</Brick>"; 
        Regex r = new Regex(brickListPattern, RegexOptions.Multiline);

        var brickMatches = r.Matches(fileContents);
        int brickMatchCount = brickMatches.Count;

        //No brick matches found, invalid file format or simply empty file.
        if (brickMatchCount == 0)
        {
            Debug.Log("No matches found for brick pattern. File format is incorrect.");
            return null;
        }

        Debug.Log($"-- Found {brickMatchCount} bricks in model description-- ");
        newImportedModel.TotalBricks = brickMatchCount;

        // Caching temporary variables for reuse during data creation below.
        string newBrickDesignID;
        string newBrickUUID;

        string newPartUUID;
        string newPartDesignID;
        string newPartType;
        string newPartMaterials;

        string newBoneUUID;
        string newBoneTransformation;

        List<Brick> bricks = new List<Brick>();

        foreach (Match brickMatch in brickMatches)
        {  
            //2. Extract brick specific data
            string brickInfoLine = brickMatch.Value;

            string brickDataPattern = "(designID=\"(.*)\" uuid=\"(.*)\">)";
            Regex brickR = new Regex(brickDataPattern, RegexOptions.None);
            Match brickInfoMatches = brickR.Match(brickInfoLine);

            newBrickDesignID = brickInfoMatches.Groups[2].Value;
            newBrickUUID = brickInfoMatches.Groups[3].Value;
            

            Brick newBrick = new Brick(newBrickUUID, newBrickDesignID);


            //3. - Extract parts and bones data
            string partInfoLine = brickMatch.Value;

            // Admittedly not the most beautiful regex of all time
            string partAndBoneDataPattern = "<Part uuid=\"(.*)\" designID=\"(.*)\" partType=\"(.*)\" materials=\"(.*)\">(?:\\s|\n)*<Bone uuid=\"(.*)\" transformation=\"(.*)\" />"; 
            Regex partR = new Regex(partAndBoneDataPattern, RegexOptions.Multiline);
            var partMatches = partR.Matches(partInfoLine);

            // Each brick may contain multiple parts
            foreach (Match partMatch in partMatches)
            {
                // Part
                newPartUUID = partMatch.Groups[1].Value;
                newPartDesignID = partMatch.Groups[2].Value;            
                newPartType = partMatch.Groups[3].Value;
                newPartMaterials = partMatch.Groups[4].Value;
                Part newPart = new Part(newPartDesignID, newPartUUID, newPartType, newPartMaterials);

                // Bone
                newBoneUUID = partMatch.Groups[5].Value;
                newBoneTransformation = partMatch.Groups[6].Value;
                Bone newBone = new Bone(newBoneUUID,newBoneTransformation);
               
                newPart.ConnectBone(newBone);
                newBrick.AddPart(newPart);
            }

            bricks.Add(newBrick);
        }

        //3. Finalize brick list into the model
        newImportedModel.bricks = bricks;
        return newImportedModel;
    }


   

    
}
