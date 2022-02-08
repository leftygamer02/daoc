using System;
using System.Collections;
using DOL.AI.Brain;
using DOL.Events;
using DOL.Database;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOL.GS.Styles;
using DOL.GS.SkillHandler;
using DOL.GS.Spells;

namespace DOL.GS
{
    public class FakeTidalAnglator : GameNPC
    {
        public FakeTidalAnglator() : base() { }

		public override bool AddToWorld()
		{
			int rand = Util.Random(1, 2);
			switch(rand)
            {
				case 1:
                    {
						Model = 764;
						Name = "marine shriller";
						Size = (byte)Util.Random(48, 54);
						Level = (byte)Util.Random(39, 42);
						Faction = FactionMgr.GetFactionByID(106);
						TetherRange = 4500;
						MaxDistance = 2000;
						Gender = eGender.Neutral;
						RoamingRange = 500;
						MaxSpeedBase = 300;

						FakeTidalAnglatorBrain sBrain = new FakeTidalAnglatorBrain();
						SetOwnBrain(sBrain);
						sBrain.AggroLevel = 0;
						sBrain.AggroRange = 400;
						base.AddToWorld();
					}break;
				case 2:
                    {
						Model = 926;
						Name = "sanidon";
						Size = (byte)Util.Random(73, 78);
						Level = (byte)Util.Random(39, 43);
						Faction = FactionMgr.GetFactionByID(91);
						TetherRange = 4500;
						MaxDistance = 2000;
						Gender = eGender.Neutral;
						RoamingRange = 500;
						MaxSpeedBase = 300;

						FakeTidalAnglatorBrain sBrain = new FakeTidalAnglatorBrain();
						SetOwnBrain(sBrain);
						sBrain.AggroLevel = 0;
						sBrain.AggroRange = 400;
						base.AddToWorld();
					}
					break;
            }
			return true;
		}
		public override void Die(GameObject killer)
		{
			if (killer != null)
            {
				if(killer is GamePlayer || killer is GamePet)
                {
					GamePlayer player = killer as GamePlayer;
					long expCap = (long)(GameServer.ServerRules.GetExperienceForLiving(player.Level) * ServerProperties.Properties.XP_HARDCAP_PERCENT / 80);
					long campCap = (long)(GameServer.ServerRules.GetExperienceForLiving(player.Level) * ServerProperties.Properties.XP_HARDCAP_PERCENT / 100);
					long grpCap = (long)(GameServer.ServerRules.GetExperienceForLiving(player.Level) * ServerProperties.Properties.XP_HARDCAP_PERCENT / 50);
					long atlasbonusCap = (long)(GameServer.ServerRules.GetExperienceForLiving(player.Level) * ServerProperties.Properties.XP_HARDCAP_PERCENT / 70);
					player.GainExperience(eXPSource.NPC, expCap, campCap, grpCap, 0, atlasbonusCap, true);
					Spawn();
                }
            }
			base.Die(killer);
		}
		public void Spawn()
		{
			TidalAnglator Add = new TidalAnglator();
			Add.X = this.X;
			Add.Y = this.Y;
			Add.Z = this.Z;
			Add.CurrentRegion = this.CurrentRegion;
			Add.Heading = this.Heading;
			Add.AddToWorld();
		}
		
	}
	public class TidalAnglator : GameNPC
	{
		public TidalAnglator() : base() { }

		public override bool AddToWorld()
		{
			Model = 950;
			Name = "tidal anglator";
			Size = (byte)Util.Random(22, 28);
			Level = (byte)Util.Random(40, 43);
			TetherRange = 3000;
			MaxDistance = 2000;
			Gender = eGender.Neutral;
			MaxSpeedBase = 191;
			RespawnInterval = -1;
			RoamingRange = 300;

			TidalAnglatorBrain sBrain = new TidalAnglatorBrain();
			SetOwnBrain(sBrain);
			sBrain.AggroLevel = 100;
			sBrain.AggroRange = 400;
			base.AddToWorld();
			return true;
		}
	}
}


namespace DOL.AI.Brain
{
	public class FakeTidalAnglatorBrain : StandardMobBrain
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public FakeTidalAnglatorBrain() : base() { }

		public override void Think()
		{
			if(Body.IsAlive==true)
            {
				Body.CurrentSpeed = 200;//for roaming speed
            }
			base.Think();
		}
		public override void Notify(DOL.Events.DOLEvent e, object sender, EventArgs args)
        {
			base.Notify(e, sender, args);
			if (sender == Body)
            {
				FakeTidalAnglator anglator = sender as FakeTidalAnglator;
				if (e == GameObjectEvent.TakeDamage)
                {
					GameObject source = (args as TakeDamageEventArgs).DamageSource;
					anglator.Die(source);
				}
			}
		}
	}
}
namespace DOL.AI.Brain
{
	public class TidalAnglatorBrain : StandardMobBrain
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public TidalAnglatorBrain() : base()
		{
			AggroLevel = 100;
			AggroRange = 450;
		}

		public override void Think()
		{
			base.Think();
		}
	}
}