using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Julia : MonoBehaviour
{
    private double height, width; // Dimensions of the Mandelbrot view
    private double rStart, iStart; // Starting points for the real and imaginary axes
    [SerializeField] public double rC, iC; // Constants for the Julia set
    private int maxIterations; // Maximum number of iterations for the Mandelbrot calculation
    private int zoom; // Zoom level for the fractal view

    private Texture2D display; // Texture for displaying the Mandelbrot fractal
    public Image image; // UI Image component to show the texture

    // Start is called before the first frame update
    void Start()
    {
        // Set initial width and calculate height based on screen dimensions
        width = 4.5;
        height = width * (Screen.height / Screen.width);

        // Set initial positions for real and imaginary axes
        rStart = -2.0;
        iStart = -1.5;

        // Set zoom and maximum iterations
        zoom = 10;
        maxIterations = 100;

        // Constants for the Julia set (change these values to create different Julia sets)
        rC = -0.7;
        iC = 0.27015;

        // Initialize the texture with the screen dimensions
        display = new Texture2D(Screen.width, Screen.height);

        // Generate the initial Julia fractal
        RunJulia();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle mouse click to move the view
        if (Input.GetMouseButtonDown(0))
        {
            // Recenter the view to the point clicked (adjust the starting point)
            rStart += (Input.mousePosition.x - (Screen.width / 2.0)) / Screen.width * width;
            iStart += (Input.mousePosition.y - (Screen.height / 2.0)) / Screen.height * height;

            // Generate the Julia fractal with the new starting points
            RunJulia();
        }

        // Handle mouse scroll to zoom in/out
        if (Input.mouseScrollDelta.y != 0)
        {
            // Calculate zoom factors for width and height based on mouse scroll
            double wFactor = width * (double)Input.mouseScrollDelta.y / zoom;
            double hFactor = height * (double)Input.mouseScrollDelta.y / zoom;

            // Adjust width and height for zoom
            width -= wFactor;
            height -= hFactor;

            // Adjust starting points for centering the view
            rStart += wFactor / 2.0;
            iStart += hFactor / 2.0;

            // Generate the Julia fractal with the new zoom level
            RunJulia();
        }
    }

    // Sets the color of each pixel of the canvas, according to the Julia algorithm
    private void RunJulia()
    {
        // Iterate over each pixel in the display texture
        for (int x = 0; x != display.width; x++)
        {
            for (int y = 0; y != display.height; y++)
            {
                // Calculate color based on Julia function
                display.SetPixel(x, y,
                    SetColor(JuliaFunction(rStart + width * (double)x / display.width,
                                            iStart + height * (double)y / display.height)));
            }
        }

        // Apply changes to the texture
        display.Apply();

        // Create a sprite from the texture and assign it to the UI image
        image.sprite = Sprite.Create(display, new Rect(0, 0, display.width, display.height),
                                       new UnityEngine.Vector2(0.5f, 0.5f));
    }

    // The Julia set algorithm to determine the number of iterations
    private int JuliaFunction(double r, double i)
    {
        int iterations = 0; // Count of iterations for determining escape time

        Complex z = new Complex(r, i); // Initialize complex number z

        // Iterate up to the maximum number of iterations
        for (int j = 0; j != maxIterations; j++)
        {
            // Update z based on the Julia formula
            z = z * z + new Complex(rC, iC);

            // Check if the magnitude of z exceeds 2 (escape condition)
            if (Complex.Abs(z) > 2)
            {
                break; // Exit if it escapes
            }
            else
            {
                iterations++; // Increment iteration count
            }
        }

        return iterations; // Return the number of iterations before escape
    }

    // Sets one of 16 different colors defined below for the Julia set
    Color SetColor(int value)
    {
        UnityEngine.Vector4 CalcColor = new UnityEngine.Vector4(0, 0, 0, 1f); // Initialize color variable

        // If the value is not equal to the maximum iterations
        if (value != maxIterations)
        {
            int colorNr = value % 16; // Use modulus to loop through 16 colors

            // Manually define 16 color shades from yellow to deep purple
            switch (colorNr)
            {
                case 0:
                    CalcColor[0] = 255.0f / 255.0f; // Bright Yellow (start of gradient)
                    CalcColor[1] = 255.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 1:
                    CalcColor[0] = 255.0f / 255.0f;
                    CalcColor[1] = 230.0f / 255.0f; // Light Yellow
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 2:
                    CalcColor[0] = 255.0f / 255.0f;
                    CalcColor[1] = 204.0f / 255.0f; // Soft Yellow
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 3:
                    CalcColor[0] = 255.0f / 255.0f;
                    CalcColor[1] = 178.0f / 255.0f; // Yellowish
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 4:
                    CalcColor[0] = 255.0f / 255.0f;
                    CalcColor[1] = 153.0f / 255.0f; // Yellow
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 5:
                    CalcColor[0] = 255.0f / 255.0f;
                    CalcColor[1] = 128.0f / 255.0f; // Yellow to Red
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 6:
                    CalcColor[0] = 255.0f / 255.0f; // Light Orange
                    CalcColor[1] = 102.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 7:
                    CalcColor[0] = 255.0f / 255.0f; // Orange
                    CalcColor[1] = 76.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 8:
                    CalcColor[0] = 255.0f / 255.0f; // Dark Orange
                    CalcColor[1] = 51.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 9:
                    CalcColor[0] = 255.0f / 255.0f; // Light Red
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 10:
                    CalcColor[0] = 204.0f / 255.0f; // Red
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 11:
                    CalcColor[0] = 153.0f / 255.0f; // Dark Red
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 12:
                    CalcColor[0] = 102.0f / 255.0f; // Deep Red
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 0.0f / 255.0f;
                    break;
                case 13:
                    CalcColor[0] = 76.0f / 255.0f; // Light Purple
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 76.0f / 255.0f;
                    break;
                case 14:
                    CalcColor[0] = 51.0f / 255.0f; // Purple
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 102.0f / 255.0f;
                    break;
                case 15:
                    CalcColor[0] = 25.0f / 255.0f; // Deep Purple (end of gradient)
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 128.0f / 255.0f;
                    break;
            }
        }

        return CalcColor; // Return the calculated color
    }
}
