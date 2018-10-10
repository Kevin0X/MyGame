using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetFontUV : EditorWindow {
    public Font font;

    [MenuItem("Tools/设置字体UV")]
    static void CreateWizard()
    {
        var win = EditorWindow.CreateInstance<SetFontUV>();
        win.Show();
    }



}
