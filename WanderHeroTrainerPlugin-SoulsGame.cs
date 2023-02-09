using System.Collections;
using System.Collections.Generic;
using SoulsGame;
using HarmonyLib;

namespace WanderHeroTrainer
{
    public partial class WanderHeroTrainerPlugin
    {
        // 获取CustomSaveData实例
        public CustomSaveData SaveData
        {
            get
            {
                Traverse classTraverse = Traverse.Create(typeof(jijiijjjjijjiijiijijjjjiijjiiijjjiiijjjijjiiijj));
                Traverse<CustomSaveData> fieldTraverse = classTraverse.Field<CustomSaveData>("jjiijiijjiijjjiijijjiijjjiiiiijiiijjijiijjjjiij");
                return fieldTraverse.Value;
            }
        }

        // 根据itemId获取物品信息
        public Json_Item GetItemById(string itemId)
        {
            return Cfg_Item.Inst.jjijjijijjjijiiijiiijjjjjjjjijjijjijiiiiijjjjij(itemId);
        }

        // 获取初始物品id列表
        private List<string> ItemIdList
        {
            get
            {
                List<string> ids = new List<string>();
                
                Traverse classTraverse = Traverse.Create(typeof(Cfg_Item));
                Traverse<Json_Item[]> fieldTraverse = classTraverse.Field<Json_Item[]>("_initItems");

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

        // 获取背包中的物品ID列表
        private List<string> GetItemIdListInPack()
        {
            return jijiijjjjijjiijiijijjjjiijjiiijjjiiijjjijjiiijj.jijjijiijjjijjjiiiiijijjjiiijijjijiijjjjjijjjjj();
        }

        // 根据物品ID获取背包中的物品数量
        private int GetItemAmountInPack(string itemId)
        {
            return jijiijjjjijjiijiijijjjjiijjiiijjjiiijjjijjiiijj.jjjjiiiiijijjjiijiijjjjiijjiiijiijjjjjijjjjiiii(itemId);
        }

        // 往背包中添加指定数量的物品（传负值可以减少）
        private void AddItemToPack(string itemId, int delta)
        {
            jijiijjjjijjiijiijijjjjiijjiiijjjiiijjjijjiiijj.jiiijiiiiijjiijjjijijjjjjijiijjjjiijijjiiiijjjj(itemId, delta);
        }
    }
}