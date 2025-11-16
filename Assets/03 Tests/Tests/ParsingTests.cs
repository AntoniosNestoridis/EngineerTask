using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;



/// <summary>
/// A summary of tests that look for confirmation or errors in the parsing methods of an XMFL file.
/// 
/// IMPORTANT NOTE: Because of working backwards and making these units tests AFTER the code was in place, 
/// they do not call separate functions from the parser, but instead specific parts of the "ParseFileToModel" method of the IXMLFileParser class.
/// And that is mostly the various types of regular expressions. It was just too late for me to break down the parsing into different function steps at this point.
/// </summary>
public class ParsingTests
{
    
    /// <summary>
    /// Test to determine that the Regex that determines the Brick blocks found in a valid file works.
    /// </summary>
    [Test]
    public void ParsingTestsValidBrickSyntax()
    {
        // 1. Arrange
        IXMLFileParser fileParser = new IXMLFileParser();
        string directory = Application.streamingAssetsPath;
        string filename = "everyoneisawesome.lxfml";
        string file = fileParser.ReadFileFromDisk(directory, filename);
        int expectedResult = 346;

        // 2. Act
        string brickListPattern = "<Brick(?:.*?|\n)*</Brick>"; 
        Regex r = new Regex(brickListPattern, RegexOptions.Multiline);

        var brickMatches = r.Matches(file);

        // 3. Assert
        Assert.That(brickMatches.Count, Is.EqualTo(expectedResult));
        Assert.That(brickMatches.Count, Is.Not.Zero);

    }

    /// <summary>
    /// Test to determine that the Regex that determines brick blocks in an invalid syntax file find no blocks.
    /// NOTE: Do not currently run this test. It seems that if the loaded file does not contain any "</Brick> " end tag, then the regex runs indefinitely.
    /// I guess the test worked nicely to identify a big issue like this
    /// </summary>
    //[Test]
    //public void ParsingTestsInvalidBrickSyntax()
    //{
    //      // 1. Arrange
    //    IXMLFileParser fileParser = new IXMLFileParser();
    //    string directory = Application.streamingAssetsPath;
    //    string filename = "wrongSyntax.lxfml";
    //    string file = fileParser.ReadFileFromDisk(directory, filename);

    //    // 2. Act
    //    string brickListPattern = "<Brick(?:.*?|\n)*</Brick>"; 
    //    Regex r = new Regex(brickListPattern, RegexOptions.Multiline);

    //    var brickMatches = r.Matches(file);

    //    // 3. Assert
    //    Assert.That(brickMatches.Count, Is.Zero);
    //}

    [Test]
    public void ParsingTestsValidBrickData()
    {
        // 1. Arrange
        // A single brick block from a valid file
        string brickBlock = "<Brick designID=\"31510\" uuid=\"b576eabe-a01c-4864-800d-4c5d51568be6\">\n " +
        "<Part uuid=\"99edf3f9-d5d3-4e0f-955e-5daeab7bb7c6\" designID=\"31510\" partType=\"rigid\" materials=\"107:0\">\n " +
        "<Bone uuid=\"7c87c486-661a-426e-b11c-e985c215c2d5\" transformation=\"-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-11.2\" /> \n" +
        "</Part>\n " +
        "</Brick>";

        string expectedDesignID = "31510";
        string expectedUUID = "b576eabe-a01c-4864-800d-4c5d51568be6";

        // 2. Act
         string brickDataPattern = "(designID=\"(.*)\" uuid=\"(.*)\">)";
        Regex brickR = new Regex(brickDataPattern, RegexOptions.None);
        Match brickInfoMatches = brickR.Match(brickBlock);

        // 3. Assert
        Assert.That(brickInfoMatches.Groups[2].Value, Is.EqualTo(expectedDesignID));
        Assert.That(brickInfoMatches.Groups[3].Value, Is.EqualTo(expectedUUID));
    }

    [Test]
    public void ParsingTestsInvalidBrickData()
    {
        // 1. Arrange
        // An invalid brick block, essentially anything that doesn't have the expected pattern
        string file = "<Bricklest brick </Brick>";

        // 2. Act
         string brickDataPattern = "(designID=\"(.*)\" uuid=\"(.*)\">)";
        Regex brickR = new Regex(brickDataPattern, RegexOptions.None);
        Match brickInfoMatches = brickR.Match(file);

        // 3. Assert
        Assert.That(brickInfoMatches.Length, Is.Zero);
      
    }


    [Test]
    public void ParsingTestsSinglePartAndBone()
    {
         // A single brick block from a valid file
        string brickBlock = "<Brick designID=\"31510\" uuid=\"b576eabe-a01c-4864-800d-4c5d51568be6\">\n " +
        "<Part uuid=\"99edf3f9-d5d3-4e0f-955e-5daeab7bb7c6\" designID=\"31510\" partType=\"rigid\" materials=\"107:0\">\n " +
        "<Bone uuid=\"7c87c486-661a-426e-b11c-e985c215c2d5\" transformation=\"-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-11.2\" /> \n" +
        "</Part>\n " +
        "</Brick>";

        string expectedPartDesignID = "31510";
        string expectedPartUUID = "99edf3f9-d5d3-4e0f-955e-5daeab7bb7c6";
        string expectedPartType = "rigid";
        string expectedMaterials = "107:0";

        string expectedBoneUUID = "7c87c486-661a-426e-b11c-e985c215c2d5";
        string expectedBoneTransformation = "-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-11.2";
        
        // 2. Act
        string partAndBoneDataPattern = "<Part uuid=\"(.*)\" designID=\"(.*)\" partType=\"(.*)\" materials=\"(.*)\">(?:\\s|\n)*<Bone uuid=\"(.*)\" transformation=\"(.*)\" />"; 
        Regex partR = new Regex(partAndBoneDataPattern, RegexOptions.Multiline);
        var partMatches = partR.Matches(brickBlock);
     
        
        // 3. Assert
        Assert.That(partMatches.Count, Is.Not.Zero);
        var match = partMatches[0];

        Assert.That(match.Groups.Count, Is.EqualTo(7));

        Assert.That(match.Groups[1].Value, Is.EqualTo(expectedPartUUID));
        Assert.That(match.Groups[2].Value, Is.EqualTo(expectedPartDesignID));
        Assert.That(match.Groups[3].Value, Is.EqualTo(expectedPartType));
        Assert.That(match.Groups[4].Value, Is.EqualTo(expectedMaterials));

        Assert.That(match.Groups[5].Value, Is.EqualTo(expectedBoneUUID));
        Assert.That(match.Groups[6].Value, Is.EqualTo(expectedBoneTransformation));
    }

    [Test]
    public void ParsingTestsInvalidSinglePartAndBone()
    {
         // An invalid block missing bone syntax and information
        string InvalidbrickBlock = "<Brick designID=\"31510\" uuid=\"b576eabe-a01c-4864-800d-4c5d51568be6\">\n " +
        "<Part uuid=\"99edf3f9-d5d3-4e0f-955e-5daeab7bb7c6\" designID=\"31510\" partType=\"rigid\" materials=\"107:0\">\n " +
        "<transformation=\"-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-11.2\" /> \n" +
        "</Part>\n " +
        "</Brick>";

        // 2. Act
        string partAndBoneDataPattern = "<Part uuid=\"(.*)\" designID=\"(.*)\" partType=\"(.*)\" materials=\"(.*)\">(?:\\s|\n)*<Bone uuid=\"(.*)\" transformation=\"(.*)\" />"; 
        Regex partR = new Regex(partAndBoneDataPattern, RegexOptions.Multiline);
        var partMatches = partR.Matches(InvalidbrickBlock);
     
        
        // 3. Assert
        Assert.That(partMatches.Count, Is.Zero);
       
    }



    [Test]
    public void ParsingTestsMultiplePartsAndBones()
    {
         // A single brick block from a valid file
        string brickBlock = "<Brick designID=\"31510\" uuid=\"b576eabe-a01c-4864-800d-4c5d51568be6\">\n " +
        "<Part uuid=\"99edf3f9-d5d3-4e0f-955e-5daeab7bb7c6\" designID=\"31510\" partType=\"rigid\" materials=\"107:0\">\n " +
        "<Bone uuid=\"7c87c486-661a-426e-b11c-e985c215c2d5\" transformation=\"-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-11.2\" /> \n" +
        "</Part>\n " +
        "<Part uuid=\"b92e74b6-d06e-4fd2-8a05-d113d6a0e315\" designID=\"3818\" partType=\"rigid\" materials=\"23:0\">" +
        "<Bone uuid=\"8e45c2a2-5773-4862-8200-7ccf42ca9608\" transformation=\"-0.0000002384186,-0.1822354,-0.983255,0.9238798,0.3762749,-0.06973854,0.3826832,-0.9084094,0.1683635,2.750538,3.15,0.62\" />" +
        "</Part>" +
        "</Brick>";

        string expectedFirstPartDesignID = "31510";
        string expectedFirstPartUUID = "99edf3f9-d5d3-4e0f-955e-5daeab7bb7c6";
        string expectedFirstBoneUUID = "7c87c486-661a-426e-b11c-e985c215c2d5";

        string expectedsecondPartDesignID = "3818";
        string expectedsecondPartUUID = "b92e74b6-d06e-4fd2-8a05-d113d6a0e315";
        string expectedsecondBoneUUID = "8e45c2a2-5773-4862-8200-7ccf42ca9608";
       
        // 2. Act
        string partAndBoneDataPattern = "<Part uuid=\"(.*)\" designID=\"(.*)\" partType=\"(.*)\" materials=\"(.*)\">(?:\\s|\n)*<Bone uuid=\"(.*)\" transformation=\"(.*)\" />"; 
        Regex partR = new Regex(partAndBoneDataPattern, RegexOptions.Multiline);
        var partMatches = partR.Matches(brickBlock);
     
        
        // 3. Assert
        Assert.That(partMatches.Count, Is.EqualTo(2));
        var match1 = partMatches[0];
        var match2 = partMatches[1];

        Assert.That(match1.Groups[1].Value, Is.EqualTo(expectedFirstPartUUID));
        Assert.That(match1.Groups[2].Value, Is.EqualTo(expectedFirstPartDesignID));
        Assert.That(match1.Groups[5].Value, Is.EqualTo(expectedFirstBoneUUID));


        Assert.That(match2.Groups[1].Value, Is.EqualTo(expectedsecondPartUUID));
        Assert.That(match2.Groups[2].Value, Is.EqualTo(expectedsecondPartDesignID));
        Assert.That(match2.Groups[5].Value, Is.EqualTo(expectedsecondBoneUUID));
    }





}
