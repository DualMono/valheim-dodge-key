using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace DodgeKey
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class DodgeKey : BaseUnityPlugin
    {
        public const string PluginGUID = "DualMono";
        public const string PluginName = "DodgeKey";
        public const string PluginVersion = "0.0.1";

        private ConfigEntry<KeyCode> DodgeButtonKey;
        private ConfigEntry<InputManager.GamepadButton> DodgeInputGamepad;
        private ButtonConfig DodgeButton;

        private ConfigEntry<bool> InvertDodge;

        private void Awake()
        {
            CreateConfig();
            AddInputs();

            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("DodgeKey loaded");
        }

        private void Update()
        {
            if (ZInput.instance == null || Player.m_localPlayer == null)
            {
                return;
            }
            if (ZInput.GetButtonDown(DodgeButton.Name))
            {
                Vector3 lookDir = Player.m_localPlayer.m_lookDir;
                Vector3 moveDir = Player.m_localPlayer.m_moveDir;
                Vector3 dodgeDir;
                if (moveDir.magnitude > 0.1f)
                {
                    dodgeDir = moveDir;
                }
                else
                {
                    dodgeDir = InvertDodge.Value ? -lookDir : lookDir;
                }
                Player.m_localPlayer.Dodge(dodgeDir);
            }
        }

        private void CreateConfig()
        {
            DodgeButtonKey = Config.Bind("General", "DodgeKey", KeyCode.LeftAlt, "Key to dodge");
            DodgeInputGamepad = Config.Bind("General", "DodgeInputGamepad", InputManager.GamepadButton.ButtonSouth, "Gamepad button to dodge");
            InvertDodge = Config.Bind("General", "InvertDodge", true, "Invert dodge look direction");
        }

        private void AddInputs()
        {
            DodgeButton = new ButtonConfig
            {
                Name = "Dodge",
                Config = DodgeButtonKey,
                GamepadConfig = DodgeInputGamepad,
            };
            InputManager.Instance.AddButton(PluginGUID, DodgeButton);
        }

    }
}

