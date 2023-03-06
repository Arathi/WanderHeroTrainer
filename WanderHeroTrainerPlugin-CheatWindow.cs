using System;
using System.Collections.Generic;
using SoulsGame;
using UnityEngine;

namespace WanderHeroTrainer
{
    public partial class WanderHeroTrainerPlugin
    {
        private Json_GameSettings GameSettings { get; set; }
        
        // LeaderInfo
        private int[] heroData = new int[8];
        private int[] heroUpgrade = new int[8];
        private int leaderExp = 0;
        private string[] properties = new string[]
        {
            "统率",
            "指挥",
            "行动",
            "急救",
            "交易",
            "采集",
            "后勤",
            "医疗"
        };

        private Dictionary<string, string> itemAmounts = new Dictionary<string, string>();
        
        private Dictionary<string, string> inputs = new Dictionary<string, string>();

        Vector2 scrollPosition;

        public void GuiWindowPainting(int windowId)
        {
            if (Scene == GuiScene.MainMenu)
            {
                DrawMainMenu(windowId);
            }
            else if (Scene == GuiScene.GameSettingsEdit)
            {
                DrawGameSettings(windowId);
            }
            else if (Scene == GuiScene.CardEdit)
            {
                DrawCardEdit(windowId);
            }
            else if (Scene == GuiScene.PackEdit)
            {
                DrawPackEdit(windowId);
            }
            else if (Scene == GuiScene.LeaderEdit)
            {
                DrawLeaderEdit(windowId);
            }
        }
        
        private void DrawMainMenu(int windowId)
        {
            GUILayout.BeginArea(new Rect(10, 20, WindowRect.width - 20, WindowRect.height - 30));
            {
                GUILayout.Label("主菜单");

                if (GUILayout.Button("编辑游戏设置"))
                {
                    Logger.LogInfo("你点击了编辑游戏设置");
                    LoadGameSettings();
                    Scene = GuiScene.GameSettingsEdit;
                }

                if (GUILayout.Button("编辑玩家角色卡"))
                {
                    Logger.LogInfo("编辑玩家角色卡");
                    // TODO 加载玩家角色卡 SaveData._player
                    Scene = GuiScene.CardEdit;
                }

                if (GUILayout.Button("编辑背包"))
                {
                    Logger.LogInfo("编辑背包");
                    // TODO 加载背包 SaveData._pack
                    Scene = GuiScene.PackEdit;
                }

                if (GUILayout.Button("编辑团长属性"))
                {
                    Logger.LogInfo("编辑团长属性");
                    LoadLeaderInfo();
                    Scene = GuiScene.LeaderEdit;
                }
            }
            GUILayout.EndArea();
        }

        // 绘制游戏设置编辑界面
        private void DrawGameSettings(int windowId)
        {
            var labelWidth = GUILayout.MinWidth((WindowRect.width - 20) * 0.40F);
            var textFieldWidth = GUILayout.MinWidth((WindowRect.width - 20) * 0.50F);
            GUILayout.BeginArea(new Rect(10, 20, WindowRect.width - 20, WindowRect.height - 30));
            {
                GUILayout.Label("编辑游戏设置");
                GUILayout.Label("");
                
                #region 怪物伤害倍率
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("怪物伤害倍率", labelWidth);
                    
                    string key = "GameSettings._monDamageX";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        int percent = (int)(GameSettings._monDamageX * 100);
                        text = percent.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}%");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int percent = 0;
                        bool parseSucc = int.TryParse(changed, out percent);
                        if (parseSucc)
                        {
                            GameSettings._monDamageX = percent / 100f;
                            Logger.LogInfo($"怪物伤害倍率发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("%");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 怪物血量倍率
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("怪物血量倍率", labelWidth);
                    
                    string key = "GameSettings._monHpX";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        int percent = (int)(GameSettings._monHpX * 100);
                        text = percent.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}%");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int percent = 0;
                        bool parseSucc = int.TryParse(changed, out percent);
                        if (parseSucc)
                        {
                            GameSettings._monHpX = percent / 100f;
                            Logger.LogInfo($"怪物血量倍率发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("%");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 任务赏金
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("任务赏金倍率", labelWidth);
                    
                    string key = "GameSettings._questReward";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        int percent = (int)(GameSettings._questReward * 100);
                        text = percent.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}%");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int percent = 0;
                        bool parseSucc = int.TryParse(changed, out percent);
                        if (parseSucc)
                        {
                            GameSettings._questReward = percent / 100f;
                            Logger.LogInfo($"任务赏金倍率发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("%");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 掉落采集
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("掉落采集倍率", labelWidth);
                    
                    string key = "GameSettings._dropCollect";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        int percent = (int)(GameSettings._dropCollect * 100);
                        text = percent.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}%");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int percent = 0;
                        bool parseSucc = int.TryParse(changed, out percent);
                        if (parseSucc)
                        {
                            GameSettings._dropCollect = percent / 100f;
                            Logger.LogInfo($"掉落采集倍率发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("%");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 魔王间隔
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("魔王间隔", labelWidth);
                    
                    string key = "GameSettings._bossRaidCd";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        text = GameSettings._bossRaidCd.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int value = 0;
                        bool parseSucc = int.TryParse(changed, out value);
                        if (parseSucc)
                        {
                            GameSettings._bossRaidCd = value;
                            Logger.LogInfo($"魔王间隔发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("月");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 魔王沉睡
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("魔王沉睡", labelWidth);
                    
                    string key = "GameSettings._troopRaidSleep";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        text = GameSettings._troopRaidSleep.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int value = 0;
                        bool parseSucc = int.TryParse(changed, out value);
                        if (parseSucc)
                        {
                            GameSettings._troopRaidSleep = value;
                            Logger.LogInfo($"魔王沉睡发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("月");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 扩张间隔
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("扩张间隔", labelWidth);
                    
                    string key = "GameSettings._troopRaidCd";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        text = GameSettings._troopRaidCd.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int value = 0;
                        bool parseSucc = int.TryParse(changed, out value);
                        if (parseSucc)
                        {
                            GameSettings._troopRaidCd = value;
                            Logger.LogInfo($"扩张间隔发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("周");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 魔王军规模
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("魔王军规模", labelWidth);
                    
                    string key = "GameSettings._troopRaidPowerX";
                    string text = "";
                    if (!inputs.ContainsKey(key))
                    {
                        text = GameSettings._troopRaidPowerX.ToString();
                        inputs[key] = text;
                        Logger.LogInfo($"inputs[{key}] = {text}");
                    }
                    else
                    {
                        text = inputs[key];
                    }
                    
                    string changed = GUILayout.TextField(text, textFieldWidth);
                    if (changed != text)
                    {
                        inputs[key] = changed;
                        int value = 0;
                        bool parseSucc = int.TryParse(changed, out value);
                        if (parseSucc)
                        {
                            GameSettings._troopRaidPowerX = value;
                            Logger.LogInfo($"魔王军规模发生变化：{text} -> {changed}");
                        }
                    }
                    GUILayout.Label("倍");
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 允许同伴死亡
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("允许同伴死亡", labelWidth);
                    bool allow = GameSettings._allowHeroDead;
                    bool changed = GUILayout.Toggle(allow, "允许同伴死亡");
                    if (allow != changed)
                    {
                        GameSettings._allowHeroDead = changed;
                        string allowStr = (changed) ? "允许" : "禁止";
                        Logger.LogInfo($"{allowStr}同伴死亡");
                    }
                    GUILayout.Label("", GUILayout.ExpandWidth(true));
                }
                GUILayout.EndHorizontal();
                #endregion
                
                #region 允许玩家死亡
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("允许玩家死亡", labelWidth);
                    bool allow = GameSettings._allowPlayerDead;
                    bool changed = GUILayout.Toggle(allow, "允许同伴死亡");
                    if (allow != changed)
                    {
                        GameSettings._allowPlayerDead = changed;
                        string allowStr = (changed) ? "允许" : "禁止";
                        Logger.LogInfo($"{allowStr}玩家死亡");
                    }
                    GUILayout.Label("", GUILayout.ExpandWidth(true));
                }
                GUILayout.EndHorizontal();
                #endregion
                
                GUILayout.Label("", GUILayout.ExpandHeight(true));

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("退出"))
                    {
                        Logger.LogInfo("退出");
                        inputs.Clear();
                        Scene = GuiScene.MainMenu;
                    }
                    if (GUILayout.Button("保存"))
                    {
                        SaveGameSettings();
                        inputs.Clear();
                        Scene = GuiScene.MainMenu;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawCardEdit(int windowId)
        {
            GUILayout.BeginArea(new Rect(10, 20, WindowRect.width - 20, WindowRect.height - 30));
            {
                GUILayout.Label("编辑卡牌信息");

                GUILayout.Label("", GUILayout.ExpandHeight(true));
                
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("退出"))
                    {
                        Logger.LogInfo("退出");
                        Scene = GuiScene.MainMenu;
                    }
                    if (GUILayout.Button("保存"))
                    {
                        Logger.LogInfo("保存");
                        Scene = GuiScene.MainMenu;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawPackEdit(int windowId)
        {
            var labelWidth = GUILayout.MinWidth((WindowRect.width - 20) * 0.40F);
            var textFieldWidth = GUILayout.MinWidth((WindowRect.width - 20) * 0.50F);

            GUILayout.BeginArea(new Rect(10, 20, WindowRect.width - 20, WindowRect.height - 20));
            {
                GUILayout.Label("编辑背包");
                
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("退出"))
                    {
                        Logger.LogInfo("退出");
                        Scene = GuiScene.MainMenu;
                    }
                    if (GUILayout.Button("刷新"))
                    {
                        Logger.LogInfo("刷新");
                        LoadPack();
                    }
                    if (GUILayout.Button("保存"))
                    {
                        Logger.LogInfo("保存");
                        SavePack();
                        // Scene = GuiScene.MainMenu;
                    }
                }
                GUILayout.EndHorizontal();

                /*
                const string keyItemId = "text_item_id";
                GUILayout.BeginHorizontal();
                {
                    var itemId = "";
                    if (inputs.ContainsKey(keyItemId)) {
                        itemId = inputs[keyItemId];
                    }
                    var changed = GUILayout.TextField(itemId, textFieldWidth);
                    if (changed != itemId) {
                        inputs[keyItemId] = changed;
                    }

                    // GUILayout.Label("");

                    if (GUILayout.Button("添加物品"))
                    {
                        var item = GetItemById(itemId);
                        if (item != null) {
                            Logger.LogInfo($"添加一个[{item.rName}]({itemId})");
                            AddItemToPack(itemId, 1);
                            LoadPack();
                        }
                        else
                        {
                            Logger.LogInfo("物品{itemId}不存在");
                        }
                    }
                }
                GUILayout.EndHorizontal();
                */

                scrollPosition = GUILayout.BeginScrollView(
                    scrollPosition, 
                    GUILayout.Width(WindowRect.width - 20),
                    GUILayout.Height(WindowRect.height - 80)
                );
                {
                    if (itemAmounts.Count == 0) {
                        LoadPack();
                    }

                    foreach (var item in itemAmounts)
                    {
                        String itemId = item.Key;
                        String text = item.Value;
                        GUILayout.BeginHorizontal();
                        {
                            string itemName = GetItemName(itemId);
                            GUILayout.Label(itemName, labelWidth);
                            string changed = GUILayout.TextField(text, textFieldWidth);
                            if (changed != text)
                            {
                                itemAmounts[itemId] = changed;
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();

                // GUILayout.Label("", GUILayout.ExpandHeight(true));
            }
            GUILayout.EndArea();
        }

        private void DrawLeaderEdit(int windowId)
        {
            GUILayout.BeginArea(new Rect(10, 20, WindowRect.width - 20, WindowRect.height - 30));
            {
                GUILayout.Label("编辑团长信息");
                GUILayout.Label("");

                for (int index = 0; index < properties.Length; index++)
                {
                    int sum = heroData[index] + heroUpgrade[index];
                    string heroDataKey = "heroData_" + index;
                    if (!inputs.ContainsKey(heroDataKey))
                    {
                        inputs[heroDataKey] = heroData[index].ToString();
                    }

                    string heroUpgradeKey = "heroUpgrade_" + index;
                    if (!inputs.ContainsKey(heroUpgradeKey))
                    {
                        inputs[heroUpgradeKey] = heroUpgrade[index].ToString();
                    }
                    
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label($"{properties[index]} = {sum} = ");
                        string heroDataChanged = GUILayout.TextField(inputs[heroDataKey]);
                        if (heroDataChanged != inputs[heroDataKey])
                        {
                            inputs[heroDataKey] = heroDataChanged;
                            int changed = 0;
                            bool parseSucc = int.TryParse(heroDataChanged, out changed);
                            if (parseSucc)
                            {
                                heroData[index] = changed;
                                Logger.LogInfo($"{properties[index]}原始值修改为{changed}");
                            }
                        }
                        string heroUpgradeChanged = GUILayout.TextField(inputs[heroUpgradeKey]);
                        if (heroUpgradeChanged != inputs[heroUpgradeKey])
                        {
                            inputs[heroUpgradeKey] = heroUpgradeChanged;
                            int changed = 0;
                            bool parseSucc = int.TryParse(heroUpgradeChanged, out changed);
                            if (parseSucc)
                            {
                                heroUpgrade[index] = changed;
                                Logger.LogInfo($"{properties[index]}升级值修改为{changed}");
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("经验值");
                    if (!inputs.ContainsKey("leaderExp"))
                    {
                        inputs["leaderExp"] = leaderExp.ToString();
                    }
                    string changed = GUILayout.TextField(inputs["leaderExp"]);
                    if (changed != inputs["leaderExp"])
                    {
                        inputs["leaderExp"] = changed;
                        int value = 0;
                        bool parseSucc = Int32.TryParse(changed, out value);
                        if (parseSucc)
                        {
                            leaderExp = value;
                            Logger.LogInfo($"经验值修改为{changed}");
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("", GUILayout.ExpandHeight(true));
                
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("退出"))
                    {
                        Logger.LogInfo("退出");
                        inputs.Clear();
                        Scene = GuiScene.MainMenu;
                    }
                    if (GUILayout.Button("保存"))
                    {
                        Logger.LogInfo("保存");
                        inputs.Clear();
                        SaveLeaderInfo();
                        Scene = GuiScene.MainMenu;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}