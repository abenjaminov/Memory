using System.Collections.Generic;

namespace _Scripts.Game
{
    public enum TileType
    {
        RegularTile,
        EmptyTile,
        BlackTile
    }
    public static class TileTypeConverter
    {
        private static readonly Dictionary<TileType, char> TileTypeMap = new()
        {
            { TileType.RegularTile, 'X' },
            { TileType.EmptyTile, 'E' },
            { TileType.BlackTile, 'B' }
        };

        public static char GetValue(TileType tileType)
        {
            return TileTypeMap[tileType];
        }
    }
}