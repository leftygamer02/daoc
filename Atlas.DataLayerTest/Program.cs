using System;
using Atlas.DataLayer;
using Atlas.DataLayer.Models;

namespace Atlas.DataLayerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var db = new AtlasContext(eConnectionType.MYSQL, "server=localhost;port=3306;database=newatlas;user id=root;password=d4WqNsWsGTzCCea0Lb1H;treattinyasboolean=False");

            Console.Write("Deleting database...");
            db.Database.EnsureDeleted();
            Console.WriteLine("done!");

            Console.Write("Creating database...");
            db.Database.EnsureCreated();
            Console.WriteLine("done!");

            //db.Accounts.Add(new Account()
            //{
            //    Name = "GM",
            //    PrivLevel = 3,
            //    Realm = 0,
            //    CreateDate = DateTime.UtcNow,
            //    ModifyDate = DateTime.UtcNow,
            //    LastLogin = DateTime.UtcNow,
            //    LastClientVersion = "1125d",
            //});

            db.SaveChanges();
        }
    }
}
