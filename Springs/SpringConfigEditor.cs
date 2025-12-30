#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleUnitySprings
{
    [CustomEditor(typeof(SpringConfig)), CanEditMultipleObjects]
    public class SpringConfigEditor : Editor
    {
        const string GraphPreviewTimeKey = "SpringConfigEditor_GraphPreviewTimeKey";

        static float GraphPreviewTime
        {
            get => SessionState.GetFloat(GraphPreviewTimeKey, 1f);
            set => SessionState.SetFloat(GraphPreviewTimeKey, value);
        }

        static GUIStyle editableLabelStyle;

        static GUIStyle EditableLabelStyle
        {
            get
            {
                editableLabelStyle ??= new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight };
                return editableLabelStyle;
            }
        }

        static readonly Vector3[] graphPoints = new Vector3[256];

        static readonly Color graphBackgroundColor = new(0.26f, 0.26f, 0.26f, 1f);
        static readonly Color graphEdgeColor = new(0.12f, 0.12f, 0.12f, 1f);
        static readonly Color graphLineColor = new(0.52f, 0.52f, 0.52f, 1f);
        static readonly Color graphGridColor = new(1f, 1f, 1f, 0.06f);
        static readonly Color graphCurveColor = new(0f, 1f, 0f, 1f);

        static readonly Dictionary<SpringConfig, Spring> springCache = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is not SpringConfig springConfig) return;

            if (serializedObject.isEditingMultipleObjects)
            {
                GUILayout.Label("Graph not available when editing multiple objects.", EditorStyles.helpBox);
                return;
            }

            if (!springCache.TryGetValue(springConfig, out Spring spring) || float.IsNaN(spring.GetVelocity()))
            {
                springCache[springConfig] = spring = new Spring(0f, 1f, springConfig);
            }

            DrawPreviewGraph(spring);
        }

        private static void DrawPreviewGraph(Spring spring)
        {
            var previewTime = GraphPreviewTime;

            var rect = GUILayoutUtility.GetRect(100f, 158f, GUILayout.ExpandWidth(true));

            var graphRect = new Rect(rect.position + new Vector2(16f, 16f), rect.size + new Vector2(-26f, -38f));

            // Graph background
            EditorGUI.DrawRect(graphRect, graphBackgroundColor);

            // Graph grid
            if (previewTime < 5f)
            {
                for (var line = 0.1f; line < previewTime; line += 0.1f)
                {
                    EditorGUI.DrawRect(new Rect(graphRect.xMin + graphRect.width * (line / previewTime), graphRect.yMin, 1f, graphRect.height), graphGridColor);
                }
            }

            if (previewTime > 1f)
            {
                for (var line = 1f; line < previewTime; line += 1f)
                {
                    EditorGUI.DrawRect(new Rect(graphRect.xMin + graphRect.width * (line / previewTime), graphRect.yMin, 1f, graphRect.height), graphGridColor);
                }
            }

            // Graph borders
            EditorGUI.DrawRect(new Rect(graphRect.xMin, graphRect.yMin, graphRect.width, 1f), graphEdgeColor);
            EditorGUI.DrawRect(new Rect(graphRect.xMin, graphRect.yMax - 2f, graphRect.width, 2f), graphEdgeColor);

            EditorGUI.DrawRect(new Rect(graphRect.xMin, graphRect.yMin, 1f, graphRect.height), graphEdgeColor);
            EditorGUI.DrawRect(new Rect(graphRect.xMax, graphRect.yMin, 1f, graphRect.height), graphEdgeColor);

            // Update spring sample points
            var samplePoints = graphPoints.Length;

            var minValue = 0f;
            var maxValue = 1f;

            var time = 0f;
            var deltaTime = 1f / (samplePoints - 1);

            spring.AddImpulse(-spring.GetVelocity());
            spring.Instant(minValue);
            spring.To(maxValue);

            for (int i = 0; i < samplePoints; i++)
            {
                var value = spring.Get();

                graphPoints[i] = new Vector3(time, value, 0f);

                minValue = Mathf.Min(minValue, value);
                maxValue = Mathf.Max(maxValue, value);

                time += deltaTime;
                spring.Tick(deltaTime * previewTime);
            }

            // Calculate sample points gui position
            var innerRect = new Rect(graphRect.position + new Vector2(1f, 1f), graphRect.size - new Vector2(2f, 4f));

            for (int i = 0; i < samplePoints; i++)
            {
                graphPoints[i] = new Vector3(
                    innerRect.xMin + innerRect.width * graphPoints[i].x,
                    innerRect.yMin + innerRect.height * Mathf.InverseLerp(maxValue, minValue, graphPoints[i].y),
                    0f
                );
            }

            var minYPos = Mathf.InverseLerp(maxValue, minValue, 0f);
            var maxYPos = Mathf.InverseLerp(maxValue, minValue, 1f);

            // Grid value min/max labels
            EditorGUI.DrawRect(new Rect(innerRect.xMin, innerRect.yMin + innerRect.height * minYPos, innerRect.width, 1f), graphLineColor);
            EditorGUI.DrawRect(new Rect(innerRect.xMin, innerRect.yMin + innerRect.height * maxYPos, innerRect.width, 1f), graphLineColor);

            EditorGUI.LabelField(new Rect(innerRect.xMin - 12f, innerRect.yMin + innerRect.height * minYPos - 8f, 12f, 16f), "0");
            EditorGUI.LabelField(new Rect(innerRect.xMin - 12f, innerRect.yMin + innerRect.height * maxYPos - 8f, 12f, 16f), "1");

            {
                // Draw graph curve
                using var handle = new Handles.DrawingScope(graphCurveColor);
                Handles.DrawAAPolyLine(2f, samplePoints, graphPoints);
            }

            EditorGUI.LabelField(new Rect(graphRect.position + new Vector2(0f, graphRect.height + 2f), new Vector2(20f, 16f)), "0s");
            EditorGUI.LabelField(new Rect(graphRect.position + new Vector2(graphRect.width - 8f, graphRect.height + 2f), new Vector2(8f, 16f)), $"s");

            using (var propertyScope = new EditorGUI.ChangeCheckScope())
            using (new LabelWidthScope(32f))
            {
                var previewTimeRect = new Rect(graphRect.position + new Vector2(graphRect.width - 77f, graphRect.height + 2f), new Vector2(70f, 16f));
                var newPreviewTime = EditorGUI.FloatField(previewTimeRect, "Time:", previewTime, EditableLabelStyle);
                if (propertyScope.changed) GraphPreviewTime = Mathf.Clamp(newPreviewTime, 0.1f, 10f);
            }
        }

        private void OnDisable()
        {
            if (target is SpringConfig springConfig)
            {
                springCache.Remove(springConfig);
            }
        }

        readonly struct LabelWidthScope : IDisposable
        {
            readonly float previousLabelWidth;

            public LabelWidthScope(float labelWidth)
            {
                previousLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = labelWidth;
            }

            public void Dispose()
            {
                EditorGUIUtility.labelWidth = previousLabelWidth;
            }
        }
    }
}
#endif