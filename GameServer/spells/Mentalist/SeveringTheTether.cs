using System.Collections.Generic;
using DOL.AI.Brain;
using DOL.GS;
using DOL.GS.Effects;
using DOL.GS.Spells;

[SpellHandlerAttribute("SeveringTheTether")]
public class SeveringTheTetherSpellHandler : FearSpellHandler
{
    /// <summary>
    /// Dictionary to track the STT brains for each living target
    /// </summary>
    private readonly ReaderWriterDictionary<GamePet, SeveringTheTetherBrain> m_NPCSTTBrains = new ReaderWriterDictionary<GamePet, SeveringTheTetherBrain>();

    public override int CalculateSpellResistChance(GameLiving target)
    {
        return 0;
    }
    public override IList<GameLiving> SelectTargets(GameObject castTarget)
    {
        var list = new List<GameLiving>();
        var target = Caster.TargetObject;
        foreach (GameNPC npc in target.GetNPCsInRadius((ushort)Spell.Radius))
        {
            if (npc is GamePet && npc.Brain is ControlledNpcBrain && npc.Realm != Caster.Realm)//!(npc is NecromancerPet))
                list.Add(npc);
        }
        return list;
    }
    
    public override void ApplyEffectOnTarget(GameLiving target, double effectiveness)
    {
        var npcTarget = target as GamePet;
        if (npcTarget == null) return;

        base.ApplyEffectOnTarget(target, effectiveness);
    }
    
    public override void OnEffectStart(GameSpellEffect effect)
    {
        var npcTarget = effect.Owner as GamePet;
			
        var STTBrain = new SeveringTheTetherBrain();
        m_NPCSTTBrains.AddOrReplace(npcTarget, STTBrain);
			
        npcTarget.AddBrain(STTBrain);
        STTBrain.Think();

        npcTarget.Realm = Caster.Realm;
			
        base.OnEffectStart(effect);
    }
    
    public override int OnEffectExpires(GameSpellEffect effect, bool noMessages)
    {
        var npcTarget = effect.Owner as GamePet;

        SeveringTheTetherBrain STTBrain;
        if (m_NPCSTTBrains.TryRemove(npcTarget, out STTBrain))
        {
            npcTarget.RemoveBrain(STTBrain);
        }

        if(npcTarget.Brain == null)
            npcTarget.AddBrain(new StandardMobBrain());

        npcTarget.Realm = npcTarget.Owner.Realm;
        
        return base.OnEffectExpires(effect, noMessages);
    }

    public SeveringTheTetherSpellHandler(GameLiving caster, Spell spell, SpellLine line) : base(caster, spell, line) { }
}