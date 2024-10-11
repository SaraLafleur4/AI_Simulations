using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Mandelbrot : MonoBehaviour
{
    private double height, width; // Dimensions of the Mandelbrot view
    private double rStart, iStart; // Starting points for the real and imaginary axes
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
        rStart = -3.0;
        iStart = -2.0;

        // Set zoom and maximum iterations
        zoom = 10;
        maxIterations = 100;

        // Initialize the texture with the screen dimensions
        display = new Texture2D(Screen.width, Screen.height);

        // Generate the Mandelbrot fractal
        RunMandelbrot();
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

            // Generate the Mandelbrot fractal with the new starting points
            RunMandelbrot();
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

            // Generate the Mandelbrot fractal with the new zoom level
            RunMandelbrot();
        }
    }

    // Sets the color of each pixel of the canvas, according to the Mandelbrot algorithm
    private void RunMandelbrot()
    {
        // Iterate over each pixel in the display texture
        for (int x = 0; x != display.width; x++)
        {
            for (int y = 0; y != display.height; y++)
            {
                // Calculate color based on Mandelbrot function
                display.SetPixel(x, y,
                    SetColor(MandelbrotFunction(rStart + width * (double)x / display.width,
                                                iStart + height * (double)y / display.height)));
            }
        }

        // Apply changes to the texture
        display.Apply();

        // Create a sprite from the texture and assign it to the UI image
        image.sprite = Sprite.Create(display, new Rect(0, 0, display.width, display.height), new UnityEngine.Vector2(0.5f, 0.5f));
    }

    // The Mandelbrot algorithm to determine the number of iterations
    private int MandelbrotFunction(double r, double i)
    {
        int iterations = 0; // Count of iterations for determining escape time

        Complex z = new Complex(r, i); // Initialize complex number z

        // Iterate up to the maximum number of iterations
        for (int j = 0; j != maxIterations; j++)
        {
            // Update z based on the Mandelbrot formula
            z = z * z + new Complex(r, i);

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

    // Sets one of 16 different colors defined below for the Mandelbrot set
    Color SetColor(int value)
    {
        UnityEngine.Vector4 CalcColor = new UnityEngine.Vector4(0, 0, 0, 1f); // Initialize color variable

        // If the value is not equal to the maximum iterations
        if (value != maxIterations)
        {
            int colorNr = value % 16; // Use modulus to loop through 16 colors

            // Manually define 16 color shades from lime green to deep blue
            switch (colorNr)
            {
                case 0:
                    CalcColor[0] = 50.0f / 255.0f; // Lime Green (start of gradient)
                    CalcColor[1] = 205.0f / 255.0f;
                    CalcColor[2] = 50.0f / 255.0f;
                    break;
                case 1:
                    CalcColor[0] = 60.0f / 255.0f; // Light Lime Green
                    CalcColor[1] = 210.0f / 255.0f;
                    CalcColor[2] = 60.0f / 255.0f;
                    break;
                case 2:
                    CalcColor[0] = 70.0f / 255.0f; // Soft Lime
                    CalcColor[1] = 215.0f / 255.0f;
                    CalcColor[2] = 70.0f / 255.0f;
                    break;
                case 3:
                    CalcColor[0] = 80.0f / 255.0f; // Lime
                    CalcColor[1] = 220.0f / 255.0f;
                    CalcColor[2] = 80.0f / 255.0f;
                    break;
                case 4:
                    CalcColor[0] = 90.0f / 255.0f; // Bright Green
                    CalcColor[1] = 230.0f / 255.0f;
                    CalcColor[2] = 90.0f / 255.0f;
                    break;
                case 5:
                    CalcColor[0] = 100.0f / 255.0f; // Bright Greenish
                    CalcColor[1] = 240.0f / 255.0f;
                    CalcColor[2] = 100.0f / 255.0f;
                    break;
                case 6:
                    CalcColor[0] = 110.0f / 255.0f; // Light Green
                    CalcColor[1] = 250.0f / 255.0f;
                    CalcColor[2] = 110.0f / 255.0f;
                    break;
                case 7:
                    CalcColor[0] = 120.0f / 255.0f; // Soft Green
                    CalcColor[1] = 255.0f / 255.0f;
                    CalcColor[2] = 120.0f / 255.0f;
                    break;
                case 8:
                    CalcColor[0] = 80.0f / 255.0f; // Light Teal
                    CalcColor[1] = 255.0f / 255.0f;
                    CalcColor[2] = 150.0f / 255.0f;
                    break;
                case 9:
                    CalcColor[0] = 50.0f / 255.0f; // Soft Teal
                    CalcColor[1] = 255.0f / 255.0f;
                    CalcColor[2] = 180.0f / 255.0f;
                    break;
                case 10:
                    CalcColor[0] = 20.0f / 255.0f; // Light Cyan
                    CalcColor[1] = 220.0f / 255.0f;
                    CalcColor[2] = 255.0f / 255.0f;
                    break;
                case 11:
                    CalcColor[0] = 10.0f / 255.0f; // Cyan
                    CalcColor[1] = 180.0f / 255.0f;
                    CalcColor[2] = 255.0f / 255.0f;
                    break;
                case 12:
                    CalcColor[0] = 0.0f / 255.0f; // Light Blue
                    CalcColor[1] = 140.0f / 255.0f;
                    CalcColor[2] = 255.0f / 255.0f;
                    break;
                case 13:
                    CalcColor[0] = 0.0f / 255.0f; // Blue
                    CalcColor[1] = 100.0f / 255.0f;
                    CalcColor[2] = 255.0f / 255.0f;
                    break;
                case 14:
                    CalcColor[0] = 0.0f / 255.0f; // Dark Blue
                    CalcColor[1] = 60.0f / 255.0f;
                    CalcColor[2] = 255.0f / 255.0f;
                    break;
                case 15:
                    CalcColor[0] = 0.0f / 255.0f; // Deep Blue (end of gradient)
                    CalcColor[1] = 0.0f / 255.0f;
                    CalcColor[2] = 128.0f / 255.0f;
                    break;
            }
        }

        return CalcColor; // Return the calculated color
    }
}
