using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Destiny : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        static internal bool toggled = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            toggled = !toggled;

            CreateTimer(6.0f, () => { toggled = false; owner.SetSpell("Destiny", 3, true); });
            owner.SetSpell("WildCards", 3, true);
            CreateTimer(0.02f, () => { ((IObjAiBase)spell.CastInfo.Owner).GetSpell(3).SetCooldown(0f); });

            var x = GetChampionsInRange(owner.Position, 10000, true);
            foreach (var champ in x)
            {
                if (champ.Team != owner.Team)
                {
                    AddParticle(owner, champ, "DestinyEye.troy", champ.Position, lifetime: 6.0f, bone: "head", reqVision: false);
                    //IChampion xy = AddChampion(owner.Team, 1000, "Tryndamere", GetPointFromUnit(champ, -100));
                    //xy.SetStatus(StatusFlags.Targetable, false);
                    //
                    //var Champs = GetChampionsInRange(owner.Position, 50000, true);
                    //foreach (IChampion player in Champs)
                    //{
                    //    xy.SetStatus(StatusFlags.Targetable, false);
                    //    xy.SetInvisible((int)player.GetPlayerId(), xy, 0f, 0.1f);
                    //    xy.SetHealthbarVisibility((int)player.GetPlayerId(), xy, false);
                    //    CreateTimer(6.0f, () => { xy.TeleportTo(0, 0); });
                    //}
                }
            }
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
}