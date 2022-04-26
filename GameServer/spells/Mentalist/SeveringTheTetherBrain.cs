using DOL.GS;

namespace DOL.AI.Brain
{
	public class SeveringTheTetherBrain : StandardMobBrain
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
			var target = Body.ControlledBrain.Owner;
			if (target == null)
				return;
			Body.StartAttack(target);
		}
	}
} 
