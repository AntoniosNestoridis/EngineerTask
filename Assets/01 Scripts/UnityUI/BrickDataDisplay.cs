using TMPro;
using UnityEngine;

public class BrickDataDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI brickNumberID;
    [SerializeField]
    private TextMeshProUGUI designUIValue;
    [SerializeField]
    private TextMeshProUGUI uuidUIValue;
    [SerializeField]
    private TextMeshProUGUI partsAndBonesTextValue;

    public void SetupDisplayData(Brick brickData , int index)
    {
        brickNumberID.text = index.ToString();
        designUIValue.text = brickData.designID;
        uuidUIValue.text = brickData.uuid;

        // Creating the text structure for parts and bones info

        string targetText = ""; // The final text to produce for the parts and bones

        // Caching vars
        string partText = ""; 
        string boneText = "";

        for (int i =0; i<  brickData.parts.Count; i++)
        {
            Part currentPart = brickData.parts[i];

            partText = $"Part# {i} - DesignID:{currentPart.designID} - UUID: {currentPart.uuID} -\n Type:{currentPart.partType} - Materials:{currentPart.materials}\n";
            boneText = $"Bone# {i} - UUID: {currentPart.bone.uuID} -\n Transformation:{currentPart.partType}\n" ;

            // Concat the new text info with the current one
            targetText = targetText + 
                        partText + 
                        boneText + 
                        "\n";
        }

        partsAndBonesTextValue.text = targetText;
    }

    
}
