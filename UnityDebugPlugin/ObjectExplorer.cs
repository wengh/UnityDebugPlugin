using UnityEngine;
using PluginManager.Core;

namespace UnityDebugPlugin
{
    /// <summary>
    /// In-game object explorer. Only enabled in developer builds.
    /// </summary>
    [OnGameInit]
    public class ObjectExplorer : SingleInstance<ObjectExplorer>
    {
        public override string Name
        {
            get { return "Object Explorer"; }
        }

        public string WindowTitle = "Object Explorer";

        public HierarchyPanel HierarchyPanel { get; private set; }
        public InspectorPanel InspectorPanel { get; private set; }

        private readonly int windowID = Util.GetWindowID();
        private Rect windowRect = new Rect(20, 20, 800, 600);
        private Key key;

        public bool IsVisible = false;

        private void Awake ()
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }

        private void Start ()
        {
            HierarchyPanel = gameObject.AddComponent<HierarchyPanel>();
            InspectorPanel = gameObject.AddComponent<InspectorPanel>();

            key = Keybindings.AddKeybinding("Object Explorer",
              new Key(KeyCode.LeftControl, KeyCode.O));
        }

        private void Update ()
        {
            if (key.Pressed())
            {
                IsVisible = !IsVisible;
            }
        }

        private void OnGUI ()
        {
            GUI.skin = ModGUI.Skin;

            if (IsVisible)
            {
                Cursor.visible = true;
                windowRect = GUILayout.Window(windowID, windowRect, DoWindow,
                  WindowTitle);
                windowRect = Util.PreventOffScreenWindow(windowRect);
            }
        }

        private void DoWindow (int id)
        {
            GUILayout.BeginHorizontal();

            HierarchyPanel.Display();
            InspectorPanel.Display();

            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0, 0, windowRect.width, GUI.skin.window.padding.top));
        }
    }
}