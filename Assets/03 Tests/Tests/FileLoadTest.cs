using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FileLoadTest
{
    // A test to check a valid text file
    [Test]
    public void FileLoadingTestValidFile()
    {
        // 1. Arrange
        IXMLFileParser fileParser = new IXMLFileParser();
        string directory = Application.streamingAssetsPath;
        string filename = "everyoneisawesome.lxfml";

        // 2. Act
        string result = fileParser.ReadFileFromDisk(directory, filename);

        // 3. Assert
        Assert.That(result, Is.Not.Empty);
    }

    // A test to check a loading attempt with a non existent or wrong file
    [Test]
    public void FileLoadingTestInvalidFile()
    {
        // 1. Arrange
        IXMLFileParser fileParser = new IXMLFileParser();
        string directory = Application.streamingAssetsPath;
        string filename = "invald.lxMFl";
        string expectedResult = "";

        // 2. Act
        string result = fileParser.ReadFileFromDisk(directory, filename);

        // 3. Assert
        Assert.That(result, Is.Empty);
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}
