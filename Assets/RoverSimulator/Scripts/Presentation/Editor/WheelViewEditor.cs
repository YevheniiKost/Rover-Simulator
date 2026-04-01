using UnityEditor;
using UnityEngine;

namespace RoverSimulator.Presentation.Rover
{
    [CustomEditor(typeof(WheelView))]
    public class WheelViewEditor : Editor
    {
        private const float GraphHeight = 130f;
        private const float GraphPaddingLeft = 44f;
        private const float GraphPaddingBottom = 20f;
        private const float GraphPaddingTop = 8f;
        private const float GraphPaddingRight = 8f;
        private const int GraphSampleCount = 128;
        private const int GridHorizontalDivisions = 4;
        private const int GridVerticalDivisions = 5;
        private const float SlipAxisExtensionFactor = 1.2f;
        private const float MinSafeAxisValue = 0.0001f;
        private const float CurveLineWidth = 2f;
        private const float LegendSwatchSize = 10f;
        private const float LegendRowHeight = 16f;

        private static readonly Color s_forwardFrictionColor = new Color(1f, 0.55f, 0f);
        private static readonly Color s_sidewaysFrictionColor = new Color(0.3f, 0.75f, 1f);
        private static readonly Color s_graphBackgroundColor = new Color(0.12f, 0.12f, 0.12f);
        private static readonly Color s_gridLineColor = new Color(0.28f, 0.28f, 0.28f);
        private static readonly Color s_borderColor = new Color(0.45f, 0.45f, 0.45f);

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SerializedProperty wheelColliderProperty = serializedObject.FindProperty("_wheelCollider");

            if (wheelColliderProperty == null || wheelColliderProperty.objectReferenceValue == null)
            {
                return;
            }

            WheelCollider wheelCollider = (WheelCollider)wheelColliderProperty.objectReferenceValue;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Friction Curves (Force / Slip)", EditorStyles.boldLabel);

            WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
            WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;

            float maxForce = ComputeMaxForce(forwardFriction, sidewaysFriction);
            float maxSlip = ComputeMaxSlip(forwardFriction, sidewaysFriction);

            if (maxForce < MinSafeAxisValue || maxSlip < MinSafeAxisValue)
            {
                EditorGUILayout.HelpBox(
                    "WheelCollider friction curves have no valid range to display.",
                    MessageType.Warning
                );
                return;
            }

            Rect totalRect = GUILayoutUtility.GetRect(
                GUIContent.none,
                GUIStyle.none,
                GUILayout.Height(GraphHeight),
                GUILayout.ExpandWidth(true)
            );

            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Rect graphRect = new Rect(
                totalRect.xMin + GraphPaddingLeft,
                totalRect.yMin + GraphPaddingTop,
                totalRect.width - GraphPaddingLeft - GraphPaddingRight,
                totalRect.height - GraphPaddingTop - GraphPaddingBottom
            );

            DrawGraphBackground(graphRect);
            DrawGridLines(graphRect);
            DrawFrictionCurve(graphRect, forwardFriction, maxSlip, maxForce, s_forwardFrictionColor);
            DrawFrictionCurve(graphRect, sidewaysFriction, maxSlip, maxForce, s_sidewaysFrictionColor);
            DrawGraphBorder(graphRect);
            DrawAxisLabels(totalRect, graphRect, maxSlip, maxForce);
            DrawLegend(graphRect);
        }

        private static float ComputeMaxForce(WheelFrictionCurve forward, WheelFrictionCurve sideways)
        {
            float forwardPeak = forward.extremumValue * forward.stiffness;
            float forwardAsymptote = forward.asymptoteValue * forward.stiffness;
            float sidewaysPeak = sideways.extremumValue * sideways.stiffness;
            float sidewaysAsymptote = sideways.asymptoteValue * sideways.stiffness;

            return Mathf.Max(forwardPeak, forwardAsymptote, sidewaysPeak, sidewaysAsymptote);
        }

        private static float ComputeMaxSlip(WheelFrictionCurve forward, WheelFrictionCurve sideways)
        {
            float maxAsymptoteSlip = Mathf.Max(forward.asymptoteSlip, sideways.asymptoteSlip);
            return maxAsymptoteSlip * SlipAxisExtensionFactor;
        }

        private static void DrawGraphBackground(Rect graphRect)
        {
            EditorGUI.DrawRect(graphRect, s_graphBackgroundColor);
        }

        private static void DrawGraphBorder(Rect graphRect)
        {
            Handles.color = s_borderColor;

            Vector3 topLeft = new Vector3(graphRect.xMin, graphRect.yMin, 0f);
            Vector3 topRight = new Vector3(graphRect.xMax, graphRect.yMin, 0f);
            Vector3 bottomRight = new Vector3(graphRect.xMax, graphRect.yMax, 0f);
            Vector3 bottomLeft = new Vector3(graphRect.xMin, graphRect.yMax, 0f);

            Handles.DrawLine(topLeft, topRight);
            Handles.DrawLine(topRight, bottomRight);
            Handles.DrawLine(bottomRight, bottomLeft);
            Handles.DrawLine(bottomLeft, topLeft);
        }

        private static void DrawGridLines(Rect graphRect)
        {
            Handles.color = s_gridLineColor;

            for (int row = 0; row <= GridHorizontalDivisions; row++)
            {
                float t = (float)row / GridHorizontalDivisions;
                float y = graphRect.yMax - t * graphRect.height;

                Handles.DrawLine(
                    new Vector3(graphRect.xMin, y, 0f),
                    new Vector3(graphRect.xMax, y, 0f)
                );
            }

            for (int col = 0; col <= GridVerticalDivisions; col++)
            {
                float t = (float)col / GridVerticalDivisions;
                float x = graphRect.xMin + t * graphRect.width;

                Handles.DrawLine(
                    new Vector3(x, graphRect.yMin, 0f),
                    new Vector3(x, graphRect.yMax, 0f)
                );
            }
        }

        private static void DrawFrictionCurve(
            Rect graphRect,
            WheelFrictionCurve friction,
            float maxSlip,
            float maxForce,
            Color color)
        {
            Handles.color = color;

            Vector3[] points = new Vector3[GraphSampleCount];

            for (int sampleIndex = 0; sampleIndex < GraphSampleCount; sampleIndex++)
            {
                float t = (float)sampleIndex / (GraphSampleCount - 1);
                float slip = t * maxSlip;
                float force = EvaluateFrictionCurve(friction, slip);

                float x = graphRect.xMin + t * graphRect.width;
                float y = graphRect.yMax - (force / maxForce) * graphRect.height;

                points[sampleIndex] = new Vector3(x, y, 0f);
            }

            Handles.DrawAAPolyLine(CurveLineWidth, points);
        }

        private static float EvaluateFrictionCurve(WheelFrictionCurve curve, float slip)
        {
            if (slip <= 0f)
            {
                return 0f;
            }

            float scaledExtremumValue = curve.extremumValue * curve.stiffness;
            float scaledAsymptoteValue = curve.asymptoteValue * curve.stiffness;

            if (curve.extremumSlip > 0f && slip <= curve.extremumSlip)
            {
                return (slip / curve.extremumSlip) * scaledExtremumValue;
            }

            float slipRangeLength = curve.asymptoteSlip - curve.extremumSlip;

            if (slipRangeLength > 0f && slip <= curve.asymptoteSlip)
            {
                float t = (slip - curve.extremumSlip) / slipRangeLength;
                return Mathf.Lerp(scaledExtremumValue, scaledAsymptoteValue, t);
            }

            return scaledAsymptoteValue;
        }

        private static void DrawAxisLabels(Rect totalRect, Rect graphRect, float maxSlip, float maxForce)
        {
            GUIStyle forceValueStyle = new GUIStyle(EditorStyles.miniLabel);
            forceValueStyle.normal.textColor = new Color(0.65f, 0.65f, 0.65f);
            forceValueStyle.alignment = TextAnchor.MiddleRight;

            for (int row = 0; row <= GridHorizontalDivisions; row++)
            {
                float t = (float)row / GridHorizontalDivisions;
                float forceValue = t * maxForce;
                float y = graphRect.yMax - t * graphRect.height;

                Rect labelRect = new Rect(totalRect.xMin, y - 7f, GraphPaddingLeft - 4f, 14f);
                GUI.Label(labelRect, forceValue.ToString("F1"), forceValueStyle);
            }

            GUIStyle slipValueStyle = new GUIStyle(EditorStyles.miniLabel);
            slipValueStyle.normal.textColor = new Color(0.65f, 0.65f, 0.65f);
            slipValueStyle.alignment = TextAnchor.UpperCenter;

            for (int col = 0; col <= GridVerticalDivisions; col++)
            {
                float t = (float)col / GridVerticalDivisions;
                float slipValue = t * maxSlip;
                float x = graphRect.xMin + t * graphRect.width;

                Rect labelRect = new Rect(x - 20f, graphRect.yMax + 2f, 40f, 14f);
                GUI.Label(labelRect, slipValue.ToString("F2"), slipValueStyle);
            }
        }

        private static void DrawLegend(Rect graphRect)
        {
            GUIStyle legendLabelStyle = new GUIStyle(EditorStyles.miniLabel);

            float legendX = graphRect.xMax - 76f;
            float legendY = graphRect.yMin + 5f;

            EditorGUI.DrawRect(
                new Rect(legendX, legendY, LegendSwatchSize, LegendSwatchSize),
                s_forwardFrictionColor
            );
            legendLabelStyle.normal.textColor = s_forwardFrictionColor;
            GUI.Label(
                new Rect(legendX + LegendSwatchSize + 3f, legendY - 1f, 62f, 14f),
                "Forward",
                legendLabelStyle
            );

            legendY += LegendRowHeight;

            EditorGUI.DrawRect(
                new Rect(legendX, legendY, LegendSwatchSize, LegendSwatchSize),
                s_sidewaysFrictionColor
            );
            legendLabelStyle.normal.textColor = s_sidewaysFrictionColor;
            GUI.Label(
                new Rect(legendX + LegendSwatchSize + 3f, legendY - 1f, 62f, 14f),
                "Sideways",
                legendLabelStyle
            );
        }
    }
}

