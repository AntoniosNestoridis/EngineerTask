using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private IXMLFileParser parser;

    
    [SerializeField]
    private TMP_InputField filenameTextField;

    [Space]
    [SerializeField]
    private TextMeshProUGUI totalBricksValue;
    [SerializeField]
    private TextMeshProUGUI totalPartsValue;
    [SerializeField]
    private TextMeshProUGUI uniquePartsValue;

    [Space]
    [SerializeField]
    private RectTransform brickListContentParent;
    [SerializeField]
    private GameObject brickDataDisplayPrefab;

    

    // --- Button control functionality

    /// <summary>
    /// Clears the current model reference and all the displayed UI data
    /// </summary>
    public void ClearCurrentModel()
    {
        // TODO:
    }

    /// <summary>
    /// Calls the pars
    /// </summary>
    public async void LoadAndParseModel()
    {
        // Parser call 
        Model loadedModel = parser.LoadAndParseFile(filenameTextField.text);

        if (loadedModel == null)
        {
            Debug.LogError("File or model could not be loaded correctly");

            // TODO: Display error

            return;
        }

        // Display general model info
        totalBricksValue.text = loadedModel.TotalBricks.ToString();
        totalPartsValue.text = loadedModel.GetTotalPartsCount().ToString();
        uniquePartsValue.text = "0";// loadedModel.GetUniquesPartsCount(loadedModel);
        
        // Display brick list info
        for (int i = 0; i < loadedModel.bricks.Count; i++)
        {
            // Create a data display prefab and parent it under the content
            BrickDataDisplay newDataDisplay = Instantiate(brickDataDisplayPrefab, brickListContentParent).GetComponent<BrickDataDisplay>();

            // Setup values
            newDataDisplay.SetupDisplayData(loadedModel.bricks[i], i );

            await Task.Yield();
        }


    }
}
