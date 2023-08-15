using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Auto-singleton for drawing debug text in the scene view.
/// </summary>
public class DebugLabel : AutoSingletonBehaviour<DebugLabel>
{


    struct Label2D
    {

        public string text;
        public Color colour;

        public Label2D(string text, Color colour)
        {
            this.text = text;
            this.colour = colour;
        }


    }

    struct Label3D
    {

        public Vector3 position;
        public string text;
        public Color colour;

        public Label3D(Vector3 position, string text, Color colour)
        {
            this.position = position;
            this.text = text;
            this.colour = colour;
        }


    }

    List<Label3D> _3Dlabels = new List<Label3D>();
    List<Label3D> _permanent3DLabels = new List<Label3D>();
    List<Label2D> _2Dlabels = new List<Label2D>();
    List<Label2D> _permanent2DLabels = new List<Label2D>();

    private float _currentY = 0;



#if UNITY_EDITOR
    void OnDrawGizmos()
    {

        Handles.BeginGUI();
        _currentY = 8;
        for (int i = 0; i < _permanent2DLabels.Count; i++)
        {
            Draw2DLabel(_permanent2DLabels[i]);
        }
        for (int i = 0; i < _2Dlabels.Count; i++)
        {
            Draw2DLabel(_2Dlabels[i]);
        }
        Handles.EndGUI();

        for (int i = 0; i < _3Dlabels.Count; i++)
        {
            Draw3DLabel(_3Dlabels[i]);
        }
        for (int i = 0; i < _permanent3DLabels.Count; i++)
        {
            Draw3DLabel(_permanent3DLabels[i]);
        }
        _3Dlabels.Clear();
        _2Dlabels.Clear();
        GUI.contentColor = Color.white;
    }

    void Draw3DLabel(Label3D label)
    {
        GUI.contentColor = label.colour;
        Handles.Label(label.position, label.text);

    }

    void Draw2DLabel(Label2D label)
    {

        GUI.contentColor = label.colour;
        Rect rect = new Rect(10, _currentY, Screen.width, Screen.height);
        GUI.Label(rect, label.text);
        _currentY += 16;
    }

#endif

    #region 2d statics

    /// <summary>
    /// Draws text in the corner of the scene view.
    /// </summary>
    /// <param name="text">The text to draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(string text)
    {
        Instance._2Dlabels.Add(new Label2D(text, Color.white));
    }
    /// <summary>
    /// Converts an object to a string and draws the text in the corner of the scene view.
    /// </summary>
    /// <param name="o">The object to convert and then draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(object o)
    {
        Instance._2Dlabels.Add(new Label2D(o.ToString(), Color.white));
    }
    /// <summary>
    /// Converts an object to a string and draws the text in the corner of the scene view every frame.
    /// </summary>
    /// <param name="o">The object to convert and then draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(object o)
    {
        Instance._permanent2DLabels.Add(new Label2D(o.ToString(), Color.white));
    }
    /// <summary>
    /// Draws text in the corrner of the scene view every frame.
    /// </summary>
    /// <param name="text">The text to draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(string text)
    {
        Instance._permanent2DLabels.Add(new Label2D(text, Color.white));
    }
    /// <summary>
    /// Draws text in the corner of the scene view.
    /// </summary>
    /// <param name="text">The text to draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(string text, Color colour)
    {
        Instance._2Dlabels.Add(new Label2D(text, colour));
    }
    /// <summary>
    /// Converts an object to a string and draws the text in the corner of the scene view.
    /// </summary>
    /// <param name="o">The object to convert and then draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(object o, Color colour)
    {
        Instance._2Dlabels.Add(new Label2D(o.ToString(), colour));
    }
    /// <summary>
    /// Converts an object to a string and draws the text in the corner of the scene view every frame.
    /// </summary>
    /// <param name="o">The object to convert and then draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(object o, Color colour)
    {
        Instance._permanent2DLabels.Add(new Label2D(o.ToString(), colour));
    }
    /// <summary>
    /// Draws text in the corrner of the scene view every frame.
    /// </summary>
    /// <param name="text">The text to draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(string text, Color colour)
    {
        Instance._permanent2DLabels.Add(new Label2D(text, colour));
    }

    #endregion

    #region 3d statics

    /// <summary>
    /// Draws camera facing text in the scene view at a position in the scene.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="text">The text to draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(Vector3 position, string text)
    {
        Instance._3Dlabels.Add(new Label3D(position, text, Color.white));
    }
    /// <summary>
    /// Converts an object to a string and then draws camera facing text in the scene view at a position in the scene.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="o">The object to convert and then draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(Vector3 position, object o)
    {
        Instance._3Dlabels.Add(new Label3D(position, o.ToString(), Color.white));
    }
    /// <summary>
    /// Converts an object to a string and then draws camera facing text in the scene view at a position in the scene every frame.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="o">The object to convert and then draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(Vector3 position, object o)
    {
        Instance._permanent3DLabels.Add(new Label3D(position, o.ToString(), Color.white));
    }
    /// <summary>
    /// Draws camera facing text in the scene view at a position in the scene every frame.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="text">The text to draw</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(Vector3 position, string text)
    {
        Instance._permanent3DLabels.Add(new Label3D(position, text, Color.white));
    }
    /// <summary>
    /// Draws camera facing text in the scene view at a position in the scene.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="text">The text to draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(Vector3 position, string text, Color colour)
    {
        Instance._3Dlabels.Add(new Label3D(position, text, colour));
    }
    /// <summary>
    /// Converts an object to a string and then draws camera facing text in the scene view at a position in the scene.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="o">The object to convert and then draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Draw(Vector3 position, object o, Color colour)
    {
        Instance._3Dlabels.Add(new Label3D(position, o.ToString(), colour));
    }
    /// <summary>
    /// Converts an object to a string and then draws camera facing text in the scene view at a position in the scene every frame.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="o">The object to convert and then draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(Vector3 position, object o, Color colour)
    {
        Instance._permanent3DLabels.Add(new Label3D(position, o.ToString(), colour));
    }
    /// <summary>
    /// Draws camera facing text in the scene view at a position in the scene every frame.
    /// </summary>
    /// <param name="position">The world-space position to draw the label at</param>
    /// <param name="text">The text to draw</param>
    /// <param name="colour">The colour to draw with</param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawForever(Vector3 position, string text, Color colour)
    {
        Instance._permanent3DLabels.Add(new Label3D(position, text, colour));
    }

    #endregion
}
