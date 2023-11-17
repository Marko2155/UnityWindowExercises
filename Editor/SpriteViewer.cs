using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;


public class SpriteViewer : EditorWindow
{
    [MenuItem("Tools/Sprite Viewer")]
    public static void ShowExample()
    {
        SpriteViewer wnd = GetWindow<SpriteViewer>();
        wnd.titleContent = new GUIContent("Sprite Viewer");
    }

    private VisualElement m_rightside;

    public void CreateGUI()
    {
        var guids = AssetDatabase.FindAssets("t:Sprite");
        var sprites = new List<Sprite>();

        foreach (var guid in guids)
        {
            sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        VisualElement root = rootVisualElement;

        var twosidesplitview = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        
        root.Add(twosidesplitview);

        var leftside = new ListView();
        twosidesplitview.Add(leftside);
        m_rightside = new VisualElement();
        twosidesplitview.Add(m_rightside);

        leftside.makeItem = () => new Label();
        leftside.bindItem = (item, index) => { (item as Label).text = sprites[index].name; };
        leftside.itemsSource = sprites;
        leftside.onSelectionChange += OnSpriteChange;
    }

    private void OnSpriteChange(IEnumerable<object> selectedItems)
    {
        m_rightside.Clear();
        var selectedSprite = selectedItems.First() as Sprite;

        if (selectedSprite == null) return;

        var spriteImage = new Image();
        spriteImage.scaleMode = ScaleMode.ScaleToFit;
        spriteImage.sprite = selectedSprite;
        
        m_rightside.Add(spriteImage);
    }
}
