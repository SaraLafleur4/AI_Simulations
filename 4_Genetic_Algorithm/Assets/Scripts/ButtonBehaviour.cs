using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    // Reference to the PopulationManager, which manages the genetic algorithm
    public PopulationManager populationManager;

    // This method is called when the button is clicked
    public void OnNextGenerationButtonClicked()
    {
        // This triggers the process of breeding a new generation of individuals
        populationManager.BreedNewPopulation();
    }
}
