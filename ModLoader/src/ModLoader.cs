using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using System.IO;

[assembly: ModInfo( "ModLoader",
	Description = "Dynamic Mod Loader",
	Website     = "https://github.com/kkrt00/VintageStory_ModLoader",
	Authors     = new []{ "kkrt00" } )]

namespace ModLoader
{
    public class ModLoaderClass : ModSystem
    {
        ICoreClientAPI capi;
        private static string modsDir = System.Reflection.Assembly.GetExecutingAssembly()
                                        .Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly()
                                        .Location.IndexOf("ModLoader\\bin\\Debug\\net452\\ModLoader.dll"));

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Client;
        }
        
        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);

            capi = api;
            capi.Input.RegisterHotKey("modLoader", "Load a mod", GlKeys.U, HotkeyType.GUIOrOtherControls);
            capi.Input.SetHotKeyHandler("modLoader", ModLoaderFunction);
        }

        private bool ModLoaderFunction(KeyCombination comb)
        {
            var assemblyBytes = File.ReadAllBytes(modsDir + @"SamplePopUp\bin\Debug\net452\SamplePopUp.dll");  
            var assemblyTest = Assembly.Load(assemblyBytes);

            var classType = assemblyTest.GetType("SamplePopUp.SampleGuiTextSystem");
            var c = System.Activator.CreateInstance(classType);

            classType.InvokeMember("StartClientSide", BindingFlags.InvokeMethod, null, c, new object[] { capi });

            return true;
        }

    }
    
}