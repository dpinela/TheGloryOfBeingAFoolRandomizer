using IO = System.IO;
using Collections = System.Collections.Generic;
using PMActions = HutongGames.PlayMaker.Actions;
using UE = UnityEngine;
using MAPI = Modding;
using IC = ItemChanger;
using ICSpecLoc = ItemChanger.Locations.SpecialLocations;
using RC = RandomizerCore;
using Rando = RandomizerMod;
using RandoData = RandomizerMod.RandomizerData;
using MC = MenuChanger;

namespace TheGloryOfBeingAFoolRandomizer
{
    public class TheGloryOfBeingAFoolRandomizer : MAPI.Mod, MAPI.IGlobalSettings<ModSettings>
    {
        public override string GetVersion() => "1.0";

        private const string Colosseum3LocationName = "The_Glory_of_Being_a_Fool-Colosseum";

        public override Collections.List<(string, string)> GetPreloadNames() => new()
        {
            (IC.SceneNames.Room_Colosseum_Spectate, "Crowd Audio")
        };

        public override void Initialize(Collections.Dictionary<string, Collections.Dictionary<string, UE.GameObject>> preloads)
        {
            TheGloryOfBeingAFool.Cheer = (UE.AudioClip)preloads[IC.SceneNames.Room_Colosseum_Spectate]["Crowd Audio"].LocateMyFSM("Control")
                .FsmStates.FirstOrDefault(s => s.Name == "Cheer")
                .Actions.OfType<PMActions.AudioPlayerOneShotSingle>().FirstOrDefault()
                .audioClip.Value;
            IC.Finder.DefineCustomItem(new TheGloryOfBeingAFool());
            IC.Finder.DefineCustomLocation(new ICSpecLoc.ColosseumLocation()
            {
                name = Colosseum3LocationName,
                sceneName = IC.SceneNames.Room_Colosseum_Gold,
                objectName = "Shiny Item",
                fsmParent = "Colosseum Manager",
                fsmName = "Geo Pool",
                fsmVariable = "Shiny Obj",
                flingType = IC.FlingType.Everywhere,
                tags = new() { LocationMetadataTag() }
            });

            Rando.RC.RequestBuilder.OnUpdate.Subscribe(30, ApplyPreviewSetting);
            Rando.RC.RequestBuilder.OnUpdate.Subscribe(50, AddGloryToPool);
            Rando.RC.RCData.RuntimeLogicOverride.Subscribe(50, HookLogic);

            Rando.Menu.RandomizerMenuAPI.AddMenuPage(_ => {}, BuildConnectionMenuButton);
            Rando.Logging.SettingsLog.AfterLogSettings += LogRandoSettings;

            if (MAPI.ModHooks.GetMod("RandoSettingsManager") != null)
            {
                HookRSM();
            }
        }

        private static IC.Tag LocationMetadataTag()
        {
            var t = new IC.Tags.InteropTag();
            t.Message = "RandoSupplementalMetadata";
            t.Properties["ModSource"] = nameof(TheGloryOfBeingAFoolRandomizer);
            t.Properties["PinSpriteKey"] = "Lore";
            t.Properties["MapLocations"] = new (string scene, float x, float y)[]
            {
                (IC.SceneNames.Deepnest_East_09, 2.7f, 0.0f)
            };
            return t;
        }

        private ModSettings settings = new();

        public void OnLoadGlobal(ModSettings s)
        {
            settings = s;
        }

        public ModSettings OnSaveGlobal() => settings;

        private void AddGloryToPool(Rando.RC.RequestBuilder rb)
        {
            if (settings.Enabled)
            {
                rb.AddItemByName(TheGloryOfBeingAFool.Name);
                rb.AddLocationByName(Colosseum3LocationName);
            }
        }

        private void ApplyPreviewSetting(Rando.RC.RequestBuilder rb)
        {
            if (settings.Enabled && !rb.gs.LongLocationSettings.ColosseumPreview)
            {
                rb.EditLocationRequest(Colosseum3LocationName, info =>
                {
                    info.onPlacementFetch += (_, _, placement) =>
                        placement.GetOrAddTag<IC.Tags.DisableItemPreviewTag>();
                });
            }
        }

        private void HookLogic(Rando.Settings.GenerationSettings gs, RC.Logic.LogicManagerBuilder lmb)
        {
            if (settings.Enabled)
            {
                var term = lmb.GetOrAddTerm(TheGloryOfBeingAFool.Name);
                lmb.AddItem(
                    new RC.LogicItems.SingleItem(TheGloryOfBeingAFool.Name,
                    new(term, 1)));
                lmb.LogicLookup[Colosseum3LocationName] = lmb.LogicLookup[IC.LocationNames.Pale_Ore_Colosseum];
            }
        }

        private bool BuildConnectionMenuButton(MC.MenuPage landingPage, out MC.MenuElements.SmallButton settingsButton)
        {
            var button = new MC.MenuElements.SmallButton(landingPage, "T.G.O.B.A.F. Rando");

            void UpdateButtonColor()
            {
                button.Text.color = settings.Enabled ? MC.Colors.TRUE_COLOR : MC.Colors.DEFAULT_COLOR;
            }

            UpdateButtonColor();
            button.OnClick += () =>
            {
                settings.Enabled = !settings.Enabled;
                UpdateButtonColor();
            };
            settingsButton = button;
            return true;
        }

        private void LogRandoSettings(Rando.Logging.LogArguments args, IO.TextWriter w)
        {
            w.WriteLine("Logging TheGloryOfBeingAFoolRandomizer settings:");
            w.WriteLine(RandoData.JsonUtil.Serialize(settings));
        }

        private void HookRSM()
        {
            RandoSettingsManager.RandoSettingsManagerMod.Instance.RegisterConnection(
                new RandoSettingsManagerProxy()
                {
                    getter = () => settings,
                    setter = rs => settings = rs
                }
            );
        }
    }
}
