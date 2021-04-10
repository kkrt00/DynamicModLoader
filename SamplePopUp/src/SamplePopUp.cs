using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace SamplePopUp
{

    public class SampleGuiDialog : GuiDialog
    {
        public override string ToggleKeyCombinationCode => "samplePopUp";

        public SampleGuiDialog(ICoreClientAPI capi) : base(capi)
        {
            SetupDialog();
        }

        private void SetupDialog()
        {
            // Auto-sized dialog at the center of the screen
            ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);

            // Just a simple 300x300 pixel box
            ElementBounds textBounds = ElementBounds.Fixed(0, 40, 400, 120);

            // Background boundaries. Again, just make it fit it's child elements, then add the text as a child element
            ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            bgBounds.BothSizing = ElementSizing.FitToChildren;
            bgBounds.WithChildren(textBounds);

            SingleComposer = capi.Gui.CreateCompo("popUpDialog", dialogBounds)
                .AddShadedDialogBG(bgBounds)
                .AddDialogTitleBar("Success!", OnTitleBarCloseClicked)
                .AddStaticText(@"This is sample popup from the second mod. 
                You can edit this string in VS Code, rebuild (CTRL+SHIFT+B). 
                Then ingame just press U to load mod again 
                and then O to see updated string", CairoFont.WhiteDetailText(), textBounds)
              .Compose()
            ;
        }

        private void OnTitleBarCloseClicked()
        {
            TryClose();
        }
    }

    public class SampleGuiTextSystem : ModSystem
    {
        ICoreClientAPI capi;
        GuiDialog dialog;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Client;
        }
        
        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);

            dialog = new SampleGuiDialog(api);

            capi = api;
            capi.Input.RegisterHotKey("samplePopUp", "Pop up window with text", GlKeys.O, HotkeyType.GUIOrOtherControls);
            capi.Input.SetHotKeyHandler("samplePopUp", ToggleGui);
        }

        private bool ToggleGui(KeyCombination comb)
        {

            if (dialog.IsOpened()) dialog.TryClose();
            else dialog.TryOpen();       

            return true;
        }
    }
}