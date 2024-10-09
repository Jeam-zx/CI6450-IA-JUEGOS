using UnityEngine;
using UnityEngine.UI;

public class TextHandler
{
    private GameObject textObject;
    private readonly Transform parentTransform;

    /// <summary>
    /// Initializes a new instance of the TextHandler class.
    /// </summary>
    /// <param name="parent">The parent transform to attach the text object to.</param>
    /// <param name="text">The text to display.</param>
    public TextHandler(Transform parent, string text)
    {
        parentTransform = parent;
        CreateTextObject(text);
    }

    /// <summary>
    /// Creates a text object to display the specified text.
    /// </summary>
    /// <param name="text">The text to display.</param>
    private void CreateTextObject(string text)
    {
        // Create a canvas to hold the text object
        GameObject canvas = new("Canvas");
        canvas.transform.parent = parentTransform; // Make the canvas a child of the parent transform
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        Canvas canvasComponent = canvas.GetComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.WorldSpace; // Set to WorldSpace to position in the world
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(0.1f, 0.1f); // Adjust the size of the canvas

        // Create the text object
        textObject = new GameObject("Text");
        textObject.transform.parent = canvas.transform;
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.text = text;
        textComponent.fontSize = 12;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.color = Color.white;

        // Adjust the RectTransform of the text object
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0.1f, 0); // Position the text above the parent object
        rectTransform.sizeDelta = new Vector2(100, 20); // Adjust the size of the text rectangle
        rectTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f); // Adjust the scale to be visible in the world
    }

    /// <summary>
    /// Updates the position of the text object to follow the parent transform.
    /// </summary>
    public void UpdateTextPosition()
    {
        if (textObject != null)
        {
            Vector3 offset = parentTransform.up * 0.6f; // Adjust the height of the text above the parent object
            textObject.transform.SetPositionAndRotation(parentTransform.position + offset, parentTransform.rotation);
        }
    }
}