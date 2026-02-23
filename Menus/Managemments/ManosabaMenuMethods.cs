using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.ID;

namespace SakurabaEmaMod.Menus.Managemments
{
    public static class ManosabaMenuMethods
    {
        public static void ChangeMenu(int TargetMenuID)
        {
            ManosabaMenuUpdate.ToOtherMenu = true;
            ManosabaMenuUpdate.NextMenuID = TargetMenuID;
        }
        public static void OpenWorkshop()
        {
            ManosabaMenuUpdate.ToOtherMenu = true;
            ManosabaMenuUpdate.NextMenuID = MenuID.FancyUI;
            ManosabaMenuUpdate.OnChangeToTargetMenuID.Add(delegate
            {
                UIWorkshopHub workshopHub = new UIWorkshopHub(null);
                workshopHub.EnterHub();
                Main.MenuUI.SetState(workshopHub);
            });
        }
        public static void OpenAchievements()
        {
            ManosabaMenuUpdate.ToOtherMenu = true;
            ManosabaMenuUpdate.NextMenuID = MenuID.FancyUI;
            ManosabaMenuUpdate.OnChangeToTargetMenuID.Add(delegate
            {
                Main.menuMode = MenuID.FancyUI;
                Main.MenuUI.SetState(Main.AchievementsMenu);
            });
        }
    }
}
