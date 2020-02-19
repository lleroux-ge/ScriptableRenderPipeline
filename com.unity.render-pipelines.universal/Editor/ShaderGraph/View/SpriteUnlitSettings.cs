using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphing.Util;
using UnityEditor.ShaderGraph.Drawing.Controls;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.ShaderGraph.Drawing;

namespace UnityEditor.Experimental.Rendering.Universal
{
    class SpriteUnlitSettingsView : VisualElement
    {
        SpriteUnlitMasterNode m_Node;

        public SpriteUnlitSettingsView(SpriteUnlitMasterNode node)
        {
            m_Node = node;

            PropertySheet ps = new PropertySheet();

            ps.Add(new PropertyRow(new Label("Blend")), (row) =>
                {
                    row.Add(new EnumField(AlphaMode.Additive), (field) =>
                    {
                        field.value = m_Node.alphaMode;
                        field.RegisterValueChangedCallback(ChangeAlphaMode);
                    });
                });

            Add(ps);
        }

        void ChangeAlphaMode(ChangeEvent<Enum> evt)
        {
            if (Equals(m_Node.alphaMode, evt.newValue))
                return;

            m_Node.owner.owner.RegisterCompleteObjectUndo("Alpha Mode Change");
            m_Node.alphaMode = (AlphaMode)evt.newValue;
        }
    }
}
