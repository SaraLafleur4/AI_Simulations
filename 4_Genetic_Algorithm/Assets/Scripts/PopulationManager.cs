using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Enum representing different fitness levels for individuals
public enum FitnessLevel
{
    Best,
    Good,
    NotBad,
    Poor
}

public class PopulationManager : MonoBehaviour
{
    public GameObject individualPrefab;     // Prefab for individual (GameObject) creations (which contain DNA)
    public int populationSize = 20;         // Size of the population for each generation
    public List<GameObject> population;     // List to hold the current population of individuals
    public float mutationRate = 0.01f;      // Mutation rate - chance that an offspring's DNA will mutate
    public int generation = 0;              // Tracks which generation we are currently in

    // Called when the script starts
    private void Start()
    {
        // Initialize the population list and create the first generation
        population = new List<GameObject>();
        InitializePopulation();
    }

    // Creates the initial population of individuals with random genes
    private void InitializePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            // Set a random position for the individual
            Vector3 position = new Vector3(Random.Range(-8, 8), Random.Range(-2.5f, 5.5f), 0);
            // Instantiate the individual prefab
            GameObject individual = Instantiate(individualPrefab, position, Quaternion.identity);

            // Randomize the genes (color components) for the individual
            DNA dna = individual.GetComponent<DNA>();
            dna.r = Random.Range(0.0f, 1.0f);
            dna.g = Random.Range(0.0f, 1.0f);
            dna.b = Random.Range(0.0f, 1.0f);

            // Evaluate the fitness of the newly created individual
            EvaluateFitness(individual);

            // Add the individual to the population list
            population.Add(individual);
        }
    }

    // Breeds a new population from the current population
    public void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();

        // Check if population is non-empty before breeding
        if (population.Count == 0)
        {
            Debug.LogError("Population is empty. Cannot breed new population.");
            return;
        }

        // Generate a new population by breeding pairs of individuals
        for (int i = 0; i < populationSize; i += 2)
        {
            // Select two parents based on their fitness
            GameObject parent1 = SelectParent(population);
            GameObject parent2 = SelectParent(population);

            // Check for valid parent selection before breeding
            if (parent1 != null && parent2 != null)
            {
                // Breed the two parents to produce offspring
                newPopulation.Add(Breed(parent1, parent2));
                newPopulation.Add(Breed(parent2, parent1));
            }
            else
            {
                Debug.LogError("Parent selection failed. Null parent returned.");
            }
        }

        // Destroy the old population before replacing it
        for (int i = 0; i < population.Count; i++)
        {
            Destroy(population[i]);
        }

        // Replace the old population with the new population and move to the next generation
        population = newPopulation;
        generation++;
    }

    // Selects a parent based on weighted random selection, favoring individuals with higher fitness
    GameObject SelectParent(List<GameObject> population)
    {
        // List to hold fitness weights for selection
        List<int> weights = new List<int>();

        // Assign weights based on fitness levels
        foreach (var individual in population)
        {
            FitnessLevel fitness = individual.GetComponent<DNA>().fitnessLevel;

            switch (fitness)
            {
                case FitnessLevel.Best:
                    weights.Add(4);  // Best individuals have the highest weight
                    break;
                case FitnessLevel.Good:
                    weights.Add(3);
                    break;
                case FitnessLevel.NotBad:
                    weights.Add(2);
                    break;
                case FitnessLevel.Poor:
                    weights.Add(1);  // Poor individuals have the lowest weight
                    break;
            }
        }

        // Calculate total weight for random selection
        int totalWeight = weights.Sum();

        // Error check: if total weight is 0, return the first individual
        if (totalWeight == 0)
        {
            Debug.LogError("Total weight is zero. Check if all individuals have been assigned a valid fitness level.");
            return population[0];
        }

        // Select a random value between 0 and totalWeight
        int randomPick = Random.Range(0, totalWeight);

        // Iterate over the population to find the parent based on cumulative weight
        int currentSum = 0;
        for (int i = 0; i < population.Count; i++)
        {
            currentSum += weights[i];

            // Return the selected parent when the random value falls within the current sum
            if (randomPick < currentSum)
            {
                return population[i];
            }
        }

        // Fallback in case of an issue (this should not be reached)
        Debug.LogWarning("Fallback reached in SelectParent method. Returning the last individual.");
        return population[population.Count - 1];
    }


    // Breeds two individuals to create offspring with a mix of parent genes
    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        // Set random position for the offspring
        Vector3 position = new Vector3(Random.Range(-8, 8), Random.Range(-2.5f, 5.5f), 0);
        // Create a new offspring by instantiating the prefab
        GameObject offspring = Instantiate(individualPrefab, position, Quaternion.identity);

        // Get the DNA of both parents
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();
        DNA offspringDNA = offspring.GetComponent<DNA>();

        // Crossover: randomly choose genes from either parent
        offspringDNA.r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
        offspringDNA.g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
        offspringDNA.b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;

        // Apply random mutation based on the mutation rate
        if (Random.Range(0.0f, 1.0f) < mutationRate)
        {
            offspringDNA.r = Random.Range(0.0f, 1.0f);
            offspringDNA.g = Random.Range(0.0f, 1.0f);
            offspringDNA.b = Random.Range(0.0f, 1.0f);
        }

        // Evaluate fitness of the new offspring
        EvaluateFitness(offspring);

        return offspring;
    }

    // Evaluates fitness of an individual based on gene expression (here, the green component)
    private void EvaluateFitness(GameObject individual)
    {
        DNA dna = individual.GetComponent<DNA>();

        // Simple fitness function based on the green color component
        if (dna.g >= 0.6f)
        {
            dna.fitnessLevel = FitnessLevel.Best;
        }
        else if (dna.g >= 0.3f && dna.g < 0.6f)
        {
            dna.fitnessLevel = FitnessLevel.Good;
        }
        else if (dna.g >= 0.1f && dna.g < 0.3f)
        {
            dna.fitnessLevel = FitnessLevel.NotBad;
        }
        else
        {
            dna.fitnessLevel = FitnessLevel.Poor;
        }
    }
}
