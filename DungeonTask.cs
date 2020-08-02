using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class DungeonTask
	{
		public static MoveDirection[] FindShortestPath(Map map)
		{
			var chestsAndEnd = map.Chests.Append(map.Exit).ToArray();
			var wayPlayer = BfsTask.FindPaths(map, map.InitialPosition, chestsAndEnd);
			var wayPlayerToExit = wayPlayer.Where(u => u.Value == map.Exit);
			if (wayPlayerToExit.Count() == 0) return new MoveDirection[0];

			if (wayPlayerToExit.Any(u => map.Chests.Contains(u.Value)))
				return ConvertPointToMove(wayPlayerToExit.Last());

			var wayExitToChests = BfsTask.FindPaths(map, map.Exit, map.Chests);
			var ways = wayPlayer.Join(wayExitToChests, e => e.Value, o => o.Value, (e, o) => Tuple.Create(e, o));
			var partsWay = ways.OrderBy(u => u.Item1.Length + u.Item2.Length).FirstOrDefault();
			if(partsWay==null) return ConvertPointToMove(wayPlayerToExit.Last());

			var way = JoiningParts(partsWay);
			return ConvertPointToMove(way);
		}

		private static SinglyLinkedList<Point> JoiningParts(Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>> partsWay)
		{
			var way = new SinglyLinkedList<Point>(partsWay.Item2.Previous.Value, partsWay.Item1);
			var nextPoint = partsWay.Item2.Previous;
			while (nextPoint.Previous != null)
			{
				way = new SinglyLinkedList<Point>(nextPoint.Previous.Value, way);
				nextPoint = nextPoint.Previous;
			}
			return way;	
		}

		private static MoveDirection DefinitionMove(int dx, int dy)
		{
			if (dx != 0)
				if (dx == 1) return MoveDirection.Left;
				else return MoveDirection.Right;
			else
				if (dy == 1) return MoveDirection.Up;
				else return MoveDirection.Down;
		}

		private static MoveDirection[] ConvertPointToMove(SinglyLinkedList<Point> way)
		{
			return way
					.Zip(way.Previous, (nextPoint, point) => DefinitionMove(point.X - nextPoint.X, point.Y - nextPoint.Y))
					.Reverse()
					.ToArray();
		}
	}
}
