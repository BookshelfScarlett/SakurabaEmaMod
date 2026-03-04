using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace SakurabaEmaMod.Assets.Register
{
    public class ManosabaShader: ModSystem
    {        
        // 当未提供特定着色器时，用作基本绘图的默认值。此着色器仅渲染顶点颜色数据，无需修改。
        private const string ShaderPath = "SakurabaEmaMod/Assets/Effects/";
        internal const string ShaderPrefix = "ManosabaMod:";
        public static Effect TerrarRayLaser;
        public override void Load()
        {
            if (Main.dedServ)
                return;

            static Effect LoadShader(string path)
            {
                return Request<Effect>($"{ShaderPath}{path}", AssetRequestMode.ImmediateLoad).Value;
            }
            TerrarRayLaser = LoadShader(nameof(TerrarRayLaser));
            RegisterMiscShader(TerrarRayLaser, ToPassName(nameof(TerrarRayLaser)), nameof(TerrarRayLaser));
        }
        public override void Unload()
        {
            TerrarRayLaser = null;
        }
        public static string ToPassName(string oriShadername) => ShaderPrefix + oriShadername + "Pass";
        public static void RegisterMiscShader(Effect shader, string passName, string registrationName)
        {
            Ref<Effect> shaderPointer = new(shader);
            MiscShaderData passParamRegistration = new(shaderPointer, passName);
            GameShaders.Misc[$"{ShaderPrefix}:{registrationName}"] = passParamRegistration;
        }
    }
}
