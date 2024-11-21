using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    // UI sliders for configuring the number of rows and columns for input and output nodes
    public Slider nodeRowCountSlider;
    public Slider inputColumnCountSlider;
    public Slider outputColumnCountSlider;

    // Text components to display the current values of the sliders in the UI
    public TMP_Text nodeRowCountText;
    public TMP_Text inputColumnCountText;
    public TMP_Text outputColumnCountText;

    // UI buttons to trigger actions: generating nodes, starting the network, clearing colors, or clearing nodes
    public Button generateNodes;
    public Button startNeuralNetwork;
    public Button clearColors;
    public Button clearNodes;

    // References to helper components: a visualizer for rendering nodes and a neural network instance for computation
    public Visualizer visualizer;
    public NeuralNetwork neuralNetwork;

    private void Start()
    {
        // Attach listeners to slider value changes to update the displayed text
        nodeRowCountSlider.onValueChanged.AddListener(delegate { UpdateSliderText(); });
        inputColumnCountSlider.onValueChanged.AddListener(delegate { UpdateSliderText(); });
        outputColumnCountSlider.onValueChanged.AddListener(delegate { UpdateSliderText(); });

        // Attach listeners to button clicks to invoke their respective methods
        generateNodes.onClick.AddListener(delegate { GenerateNodes(); });
        startNeuralNetwork.onClick.AddListener(delegate { StartNeuralNetwork(); });
        clearColors.onClick.AddListener(delegate { ClearColors(); });
        clearNodes.onClick.AddListener(delegate { ClearNodes(); });

        // Add a NeuralNetwork component to this GameObject
        neuralNetwork = gameObject.AddComponent<NeuralNetwork>();
    }

    // Updates the text components to display the current slider values
    private void UpdateSliderText()
    {
        nodeRowCountText.text = Mathf.RoundToInt(nodeRowCountSlider.value).ToString();
        inputColumnCountText.text = Mathf.RoundToInt(inputColumnCountSlider.value).ToString();
        outputColumnCountText.text = Mathf.RoundToInt(outputColumnCountSlider.value).ToString();
    }

    // Generates visual nodes in the scene based on the slider values
    public void GenerateNodes()
    {
        // Retrieve the integer values from each slider
        int rowNb = Mathf.RoundToInt(nodeRowCountSlider.value);
        int inputColumnNb = Mathf.RoundToInt(inputColumnCountSlider.value);
        int outputColumnNb = Mathf.RoundToInt(outputColumnCountSlider.value);

        // Use the visualizer to create the nodes in the scene
        visualizer.GenerateColumns(rowNb, inputColumnNb, outputColumnNb);
    }

    // Initializes and starts the neural network, training it with patterns and visualizing results
    public void StartNeuralNetwork()
    {
        // Retrieve the integer values from each slider
        int rowNb = Mathf.RoundToInt(nodeRowCountSlider.value);
        int inputColumnNb = Mathf.RoundToInt(inputColumnCountSlider.value);
        int outputColumnNb = Mathf.RoundToInt(outputColumnCountSlider.value);

        // Initialize the neural network with input nodes, hidden nodes, and output nodes
        neuralNetwork.Initialize(inputColumnNb, 10, outputColumnNb);

        // Retrieve training patterns from the visualizer
        List<(List<double>, List<double>)> pattern = visualizer.GetPattern(rowNb, inputColumnNb, outputColumnNb);

        // Train the neural network using the patterns
        neuralNetwork.TrainNetwork(pattern);

        // Test the network with the same patterns and retrieve outputs
        List<List<double>> outputs = neuralNetwork.TestNetwork(pattern);

        // Apply the results from the neural network to the visualized nodes
        visualizer.ApplyResultsToNodes(outputs);
    }

    // Resets the colors of all nodes to their default state
    public void ClearColors()
    {
        visualizer.ClearColors();
    }

    // Removes all visualized nodes from the scene
    public void ClearNodes()
    {
        visualizer.ClearNodes();
    }
}
