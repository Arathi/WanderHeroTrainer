using System.Collections;
using System.Collections.Generic;
using SoulsGame;
using HarmonyLib;

// ！需要修改！ 分析SoulsGame.CustomSaveData，Used By
using SaveDataOper = ijjjjjijjjjiiijjijijiiijjiijiiiiijjjiijjjjjjjij;

namespace WanderHeroTrainer
{
    public partial class WanderHeroTrainerPlugin
    {
        // ！需要修改！ 反编译SaveDataOper，找到CustomSaveData的实例对象
        const string fieldNameCustomSaveData = "ijjiiijjjjijijiijjijijjjjjijiijjiiijiijjjiiijji";

        // 获取CustomSaveData实例
        public CustomSaveData SaveData
        {
            get
            {
                Traverse classTraverse = Traverse.Create(typeof(SaveDataOper));
                Traverse<CustomSaveData> fieldTraverse = classTraverse.Field<CustomSaveData>(fieldNameCustomSaveData);
                return fieldTraverse.Value;
            }
        }

        // 根据itemId获取物品信息
        public Json_Item GetItemById(string itemId)
        {
            // ！需要修改！ 反编译SoulsGame.Cfg_Item，随便找一个参数为(string)返回Json_Item的方法，有很多，效果好像都一样
            return Cfg_Item.Inst.jiiijjjiijjjiiijiiijijiiiijjjjjiijjijiijiiijjji(itemId);
        }

        // 获取初始物品id列表
        private List<string> ItemIdList
        {
            get
            {
                List<string> ids = new List<string>();
                
                // Traverse classTraverse = Traverse.Create(typeof(Cfg_Item));
                // Traverse<Cfg_Item> instTraverse = classTraverse.Field<Cfg_Item>("Inst");
                // Traverse<Json_Item[]> fieldTraverse = classTraverse.Field<Json_Item[]>("_initItems");
                Traverse<Json_Item[]> fieldTraverse = Traverse.Create(Cfg_Item.Inst).Field<Json_Item[]>("_initItems");
                var items = fieldTraverse.Value;
                foreach (var item in items)
                {
                    ids.Add(item._id);
                }

                return ids;
            }
        }

        // 加载GameSettings
        private void LoadGameSettings()
        {
            Logger.LogInfo("正在加载GameSettings");
            var source = SaveData._gameSettings;
            GameSettings = new Json_GameSettings();
            GameSettings._monDamageX = source._monDamageX;
            GameSettings._monHpX = source._monHpX;
            GameSettings._questReward = source._questReward;
            GameSettings._dropCollect = source._dropCollect;
            GameSettings._bossRaidCd = source._bossRaidCd;
            GameSettings._troopRaidSleep = source._troopRaidSleep;
            GameSettings._troopRaidCd = source._troopRaidCd;
            GameSettings._troopRaidPowerX = source._troopRaidPowerX;
            GameSettings._allowHeroDead = source._allowHeroDead;
            GameSettings._allowPlayerDead = source._allowPlayerDead;
            Logger.LogInfo("GameSettings加载完成");
        }

        // 保存GameSettings
        private void SaveGameSettings()
        {
            Logger.LogInfo("正在保存GameSettings");
            var source = SaveData._gameSettings;
            source._monDamageX = GameSettings._monDamageX;
            source._monHpX = GameSettings._monHpX;
            source._questReward = GameSettings._questReward;
            source._dropCollect = GameSettings._dropCollect;
            source._bossRaidCd = GameSettings._bossRaidCd;
            source._troopRaidSleep = GameSettings._troopRaidSleep;
            source._troopRaidCd = GameSettings._troopRaidCd;
            source._troopRaidPowerX = GameSettings._troopRaidPowerX;
            source._allowHeroDead = GameSettings._allowHeroDead;
            source._allowPlayerDead = GameSettings._allowPlayerDead;
            Logger.LogInfo("GameSettings保存完成");
        }

        // 加载团长信息
        private void LoadLeaderInfo()
        {
            Logger.LogInfo("正在加载团长信息");
            var saveData = SaveData;
            for (int index = 0; index < properties.Length; index++)
            {
                heroData[index] = saveData._heroData[index];
                heroUpgrade[index] = saveData._heroUpgrade[index];
            }
            leaderExp = saveData._leaderExp;
            Logger.LogInfo("团长信息加载完成");
        }

        // 保存团长信息
        private void SaveLeaderInfo()
        {
            Logger.LogInfo("正在保存团长信息");
            var saveData = SaveData;
            for (int index = 0; index < properties.Length; index++)
            {
                saveData._heroData[index] = heroData[index];
                saveData._heroUpgrade[index] = heroUpgrade[index];
            }
            saveData._leaderExp = leaderExp;
            Logger.LogInfo("团长信息保存完成");
        }

        // 加载背包信息
        private void LoadPack() {
            Logger.LogInfo("正在加载背包信息");
            itemAmounts.Clear();

            var initItems = ItemIdList;
            foreach (var itemId in initItems) {
                itemAmounts[itemId] = "0";
            }

            var itemIdList = GetItemIdListInPack();
            foreach (var itemId in itemIdList) {
                itemAmounts[itemId] = GetItemAmountInPack(itemId) + "";
            }
            Logger.LogInfo("背包信息加载完成");
        }

        private void SavePack() {
            foreach (var item in itemAmounts) {
                string itemId = item.Key;
                string itemName = GetItemName(itemId);
                string amountInput = item.Value;
                int amountLast = GetItemAmountInPack(itemId);
                bool parseSucc = int.TryParse(amountInput, out int amount);
                if (parseSucc) {
                    int delta = amount - amountLast;
                    if (delta != 0) {
                        Logger.LogInfo($"物品[{itemName}]({itemId})数量发生变化：{amountLast} -> {amount}，差值：{delta}");
                        AddItemToPack(itemId, delta);
                    }
                }
                else {
                    Logger.LogInfo($"物品[{itemName}]({itemId})数量无效：{amountInput}");
                }
            }
        }

        // 获取背包中的物品ID列表
        private List<string> GetItemIdListInPack()
        {
            // ！需要修改！ 分析CustomSaveData._pack，Read By
            return SaveDataOper.jijiiijijjiijijjjiijjiiijjjiijjjjijiijjijjjjiij();
        }

        // 根据物品ID获取背包中的物品数量
        private int GetItemAmountInPack(string itemId)
        {
            // ！需要修改！ 同上
            return SaveDataOper.jijiijijjjiijjjjiiiiijijiijiijjjjjjijjjiijijjij(itemId);
        }

        // 往背包中添加指定数量的物品（传负值可以减少）
        private void AddItemToPack(string itemId, int delta)
        {
            // ！需要修改！ 同上
            SaveDataOper.iiiiiijiijijjjjijjjjjjijjjjijjjjiiijiiijiijiiij(itemId, delta);
        }
    }
}