using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RapidPrototypingKit
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BevelledCube))]
    public class BevelledCubeEditor : Editor
    {
        private enum ActiveTool
        {
            None,
            Rect,
            Scale,
            Pivot,
            Bevel
        }

        static private ActiveTool _tool;
        private Tool _cachedTool;

        private Rect rect = new Rect(Vector2.zero, Vector2.one);

        private Vector3 pos = Vector3.zero;
        private Vector3 size = Vector3.one;

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                BevelledCube cube = (target as BevelledCube);
                cube.Generate();
            }

            if (GUILayout.Button("Normalize"))
            {
                BevelledCube cube = (target as BevelledCube);

                Undo.RecordObject(target, "Normalize");
                Undo.RecordObject(cube.transform, "Normalize");
                cube.pivot = Vector3.Scale(cube.pivot, cube.transform.localScale);
                cube.size = Vector3.Scale(cube.size, cube.transform.localScale);

                float scale = Mathf.Min(cube.transform.localScale.x, cube.transform.localScale.y,
                    cube.transform.localScale.z);

                cube.bevel *= scale;
                
                cube.transform.localScale = Vector3.one;
                cube.Generate();
            }
        }

        public void OnSceneGUI()
        {
            BevelledCube cube = (target as BevelledCube);

            if (Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.S:
                        RestoreCurrentTool();
                        CacheCurrentTool();
                        _tool = ActiveTool.Pivot;
                        break;
                    case KeyCode.D:
                        RestoreCurrentTool();
                        CacheCurrentTool();
                        _tool = ActiveTool.Scale;
                        break;
                    case KeyCode.A:
                        RestoreCurrentTool();
                        CacheCurrentTool();
                        _tool = ActiveTool.Rect;
                        break;
                    case KeyCode.C:
                        RestoreCurrentTool();
                        CacheCurrentTool();
                        _tool = ActiveTool.Bevel;
                        break;
                    case KeyCode.Q:
                    case KeyCode.W:
                    case KeyCode.E:
                    case KeyCode.R:
                        _tool = ActiveTool.None;
                        // RestoreCurrentTool();
                        break;
                }
            }

            if (_tool == ActiveTool.Scale)
            {
                Handles.matrix = cube.transform.localToWorldMatrix;

                EditorGUI.BeginChangeCheck();
                Handles.color = Color.red;
                Vector3 scale = Handles.ScaleHandle(cube.size, cube.pivot, Quaternion.identity, 1);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Scaled Scale At Point");
                    cube.size = scale;
                    cube.Generate();
                }
            }

            Color rectToolColor = Color.Lerp(Color.cyan, Color.blue, 0.5f);
            Handles.matrix = cube.transform.localToWorldMatrix;
            Handles.color = rectToolColor;
            Handles.DrawWireCube(cube.pivot, cube.size);

            // ----- Rect Tool -----
            if (_tool == ActiveTool.Rect)
            {
                Handles.matrix = cube.transform.localToWorldMatrix;

                EditorGUI.BeginChangeCheck();

                // var positionAndSize = ResizeCube(cube.pivot, cube.size/2, rectToolColor, 0);
                var positionAndSize = ResizeRect(
                    cube.pivot, cube.size,
                    Handles.CubeHandleCap,
                    rectToolColor,
                    Color.clear,
                    HandleUtility.GetHandleSize(Vector3.zero) * .1f,
                    .1f);

                // cube.pivot = newRect.newPosition;
                // cube.size = newRect.size;

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Pivot At Point");
                    cube.pivot = positionAndSize.newPosition;
                    cube.size = positionAndSize.size;
                    cube.Generate();
                }
            }

            // ----- Bevel Tool -----

            if (_tool == ActiveTool.Bevel)
            {
                Handles.matrix = cube.transform.localToWorldMatrix;

                EditorGUI.BeginChangeCheck();
                // Handles.SphereHandleCap(0, cube.pivot, Quaternion.identity, 0.1f, EventType.Repaint);
                float bevel = Handles.ScaleSlider(1f / cube.bevel, cube.pivot, Vector3.up, Quaternion.identity,
                    HandleUtility.GetHandleSize(Vector3.zero) * 1, 0.1f);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Updated Bevel");
                    cube.bevel = 1f / bevel;
                    cube.Generate();
                }
            }

            // ------ Pivot Tool -----
            if (_tool == ActiveTool.Pivot)
            {
                Handles.matrix = cube.transform.localToWorldMatrix;

                EditorGUI.BeginChangeCheck();

                Vector3 pivot = Handles.PositionHandle(cube.pivot, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Updated Pivot");
                    cube.pivot = pivot;
                    cube.Generate();
                }
            }

            // ----- Pivot ----
            Handles.matrix = Matrix4x4.Translate(cube.transform.position);
            Handles.color = Color.white;
            Handles.SphereHandleCap(0, Vector3.zero, Quaternion.identity, 0.1f, EventType.Repaint);


        }

        private void RestoreCurrentTool()
        {
            // Tools.current = _cachedTool;
        }

        private void CacheCurrentTool()
        {
            Tools.current = Tool.None;
            SceneView.RepaintAll();
        }

        public static (Vector3 position, Vector3 size) ResizeCube(Vector3 position, Vector3 size, Color capCol,
            float snap)
        {
            Vector3 halfRectSize = size * 0.5f;

            float capSize = HandleUtility.GetHandleSize(Vector3.zero) * .1f;

            Vector3[] handlePoints =
            {
                new Vector3(position.x - halfRectSize.x, position.y, position.z), // Left
                new Vector3(position.x + halfRectSize.x, position.y, position.z), // Right
                new Vector3(position.x, position.y + halfRectSize.y, position.z), // Top
                new Vector3(position.x, position.y - halfRectSize.y, position.z), // Bottom 
                new Vector3(position.x, position.y, position.z - halfRectSize.z), // Front 
                new Vector3(position.x, position.y, position.z + halfRectSize.z) // Back 
            };

            Handles.color = capCol;

            var newSize = size;
            var newPosition = position;

            Handles.CapFunction capFunc = Handles.CubeHandleCap;
            var leftHandle = Handles.Slider(handlePoints[0], -Vector3.right, capSize, capFunc, snap).x -
                             handlePoints[0].x;
            var rightHandle = Handles.Slider(handlePoints[1], Vector3.right, capSize, capFunc, snap).x -
                              handlePoints[1].x;
            var topHandle = Handles.Slider(handlePoints[2], Vector3.up, capSize, capFunc, snap).y - handlePoints[2].y;
            var bottomHandle = Handles.Slider(handlePoints[3], -Vector3.up, capSize, capFunc, snap).y -
                               handlePoints[3].y;
            // var backHandle =     Handles.Slider(handlePoints[4], Vector3.forward, capSize, capFunc, snap).z - handlePoints[4].z;
            // var frontHandle =  Handles.Slider(handlePoints[5], -Vector3.forward, capSize, capFunc, snap).z - handlePoints[5].z;


            newSize = new Vector3(
                Mathf.Max(.01f, newSize.x - leftHandle + rightHandle),
                Mathf.Max(.01f, newSize.y + topHandle - bottomHandle), newSize.z);
            // Mathf.Max(.01f, newSize.z + frontHandle - backHandle));
            //
            newPosition = new Vector3(
                newPosition.x + leftHandle * 0.5f + rightHandle * 0.5f,
                newPosition.y + topHandle * 0.5f + bottomHandle * 0.5f,
                newPosition.z);

            return (newPosition, newSize);
        }

        public static (Vector3 newPosition, Vector3 size) ResizeRect(Vector3 position, Vector3 size,
            Handles.CapFunction capFunc, Color capCol, Color fillCol, float capSize, float snap)
        {
            Vector3 halfRectSize = new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);

            Vector3[] rectangleCorners =
            {
                new Vector3(position.x - halfRectSize.x, position.y - halfRectSize.y,
                    position.z - halfRectSize.z), // Bottom Left
                new Vector3(position.x + halfRectSize.x, position.y - halfRectSize.y,
                    position.z - halfRectSize.z), // Bottom Right
                new Vector3(position.x + halfRectSize.x, position.y + halfRectSize.y,
                    position.z + halfRectSize.z), // Top Right
                new Vector3(position.x - halfRectSize.x, position.y + halfRectSize.y,
                    position.z + halfRectSize.z) // Top Left
            };

            Handles.color = fillCol;
            Handles.DrawSolidRectangleWithOutline(rectangleCorners, new Color(fillCol.r, fillCol.g, fillCol.b, 0.25f),
                capCol);

            Vector3[] handlePoints =
            {
                new Vector3(position.x - halfRectSize.x, position.y, position.z), // Left
                new Vector3(position.x + halfRectSize.x, position.y, position.z), // Right
                new Vector3(position.x, position.y + halfRectSize.y, position.z), // Top
                new Vector3(position.x, position.y - halfRectSize.y, position.z), // Bottom 
                new Vector3(position.x, position.y, position.z + halfRectSize.z), // Top
                new Vector3(position.x, position.y, position.z - halfRectSize.z) // Bottom 
            };

            Handles.color = capCol;

            var newSize = size;
            var newPosition = position;

            var leftHandle = Handles.Slider(handlePoints[0], -Vector3.right, capSize, capFunc, snap).x -
                             handlePoints[0].x;
            var rightHandle = Handles.Slider(handlePoints[1], Vector3.right, capSize, capFunc, snap).x -
                              handlePoints[1].x;
            var topHandle = Handles.Slider(handlePoints[2], Vector3.up, capSize, capFunc, snap).y - handlePoints[2].y;
            var bottomHandle = Handles.Slider(handlePoints[3], -Vector3.up, capSize, capFunc, snap).y -
                               handlePoints[3].y;
            var frontHandle = Handles.Slider(handlePoints[4], Vector3.forward, capSize, capFunc, snap).z -
                              handlePoints[4].z;
            var backHandle = Handles.Slider(handlePoints[5], -Vector3.forward, capSize, capFunc, snap).z -
                             handlePoints[5].z;
            newSize = new Vector3(
                Mathf.Max(.1f, newSize.x - leftHandle + rightHandle),
                Mathf.Max(.1f, newSize.y + topHandle - bottomHandle),
                Mathf.Max(1f, newSize.z + frontHandle - backHandle)
            );

            newPosition = new Vector3(
                newPosition.x + leftHandle * .5f + rightHandle * .5f,
                newPosition.y + topHandle * .5f + bottomHandle * .5f,
                newPosition.z + frontHandle * .5f + backHandle * .5f
            );

            return (newPosition, newSize);
        }
    }
}