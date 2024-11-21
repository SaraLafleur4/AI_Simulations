using UnityEngine;

public class NodeObject : MonoBehaviour
{
    // Tracks whether the node is active (yellow) or inactive (white)
    private bool isActive = false;

    // Sets up the node with an initial color
    public void SetupNode(Color initialColor)
    {
        SetColor(initialColor); // Apply the initial color to the node
    }

    // Called when the object is clicked
    private void OnMouseDown()
    {
        ToggleColor(); // Change the node's color state
    }

    // Toggles the color of the node between yellow (active) and white (inactive)
    private void ToggleColor()
    {
        isActive = !isActive; // Flip the active state
        SetColor(isActive ? Color.yellow : Color.white); // Apply the corresponding color
    }

    // Updates the node's material color
    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color; // Change the Renderer’s material color
    }
}
