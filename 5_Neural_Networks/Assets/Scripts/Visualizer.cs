using System;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    // Prefabs for creating visual representations of input and output nodes
    public GameObject inputNodePrefab;     // Cube for input nodes
    public GameObject outputNodePrefab;    // Sphere for output nodes

    // Lists to store references to the instantiated nodes
    private List<GameObject> inputNodes = new List<GameObject>();
    private List<GameObject> desiredOutputNodes = new List<GameObject>();
    private List<GameObject> actualOutputNodes = new List<GameObject>();

    // Generates columns of input, desired output, and actual output nodes
    public void GenerateColumns(int rowNb, int inputColumnNb, int outputColumnNb)
    {
        // Clear any previously generated nodes
        ClearNodes();

        // Set average spacing between nodes
        float avgOffset = 1.0f;

        // Define x-axis positions for the three types of columns
        int firstInputColumn = -7;
        int firstDesiredOutputColumn = -2;
        int firstActualOutputColumn = 4;

        // Generate input columns
        for (int col = 0; col < inputColumnNb; col++)
        {
            for (int row = 0; row < rowNb; row++)
            {
                // Position nodes in the grid
                Vector3 position = new Vector3(col * avgOffset + firstInputColumn, row * avgOffset, 0);
                GameObject node = Instantiate(inputNodePrefab, position, Quaternion.identity, transform);
                inputNodes.Add(node);
            }
        }

        // Generate desired output columns
        for (int col = 0; col < outputColumnNb; col++)
        {
            for (int row = 0; row < rowNb; row++)
            {
                // Position nodes in the grid
                Vector3 position = new Vector3(col * avgOffset + firstDesiredOutputColumn, row * avgOffset, 0);
                GameObject node = Instantiate(outputNodePrefab, position, Quaternion.identity, transform);
                desiredOutputNodes.Add(node);
            }
        }

        // Generate actual output columns
        for (int col = 0; col < outputColumnNb; col++)
        {
            for (int row = 0; row < rowNb; row++)
            {
                // Position nodes in the grid
                Vector3 position = new Vector3(col * avgOffset + firstActualOutputColumn, row * avgOffset, 0);
                GameObject node = Instantiate(outputNodePrefab, position, Quaternion.identity, transform);
                actualOutputNodes.Add(node);
            }
        }
    }

    // Clears all previously generated nodes
    public void ClearNodes()
    {
        // Destroy input nodes and clear their list
        foreach (GameObject node in inputNodes)
            Destroy(node);
        inputNodes.Clear();

        // Destroy desired output nodes and clear their list
        foreach (GameObject node in desiredOutputNodes)
            Destroy(node);
        desiredOutputNodes.Clear();

        // Destroy actual output nodes and clear their list
        foreach (GameObject node in actualOutputNodes)
            Destroy(node);
        actualOutputNodes.Clear();
    }

    // Applies results from the neural network to the actual output nodes by coloring them
    public void ApplyResultsToNodes(List<List<double>> outputs)
    {
        // Ensure the number of outputs matches the number of actual output nodes
        int totalOutputs = outputs.Count * outputs[0].Count;
        if (totalOutputs != actualOutputNodes.Count)
        {
            throw new ArgumentException($"Wrong number of outputs, totalOutputs = {totalOutputs}, should be {actualOutputNodes.Count}");
        }

        // Apply color to each output node based on the computed output value
        for (int row = 0; row < outputs.Count; row++)
        {
            for (int col = 0; col < outputs[row].Count; col++)
            {
                int nodeIndex = row + col * outputs.Count;
                Color resultColor = outputs[row][col] > 0.5 ? Color.yellow : Color.white;
                actualOutputNodes[nodeIndex].GetComponent<Renderer>().material.color = resultColor;
            }
        }
    }

    // Resets the colors of all nodes to their default state (white)
    public void ClearColors()
    {
        // Reset colors of input nodes
        foreach (GameObject input in inputNodes)
            input.GetComponent<Renderer>().material.color = Color.white;

        // Reset colors of desired output nodes
        foreach (GameObject desiredOutput in desiredOutputNodes)
            desiredOutput.GetComponent<Renderer>().material.color = Color.white;

        // Reset colors of actual output nodes
        foreach (GameObject actualOutput in actualOutputNodes)
            actualOutput.GetComponent<Renderer>().material.color = Color.white;
    }

    // Extracts training patterns from the input and output node colors
    public List<(List<double>, List<double>)> GetPattern(int rowCount, int inputColumnCount, int outputColumnCount)
    {
        var pattern = new List<(List<double>, List<double>)>();

        // Iterate through each row of nodes
        for (int row = 0; row < rowCount; row++)
        {
            List<double> inputRow = new List<double>();
            List<double> outputRow = new List<double>();

            // Extract input node values based on their color
            for (int col = 0; col < inputColumnCount; col++)
            {
                int nodeIndex = row + col * rowCount;
                double inputValue = inputNodes[nodeIndex].GetComponent<Renderer>().material.color == Color.yellow ? 1.0 : 0.0;
                inputRow.Add(inputValue);
            }

            // Extract desired output node values based on their color
            for (int col = 0; col < outputColumnCount; col++)
            {
                int nodeIndex = row + col * rowCount;
                double outputValue = desiredOutputNodes[nodeIndex].GetComponent<Renderer>().material.color == Color.yellow ? 1.0 : 0.0;
                outputRow.Add(outputValue);
            }

            // Add the row's input and output values as a training pattern
            pattern.Add((inputRow, outputRow));
        }

        return pattern;
    }
}
