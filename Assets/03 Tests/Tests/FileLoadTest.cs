using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FileLoadTest
{
    // A Test behaves as an ordinary method
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

    [Test]
    public void FileLoadingTestInvalidFile()
    {
        // 1. Arrange
        IXMLFileParser fileParser = new IXMLFileParser();
        string directory = Application.streamingAssetsPath;
        string filename = "invald.lxMFl"; // Non existent filename
        string expectedResult = "";
        // 2. Act
        string result = fileParser.ReadFileFromDisk(directory, filename);

        // 3. Assert
        Assert.That(result, Is.Empty);
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}
