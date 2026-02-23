using Newtonsoft.Json;
using SakurabaEmaMod.Menus.MainMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Menus.Managemments
{
    public class BackgroundConfig
    {
        public int BackgroundID { get; set; }
        public const int DefaultID = ManosabaMenuID.Ema;
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
                    var defaultConfig = new BackgroundConfig { BackgroundID = BackgroundConfig.DefaultID };
                    string jsonContent = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                    //写入内容
                    File.WriteAllText(defaultPath, jsonContent);
                    //标记允许开始修改背景
                    ManosabaBackground.CanChangeMenu = true;
                    ManosabaBackground.CurrentBackgroundID = BackgroundConfig.DefaultID;
                }
                else
                {
                    CurrentFileName = Path.GetFileName(jsonFileArray[0]);
                    string jsonFilePath = Path.Combine(FilePath, CurrentFileName);
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    var config = JsonConvert.DeserializeObject<BackgroundConfig>(jsonContent);
                    ManosabaBackground.CanChangeMenu = true;
                    ManosabaBackground.CurrentBackgroundID = config.BackgroundID < 0 && config.BackgroundID > 5 ? BackgroundConfig.DefaultID : config.BackgroundID;
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
        /// <summary>
        /// 工具方法，用于外部调用
        /// </summary>
        /// <param name="newName"></param>
        public void ReimplementJson(int newID)
        {
            try
            {
                //拼接完整Json路径
                string jsonFilePath= Path.Combine(FilePath, CurrentFileName);
                BackgroundConfig config;
                //查看是否存在正常的配置文件，不存在则新建
                if (File.Exists(jsonFilePath))
                {
                    string jsonContent= File.ReadAllText(jsonFilePath);
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
            catch(Exception ex)
            {
                Mod.Logger.Error($"Rename/Load Manosaba Mod background fail: {ex.Message}");
            }
        }
        public void LoadTextFileName()
        {
            try
            {
                //遍历当前文件夹下的所有txt文件
                string[] textFileArray = Directory.GetFiles(FilePath, "*.txt");
                //不存在txt文件，则创建一个txt文件
                if(textFileArray.Length ==0)
                {
                    CurrentFileName = DefaultFileName;
                    string filePath = Path.Combine(FilePath, CurrentFileName);
                    using (File.Create(filePath)) { }
                }
                else
                {
                    //如果有，取第一文件名称作为生效的名字。
                    CurrentFileName = Path.GetFileName(textFileArray[0]);
                }
            }
            //抓一个异常，使用默认值。然后输出报错
            catch (Exception ex)
            { 
                    CurrentFileName = DefaultFileName;
                string filePath = Path.Combine(FilePath, CurrentFileName);
                using (File.Create(filePath)) { }
                Mod.Logger.Error($"Load Manosaba Mod background fail: {ex.Message}, using defualt");
            }
        }
    }
}
