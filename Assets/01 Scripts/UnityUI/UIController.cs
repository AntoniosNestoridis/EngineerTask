using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;

/// <summary>
/// A simple controller class that handles all functionality that is UI related.
/// Handles both user input and is responsible for displaying the data back to them.
/// </summary>
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

    private CancellationTokenSource source;
    private CancellationToken clearToken;


    private void Awake()
    {
        source = new CancellationTokenSource();
    }


    // -- Button control functionality

    /// <summary>
    /// Asks the parser to load and parse the model data and if successful display all relevant information to the user.
    /// Connected directly to the "Load" button.
    /// Note: Used a secondary internal function, so that the proper Cancellation token can be passed.
    /// </summary>
    public void LoadModelUI()
    {
        LoadAndParseModel(Application.exitCancellationToken, source.Token);
    }


    /// <summary>
    /// Clears all information from the displayed data. Connected directly to the "Clear" button.
    /// </summary>
    public void ClearDisplayData()
    {
        // Cancel potential current loading
        source.Cancel();
        source.Dispose();
        source = new CancellationTokenSource();

        ClearAll();
    }

    // --- 

    /// <summary>
    /// Actually executes the connection with the parser and handles the displaying of all the retrieved data.
    /// </summary>
    private async void LoadAndParseModel(CancellationToken applicationExitToken, CancellationToken clearAllToken)
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
        uniquePartsValue.text = loadedModel.GetUniquePartsCount().ToString();

        // Display brick list info 
        for (int i = 0; i < loadedModel.bricks.Count; i++)
        {
            // Create a data display prefab and parent it under the content
            BrickDataDisplay newDataDisplay = Instantiate(brickDataDisplayPrefab, brickListContentParent).GetComponent<BrickDataDisplay>();

            // Setup values
            newDataDisplay.SetupDisplayData(loadedModel.bricks[i], i);

            await Task.Yield();

            // Cancel the operation and clear if the application exits
            if (applicationExitToken.IsCancellationRequested || clearAllToken.IsCancellationRequested)
            {
                Debug.Log("Cancelling brick data display method due to application exit");
                return;
            }

        }
    }

    private void ClearAll()
    {
        totalBricksValue.text = "0";
        totalPartsValue.text = "0";
        uniquePartsValue.text = "0";

        // Destroy all contents of the bricklist
        foreach(Transform child in brickListContentParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
