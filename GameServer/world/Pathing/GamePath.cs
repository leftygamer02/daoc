using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.GS
{
    public class GamePath
    {
        public GamePath(string s, object region)
        {
            _name = s;
        }

        public bool HasLavaEffect { get; set; }

        private List<GameStaticItem> _gameObjects = new List<GameStaticItem>();
        private string _name;

        public void Hide()
        {
            lock (_gameObjects)
            {
                foreach (var obj in _gameObjects)
                {
                    obj.Delete();
                }

                _gameObjects.Clear();
            }
        }

        public class MarkerModel
        {
            public static ushort Green = 408;
            public static ushort Red = 408;
            public static ushort Blue = 408;
            public static ushort Yellow = 408;
        }

        public void Append(GameLocation gameLocation, short ownerMaxSpeed, ushort model)
        {
            var obj = new GameStaticItem() { Model = model, CurrentRegionID = gameLocation.RegionID, Position = gameLocation.Position, Name = _name };

            lock (_gameObjects)
                _gameObjects.Add(obj);
        }

        public void Show()
        {
            lock (_gameObjects)
            {
                foreach (var gameObject in _gameObjects)
                {
                    gameObject.AddToWorld();
                }
            }
        }
    }
}
