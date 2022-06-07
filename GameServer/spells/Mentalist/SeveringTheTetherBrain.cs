using System;
using DOL.GS;

namespace DOL.AI.Brain
{
	public class SeveringTheTetherBrain : StandardMobBrain, IControlledBrain
	{
		/// <summary>
		/// Fixed thinking Interval for Fleeing
		/// </summary>
		public override int ThinkInterval {
			get {
				return 1500;
			}
		}

		/// <summary>
		/// Flee from Players on Brain Think
		/// </summary>
		public override void Think()
		{
			AttackOwner();
		}

		///<summary>
		/// Calculate flee target.
		/// </summary>
		///<param name="target">The target to flee.</param>
		protected virtual void CalculateFleeTarget(GameLiving target)
		{
			ushort TargetAngle = (ushort)((Body.GetHeading(target) + 2048) % 4096);

            Point2D fleePoint = Body.GetPointFromHeading(TargetAngle, 300);
			Body.StopFollowing();
			Body.StopAttack();
			Body.WalkTo(fleePoint.X, fleePoint.Y, Body.Z, Body.MaxSpeed);
		}

		protected virtual void AttackOwner()
		{
			//var target = Body?.ControlledBrain?.Owner;
			var target = GetLivingOwner();
			Console.WriteLine($"Target {target}");
			if (target == null)
				return;
			Body.StartAttack(target);
		}

		public eWalkState WalkState { get; }
		public eAggressionState AggressionState { get { return eAggressionState.Aggressive; } set { } }
		public GameLiving Owner { get; set; }
		public void Attack(GameObject target)
		{
			
		}

		public void Follow(GameObject target)
		{
			
		}

		public void FollowOwner()
		{
			
		}

		public void Stay()
		{
			
		}

		public void ComeHere()
		{
			
		}

		public void Goto(GameObject target)
		{
			
		}

		public void UpdatePetWindow()
		{
			
		}
		public GamePlayer GetPlayerOwner(){ return Owner as GamePlayer; }
		
		public virtual GameNPC GetNPCOwner()
		{
			if (!(Owner is GameNPC))
				return null;

			GameNPC owner = Owner as GameNPC;

			int i = 0;
			while (owner != null)
			{
				i++;
				if (i > 50)
				{
					break;
				}
				if (owner.Brain is ForestheartAmbusherBrain)
				{
					if ((owner.Brain as ForestheartAmbusherBrain).Owner is GamePlayer)
						return null;
					else
						owner = (owner.Brain as ForestheartAmbusherBrain).Owner as GameNPC;
				}
				else
					break;
			}
			return owner;
		}
		public virtual GameLiving GetLivingOwner()
		{
			GamePlayer player = GetPlayerOwner();
			if (player != null)
				return player;

			GameNPC npc = GetNPCOwner();
			if (npc != null)
				return npc;

			return null;
		}

		public void SetAggressionState(eAggressionState state)
		{
		}
		
		

		public bool IsMainPet { get; set; }
	}
} 
