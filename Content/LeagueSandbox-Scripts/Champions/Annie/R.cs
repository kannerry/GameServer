using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class InfernalGuardian : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public float petTimeAlive = 0.00f;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellCast.AddListener(this, spell, ExecuteSpell);
        }

        public void ExecuteSpell(ISpell spell)
        {
            AddBuff("AnnieStun", int.MaxValue, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var castrange = spell.GetCurrentCastRange();
            var apbonus = spell.CastInfo.Owner.Stats.AbilityPower.Total;
            var ownerPos = owner.Position;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            if (Extensions.IsVectorWithinRange(ownerPos, spellPos, castrange))
            {
                IMinion m = AddMinion((IChampion)owner, "AnnieTibbers", "AnnieTibbers", spellPos);
                var attackrange = m.Stats.Range.Total;
                m.Stats.MoveSpeed.BaseValue = 450.5f;
                var units = GetUnitsInRange(m.Position, attackrange, true);
                foreach (var value in units)
                {
                    if (owner.Team != value.Team && value is IAttackableUnit && !(value is IBaseTurret) && !(value is IObjAnimatedBuilding))
                    {
                        m.SetTargetUnit(value);
                        if (owner.GetBuffWithName("AnnieStun").StackCount.Equals(5))
                        {
                            LogDebug("R 4STACK");
                            AddBuff("Stun", 1.5f, 1, spell, value, owner);
                            CreateTimer(0.5f, () => { owner.RemoveBuffsWithName("AnnieStun"); });
                        }
                        for (petTimeAlive = 0.0f; petTimeAlive < 0.1f; petTimeAlive += 1.0f)
                        {
                            {
                                if (!value.IsDead && !m.IsDead)
                                {
                                    value.TakeDamage(owner, apbonus, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
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