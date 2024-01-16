using HarmonyLib;
using UnityEngine;
using Logger = BepInEx.Logging.Logger;
using System;
using BepInEx.Logging;

namespace Lower_Player_Resistances;

public class DamageModifierClass
{
    [HarmonyPatch(typeof(Player), nameof(Player.Awake))]
    static class DamagePatch
    {
        
        public enum DamageModifier
        {
            Normal,
            Resistant,
            Weak,
            Immune,
            Ignore,
            VeryResistant,
            VeryWeak
        }
        public enum DamageType
        {
            Blunt = 1,
            Slash = 2,
            Pierce = 4,
            Chop = 8,
            Pickaxe = 0x10,
            Fire = 0x20,
            Frost = 0x40,
            Lightning = 0x80,
            Poison = 0x100,
            Spirit = 0x200,
            Physical = 0x1F,
            Elemental = 0xE0
        }
        
        public struct DamageModPair
        {
            public DamageType m_type;

            public DamageModifier m_modifier;
        }
        
        static void Postfix(Player __instance)
        {
            if (__instance == null)
            {
                Lower_Player_ResistancesPlugin.Lower_Player_ResistancesLogger.Log(LogLevel.All,"Player instance is null in DamagePatch");
                return;
            }

            try
            {
                var modifierList = __instance.m_damageModifiers;
                
                    // Assign damage modifiers based on the configuration
                    modifierList.m_blunt = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.BluntConfig.Value);
                    modifierList.m_slash = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.SlashConfig.Value);
                    modifierList.m_pierce = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.PierceConfig.Value);
                    modifierList.m_fire = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.FireConfig.Value);
                    modifierList.m_frost = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.FrostConfig.Value);
                    modifierList.m_lightning = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.LightningConfig.Value);
                    modifierList.m_poison = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.PoisonConfig.Value);
                    modifierList.m_spirit = Lower_Player_ResistancesPlugin.GetDamageModifier(Lower_Player_ResistancesPlugin.SpiritConfig.Value);

                    
                    // Reassign the modified struct back to the player instance
                    __instance.m_damageModifiers = modifierList;
                    
                Lower_Player_ResistancesPlugin.Lower_Player_ResistancesLogger.Log(LogLevel.All, "Player's resistances have been lowered to weak");
            }
            catch (Exception ex)
            {
                Lower_Player_ResistancesPlugin.Lower_Player_ResistancesLogger.LogError($"Error in DamagePatch: {ex.Message}");
            }
        }
    }
}