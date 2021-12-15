using System;
using System.Reflection;
using DOL.GS.PacketHandler;
using DOL.Database;
using DOL.Events;
using log4net;
using DOL.AI.Brain;



namespace DOL.GS.Scripts
{
    public class OFMerchant : GameMerchant
    {
        #region Constructor

        public OFMerchant()
            : base()
        {
            SetOwnBrain(new BlankBrain());
        }

        #endregion Constructor

        #region AddToWorld

        public override bool AddToWorld()
        {
            GuildName = "Merchant";
            Level = 75;
            Flags |= eFlags.PEACE;

            switch (Realm)
            {
                case eRealm.Albion:
                    Name = "Sall Fadri";
                    Model = 61;
                    TradeItems = new MerchantTradeItems("OFMerchant_Alb");
                    break;
                case eRealm.Midgard:
                    Name = "Gwulla";
                    Model = 215;
                    TradeItems = new MerchantTradeItems("OFMerchant_Mid");
                    break;
                case eRealm.Hibernia:
                    Name = "Merchant";
                    Model = 342;
                    TradeItems = new MerchantTradeItems("OFMerchant_Hib");
                    break;
                default:
                    break;
            }
         
            MaxSpeedBase = 0;


            return base.AddToWorld();
        }

        #endregion AddToWorld

    }
    
    public class OFMerchantHome : GameMerchant
    {
        #region Constructor

        public OFMerchantHome()
            : base()
        {
            SetOwnBrain(new BlankBrain());
        }

        #endregion Constructor

        #region AddToWorld

        public override bool AddToWorld()
        {

            GuildName = "Medallion Merchant";
            Level = 75;
            Flags |= eFlags.PEACE;

            switch (Realm)
            {
                case eRealm.Albion:
                    Name = "Sall Fadri";
                    Model = 61;
                    TradeItems = new MerchantTradeItems("OFMerchant_Alb");
                    break;
                case eRealm.Midgard:
                    Name = "Gwulla";
                    Model = 215;
                    TradeItems = new MerchantTradeItems("OFMerchant_Mid");
                    break;
                case eRealm.Hibernia:
                    Name = "Araisa";
                    Model = 342;
                    TradeItems = new MerchantTradeItems("OFMerchant_Hib");
                    break;
            }
         
            MaxSpeedBase = 0;
            return base.AddToWorld();
        }

        #endregion AddToWorld


        public override bool Interact(GamePlayer player)
        {
            player.Out.SendMerchantWindow(TradeItems, eMerchantWindowType.Normal);
            return true;
        }
    }  
 }

