using UnityEngine;

namespace IslandBoy
{
    public class CursorManager : Singleton<CursorManager>
    {
        [SerializeField] private Texture2D _defaultCursor;
        [SerializeField] private Texture2D _promptCursor;
        [SerializeField] private Texture2D _promptCursorTransparent;

        private void Start()
        {
            SetDefaultCursor();
        }

        public void SetDefaultCursor()
        {
            Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        //public void SetPromptCursor()
        //{
        //    Cursor.SetCursor(_promptCursor, Vector2.zero, CursorMode.ForceSoftware);
        //}

        //public void SetPromptCursorTransparent()
        //{
        //    Cursor.SetCursor(_promptCursorTransparent, Vector2.zero, CursorMode.ForceSoftware);
        //}
    }
}
