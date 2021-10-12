using System;
using System.Collections;
using DOL.GS.PacketHandler;
using System.Collections.Generic;
using Atlas.DataLayer.Models;
using System.Linq;

namespace DOL.GS.Scripts
{
    public class Herald : GameNPC
    {

        public Herald() : base() { }
        public override bool AddToWorld()
        {
            Model = 2026;
            Name = "Faelyn";
            GuildName = "Atlas Herald";
            Level = 50;
            Size = 60;
            Flags |= eFlags.PEACE;
            base.AddToWorld();
            return true;
        }

        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
                return false;
            
            TurnTo(player, 500);

            var chars = GameServer.Database.Characters
                                           .Where(x => x.RealmPoints > 0 && x.RealmPoints < 70000000)
                                           .OrderByDescending(x => x.RealmPoints)
                                           .Take(25)
                                           .ToList();

            List<string> list = new List<string>();
            
            list.Add("Top 25 Highest Realm Points:\n\n");
            int count = 1;
            foreach (var chr in chars)
            {
                var realm = "";
                
                switch (chr.Realm)
                {
                    case 1:
                        realm = "Alb";
                        break;
                    case 2:
                        realm = "Mid";
                        break;
                    case 3:
                        realm = "Hib";
                        break;
                }

                string str = "#" + count + ": " + chr.Name + " (" + realm + ") - " + chr.RealmPoints + " realm points\n";
                count++;
                list.Add(str);
            }

            player.Out.SendCustomTextWindow("Realm Point Herald", list);

            return true;
        }
    }
}