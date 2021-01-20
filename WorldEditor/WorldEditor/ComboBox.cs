namespace WorldEditor
{
    using UnityEngine;

    public class ComboBox
    {
        private static bool forceToUnShow = false;
        private static int useControlID = -1;
        private bool isClickedComboButton = false;
        private int selectedItemIndex = 0;
        private Vector2 scrollViewVector =  Vector2.zero;

        private Rect rect;
        private GUIContent buttonContent;
        private GUIContent[] listContent;
        private string buttonStyle;
        private string boxStyle;
        private GUIStyle listStyle;

        public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle)
        {
            this.rect = rect;
            this.buttonContent = buttonContent;
            this.listContent = listContent;
            this.buttonStyle = "button";
            this.boxStyle = "box";
            this.listStyle = listStyle;
        }

        public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle,
            string boxStyle, GUIStyle listStyle)
        {
            this.rect = rect;
            this.buttonContent = buttonContent;
            this.listContent = listContent;
            this.buttonStyle = buttonStyle;
            this.boxStyle = boxStyle;
            this.listStyle = listStyle;
        }

        public int Show()
        {
            if (forceToUnShow)
            {
                forceToUnShow = false;
                isClickedComboButton = false;
            }

            bool done = false;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.mouseUp:
                {
                    if (isClickedComboButton)
                    {
                        done = true;
                    }
                }
                break;
            }

            if (GUI.Button(rect, buttonContent, buttonStyle))
            {
                if (useControlID == -1)
                {
                    useControlID = controlID;
                    isClickedComboButton = false;
                }

                if (useControlID != controlID)
                {
                    forceToUnShow = true;
                    useControlID = controlID;
                }

                isClickedComboButton = true;
            }

            if (isClickedComboButton)
            {
                float itemsheight = listStyle.CalcHeight(listContent[0], 1.0f) * (listContent.Length + 5);
                Rect listRect = new Rect(rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f),
                    rect.width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length);
                
                scrollViewVector = GUI.BeginScrollView (new Rect (rect.x, rect.y + rect.height, rect.width * 1.4f, 200), scrollViewVector,
                    new Rect (rect.x, rect.y, rect.width, itemsheight + rect.height), false, false);

                GUI.Box(listRect, "", boxStyle);
                int newSelectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, listStyle);
                if (newSelectedItemIndex != selectedItemIndex)
                {
                    selectedItemIndex = newSelectedItemIndex;
                    done = true;
                }
                GUI.EndScrollView();
            }

            if (done)
                isClickedComboButton = false;

            return selectedItemIndex;
        }

        public int SelectedItemIndex
        {
            get { return selectedItemIndex; }
            set { selectedItemIndex = value; }
        }
    }
}