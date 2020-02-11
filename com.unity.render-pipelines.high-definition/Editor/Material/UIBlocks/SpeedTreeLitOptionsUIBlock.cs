using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Linq;

// Include material common properties names
using static UnityEngine.Rendering.HighDefinition.HDMaterialProperties;

namespace UnityEditor.Rendering.HighDefinition
{
    class SpeedTreeLitOptionsUIBlock : MaterialUIBlock
    {
        public class Styles
        {
            public const string SpeedTreeHeader = "SpeedTree Options";

            public static GUIContent typeText = new GUIContent("Geometry Type", "Which type of tree geometry component is the part being shaded");
            public static GUIContent enableWindText = new GUIContent("Enable Wind", "Whether you want the wind effect to be on or not");
            public static GUIContent windQualityText = new GUIContent("Wind Quality", "Detail level of the wind effect");
            public static GUIContent billboardText = new GUIContent("Billboard", "This surface is a billboard");
            public static GUIContent billboardFacingText = new GUIContent("Billboard Camera Facing", "Factor which affects billboard's impact on shadows");
        }

        Expandable m_ExpandableBit;

        public enum SpeedTreeVersionEnum
        {
            SpeedTreeVer7 = 0,
            SpeedTreeVer8,
        }

        public enum SpeedTree7Geom
        {
            Branch,
            BranchDetail,
            Frond,
            Leaf,
            Mesh,
        }

        public enum WindQualityEnum
        {
            None,
            Fastest,
            Fast,
            Better,
            Best,
            Palm,
        }

        public SpeedTreeVersionEnum mAssetVersion = SpeedTreeVersionEnum.SpeedTreeVer7;
        public SpeedTree7Geom mGeomType = SpeedTree7Geom.Branch;
        public bool mWindEnable = true;
        public WindQualityEnum mWindQuality = WindQualityEnum.Best;
        public bool mBillboard = false;
        public bool mBillboardFacing = false;

        MaterialProperty geomType = null;
        public const string kGeomType = "_SpeedTreeGeom";

        MaterialProperty windEnable = null;
        public const string kWindEnable = "_WindEnabled";
        MaterialProperty windQuality = null;
        public const string kWindQuality = "_WindQuality";

        MaterialProperty isBillboard = null;
        public const string kIsBillboard = "_Billboard";
        MaterialProperty billboardFacesCam = null;
        public const string kBillboardFacing = "_BillboardFacing";

        public SpeedTreeLitOptionsUIBlock(Expandable expandableBit)
        {
            m_ExpandableBit = expandableBit;
        }

        public override void LoadMaterialProperties()
        {
            geomType = FindProperty(kGeomType);

            windEnable = FindProperty(kWindEnable);
            windQuality = FindProperty(kWindQuality);

            isBillboard = FindProperty(kIsBillboard);
            billboardFacesCam = FindProperty(kBillboardFacing);
        }

        void DrawSpeedTreeInputsGUI()
        {
            if (geomType != null)
                materialEditor.ShaderProperty(geomType, Styles.typeText);

            if (windEnable != null && windQuality != null)
            {
                materialEditor.ShaderProperty(windEnable, Styles.enableWindText);
                materialEditor.ShaderProperty(windQuality, Styles.windQualityText);
            }

            if (isBillboard != null)
            {
                materialEditor.ShaderProperty(isBillboard, Styles.billboardText);
                if (billboardFacesCam != null)
                {
                    materialEditor.ShaderProperty(billboardFacesCam, Styles.billboardFacingText);
                }
            }
        }

        void DrawSpeedTreeKeywordsGUI()
        {
            mAssetVersion = (SpeedTreeVersionEnum)EditorGUILayout.EnumPopup(mAssetVersion);

            if (windEnable == null || windQuality == null)
            {
                mWindEnable = EditorGUILayout.Toggle(Styles.enableWindText, mWindEnable);
                if (mWindEnable)
                    mWindQuality = (WindQualityEnum)EditorGUILayout.EnumPopup(Styles.windQualityText, mWindQuality);
            }

            return;
            if (mAssetVersion == SpeedTreeVersionEnum.SpeedTreeVer7)
                mGeomType = (SpeedTree7Geom)EditorGUILayout.EnumPopup(Styles.typeText, mGeomType);

            mBillboard = EditorGUILayout.Toggle(Styles.billboardText, mBillboard);
            mBillboardFacing = EditorGUILayout.Toggle(Styles.billboardFacingText, mBillboardFacing);
        }

        public override void OnGUI()
        {
            using (var header = new MaterialHeaderScope(Styles.SpeedTreeHeader, (uint)m_ExpandableBit, materialEditor))
            {
                if (header.expanded)
                {
                    DrawSpeedTreeInputsGUI();
                    DrawSpeedTreeKeywordsGUI();
                }
            }
        }
    }
}
