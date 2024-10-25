using UnityEngine;

public class DNA : MonoBehaviour
{
    // Genes for the red, green, and blue color components
    public float r;
    public float g;
    public float b;

    // Enum representing the fitness level of the individual
    public FitnessLevel fitnessLevel;

    // Cached reference to the MeshRenderer component
    private MeshRenderer meshRenderer;

    // Called at the start to initialize the individual's appearance and fitness
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetColor();
    }

    // Sets the color of the individual based on its DNA (r, g, b)
    public void SetColor()
    {
        meshRenderer.material.color = new Color(r, g, b);
    }
}
