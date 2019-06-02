using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.Display
{
    [RequireComponent(typeof(Canvas))]
    [ExecuteInEditMode]
    [AddComponentMenu("Layout/Dynamic Physical Canvas Scaler", 101)]
    public class DynamicPhysicalCanvasScaler : UIBehaviour
    {
        [Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
        [SerializeField]
        protected float m_ReferencePixelsPerUnit = 100;
        public float referencePixelsPerUnit { get { return m_ReferencePixelsPerUnit; } set { m_ReferencePixelsPerUnit = value; } }
        

        [Tooltip("The physical units the UI layout is designed for. If the screen size is larger, the UI will be scaled up, and if it's smaller, the UI will be scaled down.")]
        [SerializeField]
        protected Vector2 m_ReferencePhysicalSize = new Vector2(960, 540);
        public Vector2 referencePhysicalSize { get { return m_ReferencePhysicalSize; } set { m_ReferencePhysicalSize = value; } }
        [SerializeField]
        protected float m_MinScale = 1;
        [SerializeField]
        protected float m_MaxScale = int.MaxValue;

        [SerializeField]
        protected float m_MinScalingModifier = 1;
        [SerializeField]
        protected float m_MaxScalingModifier = 1;

        [Tooltip("Determines if the scaling is using the width or height as reference, or a mix in between.")]
        [Range(0, 1)]
        [SerializeField]
        protected float m_MatchWidthOrHeight = 0;
        public float matchWidthOrHeight { get { return m_MatchWidthOrHeight; } set { m_MatchWidthOrHeight = value; } }


        // The log base doesn't have any influence on the results whatsoever, as long as the same base is used everywhere.
        private const float kLogBase = 2;


        // Constant Physical Size settings

        public enum Unit { Centimeters, Millimeters, Inches, Points, Picas }

        [Tooltip("The physical unit to specify positions and sizes in.")]
        [SerializeField]
        protected Unit m_PhysicalUnit = Unit.Points;
        public Unit physicalUnit { get { return m_PhysicalUnit; } set { m_PhysicalUnit = value; } }

        [Tooltip("The DPI to assume if the screen DPI is not known.")]
        [SerializeField]
        protected float m_FallbackScreenDPI = 96;
        public float fallbackScreenDPI { get { return m_FallbackScreenDPI; } set { m_FallbackScreenDPI = value; } }

        [Tooltip("The pixels per inch to use for sprites that have a 'Pixels Per Unit' setting that matches the 'Reference Pixels Per Unit' setting.")]
        [SerializeField]
        protected float m_DefaultSpriteDPI = 96;
        public float defaultSpriteDPI { get { return m_DefaultSpriteDPI; } set { m_DefaultSpriteDPI = value; } }


        // World Canvas settings

        [Tooltip("The amount of pixels per unit to use for dynamically created bitmaps in the UI, such as Text.")]
        [SerializeField]
        protected float m_DynamicPixelsPerUnit = 1;
        public float dynamicPixelsPerUnit { get { return m_DynamicPixelsPerUnit; } set { m_DynamicPixelsPerUnit = value; } }


        // General variables

        private Canvas m_Canvas;
        [System.NonSerialized]
        private float m_PrevScaleFactor = 1;
        [System.NonSerialized]
        private float m_PrevReferencePixelsPerUnit = 100;

        [System.NonSerialized]
        private float m_PrevWidthBasedScaling = -1;
        [System.NonSerialized]
        private float m_LogWidthScaling = 1;

        [System.NonSerialized]
        private float m_PrevHeightBasedScaling = -1;
        [System.NonSerialized]
        private float m_LogHeightScaling = 1;

        [System.NonSerialized]
        private float m_PrevMatchWidthOrHeight = -1;
        [System.NonSerialized]
        private float m_DynamicPhysicalScaling = 1;


        protected DynamicPhysicalCanvasScaler() { }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Canvas = GetComponent<Canvas>();
            Handle();
        }

        protected override void OnDisable()
        {
            SetScaleFactor(1);
            SetReferencePixelsPerUnit(100);
            base.OnDisable();
        }

        protected virtual void Update()
        {
            Handle();
        }

        protected virtual void Handle()
        {
            if (m_Canvas == null || !m_Canvas.isRootCanvas)
                return;

            if (m_Canvas.renderMode == RenderMode.WorldSpace)
            {
                HandleWorldCanvas();
                return;
            }

            HandlePhysicalDataBasedScaling();
        }

        protected virtual void HandleWorldCanvas()
        {
            SetScaleFactor(m_DynamicPixelsPerUnit);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
        }

        protected virtual void HandlePhysicalDataBasedScaling()
        {
            float currentDpi = Screen.dpi;
            float dpi = (currentDpi <= 0 ? m_FallbackScreenDPI : currentDpi);
            float targetDpiUnitMultiplier = 1;
            switch (m_PhysicalUnit)
            {
                case Unit.Centimeters: targetDpiUnitMultiplier = 2.54f; break;
                case Unit.Millimeters: targetDpiUnitMultiplier = 25.4f; break;
                case Unit.Inches: targetDpiUnitMultiplier = 1; break;
                case Unit.Points: targetDpiUnitMultiplier = 72; break;
                case Unit.Picas: targetDpiUnitMultiplier = 6; break;
            }

            float dpiScale = dpi / targetDpiUnitMultiplier;

            // We take the log of the relative width and height before taking the average.
            // Then we transform it back in the original space.
            // the reason to transform in and out of logarithmic space is to have better behavior.
            // If one axis has twice resolution and the other has half, it should even out if widthOrHeight value is at 0.5.
            // In normal space the average would be (0.5 + 2) / 2 = 1.25
            // In logarithmic space the average is (-1 + 1) / 2 = 0


            float widthBasedScaling = ((Screen.width / dpi) * targetDpiUnitMultiplier) / m_ReferencePhysicalSize.x;
            float heightBasedScaling = ((Screen.height / dpi) * targetDpiUnitMultiplier) / m_ReferencePhysicalSize.y;
            bool dimensionsChanged = widthBasedScaling != m_PrevWidthBasedScaling || heightBasedScaling != m_PrevHeightBasedScaling;
            if (dimensionsChanged)
            {
                m_PrevWidthBasedScaling = widthBasedScaling;
                m_PrevHeightBasedScaling = heightBasedScaling;
                m_LogWidthScaling = Mathf.Log(widthBasedScaling, kLogBase);
                m_LogHeightScaling = Mathf.Log(heightBasedScaling, kLogBase);
            }

            if(dimensionsChanged || m_MatchWidthOrHeight != m_PrevMatchWidthOrHeight)
            {
                m_PrevMatchWidthOrHeight = m_MatchWidthOrHeight;
                float logWeightedAverage = Mathf.Lerp(m_LogWidthScaling, m_LogHeightScaling, m_MatchWidthOrHeight);
                m_DynamicPhysicalScaling = Mathf.Pow(kLogBase, logWeightedAverage);
            }

            float scalingAdjustedByModifier = 1;
            if (m_DynamicPhysicalScaling < 1)
            {
                scalingAdjustedByModifier = 1 - ((1 - m_DynamicPhysicalScaling) * m_MinScalingModifier);
                scalingAdjustedByModifier = Mathf.Max(scalingAdjustedByModifier, 0.001f);
            }
            else
            {
                scalingAdjustedByModifier = 1 + ((m_DynamicPhysicalScaling - 1) * m_MaxScalingModifier);
            }

            float clampedPhysicalScaleFactor = Mathf.Min(scalingAdjustedByModifier, m_MaxScale);
            clampedPhysicalScaleFactor = Mathf.Max(clampedPhysicalScaleFactor, m_MinScale);

            SetScaleFactor(clampedPhysicalScaleFactor * dpiScale);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit * targetDpiUnitMultiplier / m_DefaultSpriteDPI);
        }

        protected void SetScaleFactor(float scaleFactor)
        {
            if (scaleFactor == m_PrevScaleFactor)
                return;

            m_Canvas.scaleFactor = scaleFactor;
            m_PrevScaleFactor = scaleFactor;
        }

        protected void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
        {
            if (referencePixelsPerUnit == m_PrevReferencePixelsPerUnit)
                return;

            m_Canvas.referencePixelsPerUnit = referencePixelsPerUnit;
            m_PrevReferencePixelsPerUnit = referencePixelsPerUnit;
        }
    }
}
