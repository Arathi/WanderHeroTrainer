using BepInEx.Logging;
using UnityEngine;

namespace WanderHeroTrainer
{
    public class CheatWindow
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        private ManualLogSource Logger { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        // public Rect WindowRect { get {
        //     var rect = new Rect(X, Y, Width, Height);
        //     return GUI.Window(Id, rect, Draw, Title);
        // }}
        
        // public CheatWindow(string title, float x, float y, float width, float height, ManualLogSource logger)
        // {
        //     Id = Random.Range(0, int.MaxValue);
        //     Title = title;
        //     Scene = GuiScene.MainMenu;
        //     Logger = logger;
        // }
    }
}