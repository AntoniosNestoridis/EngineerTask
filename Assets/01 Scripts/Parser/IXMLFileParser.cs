using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using System.Text.RegularExpressions;

public class IXMLFileParser : MonoBehaviour
{

    public string FileDirectory; // TODO: Eventually we want this to be the Application.Filepath
    public string filename;

    public bool ReadFile;

    void Start()
    {
       
    }


    void Update()
    {
        if (ReadFile)
        {
            ReadFile = false;
            CreateBrickData();
        }
    }



    public void CreateBrickData()
    {

        // 1. Read text file from disk
        string fileContents = ReadFileFromDisk(FileDirectory,filename);

        // 2. Parse file into a data form
        if (!string.IsNullOrEmpty(fileContents))
        {
            ParseFile(fileContents);
        }

        // 3. Display results
    }


    /// <summary>
    /// Parses an IXFML file and creates and returns a new instance of a model or null.
    /// </summary>
    public Model ParseFile(string fileContents)
    {

        Model newImportedModel = new Model();

        // We assume a valid string here
        Debug.Log("Attempting to Parse text file into data...");

        string text = fileContents;
        string brickListPattern = @"(<Brick .*>)\n(.*>)\n(.*>)"; // Matches one Brick instance and captures in separate groups the brick data, part data and bone data. Not the best but dont want to spend more time on pretty Regex
       // string brickDataPattern= @"(<Brick .*>)";
       // string partDataPattern= @"(<Part .*>)";
        //string boneDataPattern= @"(<Bone .*>)";

        // Instantiate the regular expression object.
        Regex r = new Regex(brickListPattern, RegexOptions.Multiline);

        // Match the regular expression pattern against a text string.
        var brickMatches = r.Matches(text);
        int matchCount = brickMatches.Count;

        Debug.Log($"Found {matchCount} bricks in model");
        // Assign later?
        newImportedModel.TotalBricks = matchCount;

        // Extra brick, part and bone data from each match
        foreach (Match brickMatch in brickMatches)
        {
           // Debug.Log(brickMatch);

            //Group 1 - Brick
            string brickInfoLine = brickMatch.Groups[1].Value;

            string brickDataPattern = "(designID=\"(.*)\" uuid=\"(.*)\">)";
            Regex brickR = new Regex(brickDataPattern, RegexOptions.None);
            Match brickInfoMatches = brickR.Match(brickInfoLine);

            string designID = brickInfoMatches.Groups[2].Value;
            string uuid = brickInfoMatches.Groups[3].Value;
            
            //Group 2 - Part
            string partInfo = brickMatch.Groups[2].Value;

            // Group 3 - Bone
            string boneInfo = brickMatch.Groups[3].Value;
        }

        //while (m.Success)
        //{
        //    Console.WriteLine("Match"+ (++matchCount));
        //    for (int i = 1; i <= 2; i++)
        //    {
        //    Group g = m.Groups[i];
        //    Console.WriteLine("Group"+i+"='" + g + "'");
        //    CaptureCollection cc = g.Captures;
        //    for (int j = 0; j < cc.Count; j++)
        //    {
        //        Capture c = cc[j];
        //        System.Console.WriteLine("Capture"+j+"='" + c + "', Position="+c.Index);
        //    }
        //    }
        //    m = m.NextMatch();
        //}


        // 1. List of individual bricks

        // 2. Parse brick data

        // 3. List of parts 

        // 4. Parse part data

        // 5. 

        return newImportedModel;
    }

    /// <summary>
    /// Reads a text file from the disk and returns its contents as a string or an empty string if there is an error in the process.
    /// </summary>
    /// <param name="directory">The directory path </param>
    /// <param name="filename"> Name of the file to read </param>
    /// <returns></returns>
    public string ReadFileFromDisk(string directory, string filename)
    {

        string fileContents = "";

        Debug.Log("Attempting to read file contents...");

        try
        {
            // TODO: Check if directory and/or filename exist first before attempting to read them. But I guess the try catch does the same job.
            // Create an instance of StreamReader to read from a file.
            using (StreamReader sr = new StreamReader( directory + "/" + filename))
            {
                fileContents = sr.ReadToEnd();
                
                if (!string.IsNullOrEmpty(fileContents))
                {
                    Debug.Log("Success!");
                }
            }
        }
        catch (Exception e)
        {
            // Error output
            Debug.LogError("The file could not be read:");
            Debug.LogError(e.Message);
        }

        return fileContents;     
    }

    
}
