using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class NasusR : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.Stats.Size.BaseValue = 1.5f;
            CreateTimer(15f, () => { owner.Stats.Size.BaseValue = 1.00f; });
            owner.Stats.HealthPoints.FlatBonus += 200f;
            CreateTimer(15f, () => { owner.Stats.HealthPoints.FlatBonus -= 200f; });
            //CreateTimer(15f, () => { owner.Stats.Size.BaseValue = 1.500f; });
            //CreateTimer(15.02f, () => { owner.Stats.Size.BaseValue = 1.475f; });
            //CreateTimer(15.04f, () => { owner.Stats.Size.BaseValue = 1.450f; });
            //CreateTimer(15.06f, () => { owner.Stats.Size.BaseValue = 1.425f; });
            //CreateTimer(15.08f, () => { owner.Stats.Size.BaseValue = 1.400f; });
            //CreateTimer(15.10f, () => { owner.Stats.Size.BaseValue = 1.375f; });
            //CreateTimer(15.12f, () => { owner.Stats.Size.BaseValue = 1.350f; });
            //CreateTimer(15.14f, () => { owner.Stats.Size.BaseValue = 1.325f; });
            //CreateTimer(15.16f, () => { owner.Stats.Size.BaseValue = 1.300f; });
            //CreateTimer(15.18f, () => { owner.Stats.Size.BaseValue = 1.275f; });
            //CreateTimer(15.20f, () => { owner.Stats.Size.BaseValue = 1.250f; });
            //CreateTimer(15.22f, () => { owner.Stats.Size.BaseValue = 1.225f; });
            //CreateTimer(15.24f, () => { owner.Stats.Size.BaseValue = 1.200f; });
            //CreateTimer(15.26f, () => { owner.Stats.Size.BaseValue = 1.175f; });
            //CreateTimer(15.28f, () => { owner.Stats.Size.BaseValue = 1.150f; });
            //CreateTimer(15.30f, () => { owner.Stats.Size.BaseValue = 1.125f; });
            //CreateTimer(15.32f, () => { owner.Stats.Size.BaseValue = 1.100f; });
            //CreateTimer(15.34f, () => { owner.Stats.Size.BaseValue = 1.075f; });
            //CreateTimer(15.36f, () => { owner.Stats.Size.BaseValue = 1.050f; });
            //CreateTimer(15.38f, () => { owner.Stats.Size.BaseValue = 1.025f; });
            //CreateTimer(15.40f, () => { owner.Stats.Size.BaseValue = 1.000f; });
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}