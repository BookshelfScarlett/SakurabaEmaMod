using Newtonsoft.Json;
using SakurabaEmaMod.Menus.MainMenu;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Menus.Managemments
{
    public class BackgroundConfig
    {
        public int BackgroundID { get; set; }
        public bool IsDoneMoonlordFight { get; set; }
        public const int DefaultID = ManosabaMenuID.Ema;
        public const bool DefaultBoolen = true;
    }
    public class ManosabaMenuSystem : ModSystem
    {
        private const string DefaultFileName = "ManosabaBackgroundConfig.json";
        private string FilePath;
        public static string CurrentFileName = "None";
        public static ManosabaMenuSystem Instance;
        public override void Load()
        {
            Instance = this;
            //初始化文件路径，将文件放入到本地游戏内
            FilePath = Path.Combine(Main.SavePath, "ManosabaMod");
            //不存在文件路径时创建路径
            Directory.CreateDirectory(FilePath);
            //然后，加载与读取
            LoadJsonConfig();
        }

        public void LoadJsonConfig()
        {
            try
            {
                //遍历这个路径下所有的json文件
                //目前只有一个json文件，这里直接用个数即可
                string[] jsonFileArray = Directory.GetFiles(FilePath, "*.json");
                //如果不存在json文件，新建一个
                if (jsonFileArray.Length == 0)
                {
                    CurrentFileName = DefaultFileName;
                    //把当前文件名与默认文件名拼起来
                    string defaultPath = Path.Combine(FilePath, CurrentFileName);
                    //实例化自定义的json内容
                    var defaultConfig = new BackgroundConfig { BackgroundID = BackgroundConfig.DefaultID, IsDoneMoonlordFight = BackgroundConfig.DefaultBoolen };
                    string jsonContent = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                    //写入内容
                    File.WriteAllText(defaultPath, jsonContent);
                    //标记允许开始修改背景
                    ManosabaBackground.CanChangeMenu = true;
                    ManosabaBackground.CurrentBackgroundID = BackgroundConfig.DefaultID;
                    Logo.IsDoneMoonLordFight = BackgroundConfig.DefaultBoolen;
                }
                else
                {
                    CurrentFileName = Path.GetFileName(jsonFileArray[0]);
                    string jsonFilePath = Path.Combine(FilePath, CurrentFileName);
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    var config = JsonConvert.DeserializeObject<BackgroundConfig>(jsonContent);
                    ManosabaBackground.CanChangeMenu = true;
                    ManosabaBackground.CurrentBackgroundID = config.BackgroundID < 0 && config.BackgroundID > 7 ? BackgroundConfig.DefaultID : config.BackgroundID;
                    Logo.IsDoneMoonLordFight = config.IsDoneMoonlordFight;
                }

            }
            catch (Exception ex)
            {
                // 异常：创建默认JSON并使用默认ID
                CurrentFileName = DefaultFileName;
                string defaultFilePath = Path.Combine(FilePath, CurrentFileName);
                var defaultConfig = new BackgroundConfig { BackgroundID = BackgroundConfig.DefaultID };
                string jsonContent = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                File.WriteAllText(defaultFilePath, jsonContent);
                ManosabaBackground.CanChangeMenu = true;
                ManosabaBackground.CurrentBackgroundID = BackgroundConfig.DefaultID;
                Mod.Logger.Error($"Load Manosaba Mod background fail: {ex.Message}, using defualt");
            }
        }

        public override void Unload()
        {
            Instance = null;
        }
        public void ReimplementJson(bool isDownedML)
        {
            try
            {
                //拼接完整Json路径
                string jsonFilePath = Path.Combine(FilePath, CurrentFileName);
                BackgroundConfig config;
                //查看是否存在正常的配置文件，不存在则新建
                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    config = JsonConvert.DeserializeObject<BackgroundConfig>(jsonContent) ?? new BackgroundConfig();
                }
                else
                {
                    config = new BackgroundConfig();
                }
                //更新ID值覆盖现有内容
                config.IsDoneMoonlordFight = isDownedML;
                string newjsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(jsonFilePath, newjsonContent);
            }
            catch (Exception ex)
            {
                Mod.Logger.Error($"Rename/Load Manosaba Mod Down fail: {ex.Message}");
            }
        }
        /// <summary>
        /// 工具方法，用于外部调用
        /// </summary>
        /// <param name="newName"></param>
        public void ReimplementJson(int newID)
        {
            try
            {
                //拼接完整Json路径
                string jsonFilePath = Path.Combine(FilePath, CurrentFileName);
                BackgroundConfig config;
                //查看是否存在正常的配置文件，不存在则新建
                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    config = JsonConvert.DeserializeObject<BackgroundConfig>(jsonContent) ?? new BackgroundConfig();
                }
                else
                {
                    config = new BackgroundConfig();
                }
                //更新ID值覆盖现有内容
                config.BackgroundID = newID;
                string newjsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(jsonFilePath, newjsonContent);
            }
            catch (Exception ex)
            {
                Mod.Logger.Error($"Rename/Load Manosaba Mod background fail: {ex.Message}");
            }
        }
    }
}
