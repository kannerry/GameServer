using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AbsoluteZero : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        static internal IParticle xy;
        static internal bool pressed = false;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            AbsoluteZero2.popped = false;
            pressed = true;
            owner.SetSpell("AbsoluteZero2", 3, true);
            xy = AddParticle(owner, owner, "AbsoluteZero2_green_cas.troy", owner.Position, lifetime: 3.0f);
            CreateTimer(3.0f, () => 
            {
                if(AbsoluteZero2.popped != true)
                {
                    owner.SetSpell("AbsoluteZero", 3, true);
                    float tempfloat = 0;
                    tempfloat += AbsoluteZero2.i;
                    AbsoluteZero2.i = 0;
                    LogDebug(tempfloat.ToString());
                    AddParticle(owner, owner, "AbsoluteZero_nova.troy", owner.Position, lifetime: 4.0f);
                    var x = GetUnitsInRange(owner.Position, 1000, true);
                    foreach (var unit in x)
                    {
                        if (unit.Team != owner.Team)
                        {
                            unit.TakeDamage(owner, tempfloat / 10, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_RAW, false);
                        }
                    }
                    xy.SetToRemove();
                    owner.StopAnimation("Spell4");
                    ForceMovement(owner, "run", new Vector2(owner.Position.X, owner.Position.Y + 5), 1000, 0, 0, 0, 0);
                    pressed = false;
                    //AbsoluteZero2.popped = true;
                }
            });
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

    public class AbsoluteZero2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        static internal bool popped = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            float tempfloat = 0;
            tempfloat += i;
            i = 0;
            LogDebug(tempfloat.ToString());
            AbsoluteZero.pressed = false;
            popped = true;
            var x = GetUnitsInRange(owner.Position, 1000, true);
            foreach(var unit in x)
            {
                if(unit.Team != owner.Team)
                {
                    unit.TakeDamage(owner, tempfloat / 10, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_RAW, false);
                }
            }
            AbsoluteZero.xy.SetToRemove();
            owner.StopAnimation("Spell4");
            owner.SetSpell("AbsoluteZero", 3, true);
            AddParticle(owner, owner, "AbsoluteZero_nova.troy", owner.Position, lifetime: 4.0f);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
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
        static internal float i = 0;
        public void OnUpdate(float diff)
        {
            if(AbsoluteZero.pressed == true)
            {
                i += diff;
                //LogDebug(i.ToString());
            }
            if (i > 6100)
            {
                i = 0;
            }
        }
    }
}