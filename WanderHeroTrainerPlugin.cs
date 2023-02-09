using BepInEx;
using BepInEx.Configuration;
using SoulsGame;
using UnityEngine;

namespace WanderHeroTrainer
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public partial class WanderHeroTrainerPlugin : BaseUnityPlugin
    {
        const string GUID = "com.undsf.bepinex.WanderHeroTrainer";
        const string NAME = "WanderHeroTrainer";
        const string VERSION = "0.4.0";
        const string GAME_VERSION = "0.8.230208";

        const int WINDOW_ID = 724725413;

        private ConfigEntry<bool> Enabled { get; set; }
        private ConfigEntry<KeyCode> EnableKey { get; set; }
        private ConfigEntry<KeyboardShortcut> GuiKey { get; set; }
        
        private KeyboardShortcut TestKey = new KeyboardShortcut(KeyCode.T, KeyCode.LeftControl);

        private Rect WindowRect { get; set; }

        private bool DisplayGui { get; set; }

        private GuiScene Scene { get; set; }

        private void Awake()
        {
            Logger.LogInfo($"{GUID} {VERSION} for WH-{GAME_VERSION} 已唤醒");
            Enabled = Config.Bind("Commons", "Enabled", false, "插件开关");
            EnableKey = Config.Bind("Commons", "EnableKey", KeyCode.F11, "开关按钮");
            GuiKey = Config.Bind( "Commons", "GuiKey", new KeyboardShortcut(KeyCode.G, KeyCode.LeftControl), "GUI按钮" );
            Scene = GuiScene.MainMenu;
        }

        private void Start()
        {
            Logger.LogInfo($"{GUID} {VERSION} 已启动");
        }

        private void Update()
        {
            // 更新
            KeyEventProcess();
            if (!Enabled.Value) return;
        }

        private void OnGUI()
        {
            if (!Enabled.Value) return;
            if (!DisplayGui) return;

            var rect = new Rect(10, 10, Screen.width / 4.0f, Screen.height - 20);
            WindowRect = GUI.Window(WINDOW_ID, rect, GuiWindowPainting, "存档编辑");
        }
        
        private void KeyEventProcess()
        {
            if (Input.GetKeyDown(EnableKey.Value))
            {
                Enabled.Value = !Enabled.Value;
                string enableStr = Enabled.Value ? "启用" : "禁用";
                Logger.LogInfo($"{GUID} {VERSION} 已{enableStr}");
                if (Enabled.Value && Scene != GuiScene.MainMenu)
                {
                    Logger.LogInfo("重置菜单");
                    Scene = GuiScene.MainMenu;
                }
            }

            if (!Enabled.Value) return;

            if (GuiKey.Value.IsDown())
            {
                DisplayGui = !DisplayGui;
                string displayStr = DisplayGui ? "显示" : "隐藏";
                Logger.LogInfo($"{displayStr}GUI");
            }

            if (TestKey.IsDown())
            {
                var itemIdList = ItemIdList;
                foreach (var itemId in itemIdList)
                {
                    Json_Item item = GetItemById(itemId);
                    Logger.LogInfo($"{itemId} => {item.rName}");
                    Logger.LogInfo(item.rDesc);
                    Logger.LogInfo("");
                }
            }
        }

        private void OnDestroy() {
            Logger.LogInfo($"{GUID} {VERSION} 已销毁");
        }
    }
}
