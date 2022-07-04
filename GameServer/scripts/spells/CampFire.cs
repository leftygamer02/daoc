using DOL.GS.PacketHandler;
using DOL.GS.Effects;
using DOL.Database;
using DOL.AI.Brain;

namespace DOL.GS.Spells
{
    public class FireCampSpellHandler : DoTSpellHandler
    {
        protected GameFont font;
        protected bool ApplyOnNPC = false;
        protected bool ApplyOnCombat = false;
        protected bool Friendly = false;
        protected ushort sRadius = 350;

        public override void ApplyEffectOnTarget(GameLiving target, double effectiveness)
        {
            GameSpellEffect neweffect = CreateSpellEffect(target, effectiveness);
            if (font != null)
            {
                font.AddToWorld();
                neweffect.Start(font);
            }
        }

        public override void OnEffectPulse(GameSpellEffect effect)
        {
            if (font == null || font.ObjectState == GameObject.eObjectState.Deleted)
            {
                effect.Cancel(false);
                return;
            }
            foreach (GamePlayer player in font.GetPlayersInRadius(sRadius))
            {
                Friendly = player.Realm == font.Realm;
                if (Friendly && player.IsAlive && !player.InCombat)
                {
                    Endu(player);
                    Mana(player);
                    Life(player);
                }
            }
            foreach (GameNPC npc in font.GetNPCsInRadius(sRadius))
            {
                Friendly = npc.Realm == font.Realm;
                if (Friendly && npc.IsAlive && !npc.InCombat)
                {
                    Endu(npc);
                    Mana(npc);
                    Life(npc);
                }
            }
        }

        /// <summary>
        /// Uses percent of damage to heal the caster
        /// </summary>
        public virtual void Endu(GameLiving target)
        {
            if (target == null) 
                return;
            if (target is GamePlayer == false)
                return;

            int er = 0;

            if (target.Endurance != target.MaxEndurance)
            {
                er = target.MaxEndurance / 20;
            }
            if (er > 0)
            {
                target.ChangeEndurance(target, eEnduranceChangeType.Regenerate, er);
                MessageToLiving(target, "You gain " + er.ToString() + " endurance points from campfire.", eChatType.CT_Spell);
            }

        }

        /// <summary>
        /// Uses percent of damage to heal the caster
        /// </summary>
        public virtual void Mana(GameLiving target)
        {
            if (target == null) 
                return;
            if (target is GamePlayer == false) 
                return;

            int mr = 0;

            if (target.MaxMana != target.Mana)
            {
                mr = target.MaxMana / 20;
            }
            if (mr > 0)
            {
                target.ChangeMana(target, eManaChangeType.Regenerate, mr);
                MessageToLiving(target, "You gain " + mr.ToString() + " power points from campfire.", eChatType.CT_Spell);
            }

        }

        /// <summary>
        /// Uses percent of damage to heal the caster
        /// </summary>
        public virtual void Life(GameLiving target)
        {
            if (target == null) return;

            int hr = 0;

            if (target.Health != target.MaxHealth)
            {
                hr = target.MaxHealth / 20;
            }
            if (target.IsDiseased)
            {
                MessageToCaster("You are diseased.", eChatType.CT_SpellResisted);
                //MessageToCaster("Vous êtes malade.", eChatType.CT_SpellResisted);
                hr >>= 1;
            }
            if (hr > 0)
            {
                target.ChangeHealth(target, eHealthChangeType.Regenerate, hr);
                GamePlayer owner = null;
                if (target is GameNPC && (target as GameNPC).Brain is IControlledBrain)
                    owner = ((target as GameNPC).Brain as IControlledBrain).GetPlayerOwner();
                if (owner != null)
                    owner.Out.SendMessage("Your " + target.Name + " gain " + hr + " health points from campfire.", eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
                else
                    MessageToLiving(target, "You gain " + hr + " health points from campfire.", eChatType.CT_Spell);
            }

        }
        /// <summary>
        /// Tries to start a spell attached to an item (/use with at least 1 charge)
        /// Override this to do a CheckBeginCast if needed, otherwise spell will always cast and item will be used.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="item"></param>
        public override bool StartSpell(GameLiving target, InventoryItem item)
        {
            m_spellItem = item;

            if (m_caster is GamePlayer == false)
                return false;

            if (m_caster.CurrentZone.IsOF || m_caster.CurrentZone.IsBG|| m_caster.CurrentRegion.IsRvR)
            {
                (m_caster as GamePlayer).Out.SendMessage("You can't use campfire in this zone!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                return false;
            }
            foreach (GameNPC font in m_caster.GetNPCsInRadius((ushort)700))
            {
                if (font != null && font.Realm == m_caster.Realm && font.Model == 3460)
                {
                    (m_caster as GamePlayer).Out.SendMessage("There's already a campfire you can benefits here!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                    return false;
                }
            }
            return StartSpell(target);
        }

        public override int OnEffectExpires(GameSpellEffect effect, bool noMessages)
        {
            if (font != null) font.Delete();
            lock (effect.Owner.EffectList)
            {
                effect.Owner.EffectList.Remove(effect);
            }
            return base.OnEffectExpires(effect, noMessages);
        }

        // constructor
        public FireCampSpellHandler(GameLiving caster, Spell spell, SpellLine line) : base(caster, spell, line) { }
    }

    /// <summary>
    /// Font for FireCamp
    /// </summary>
    [SpellHandlerAttribute("Campfire")]
    public class CampfireSpellHandler : FireCampSpellHandler
    {
        public CampfireSpellHandler(GameLiving caster, Spell spell, SpellLine line)
            : base(caster, spell, line)
        {
            ApplyOnNPC = true;
            font = new GameFont();
            font.Model = 3460;
            font.Name = spell.Name;
            font.Realm = caster.Realm;
            font.X = caster.X;
            font.Y = caster.Y;
            font.Z = caster.Z;
            font.CurrentRegionID = caster.CurrentRegionID;
            font.Heading = caster.Heading;
            font.Owner = (GamePlayer)caster;
        }

        /// <summary>
        /// When an applied effect expires.
        /// Duration spells only.
        /// </summary>
        /// <param name="effect">The expired effect</param>
        /// <param name="noMessages">true, when no messages should be sent to player and surrounding</param>
        /// <returns>immunity duration in milliseconds</returns>
        public override int OnEffectExpires(GameSpellEffect effect, bool noMessages)
        {
            base.OnEffectExpires(effect, noMessages);
            if (!noMessages)
            {
            }
            return 0;
        }
    }
}