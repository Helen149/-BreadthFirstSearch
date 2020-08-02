using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class BfsTask
	{
	    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
			var queue = new Queue<SinglyLinkedList<Point>>();
			var linkPoint = new SinglyLinkedList<Point>(start);
			queue.Enqueue(linkPoint);
			var track = new HashSet<Point>();
			track.Add(start);
			var countChest = 0;

			while (queue.Count != 0)
			{
				linkPoint = queue.Dequeue();
				var point = linkPoint.Value;	
				if (point.X < 0 || point.X >= map.Dungeon.GetLength(0) || point.Y < 0 || point.Y >= map.Dungeon.GetLength(1)) continue;
				if (map.Dungeon[point.X, point.Y] != MapCell.Empty) continue;

				FindLinkPoint(queue, track, linkPoint);
				if (chests.Any(u => u == point))
				{
					countChest++;
					yield return linkPoint;
				}
					
				if (chests.Length == countChest) break;
			}
			yield break;
		}

		private static void FindLinkPoint(Queue<SinglyLinkedList<Point>> queue, HashSet<Point> track, SinglyLinkedList<Point> linkPoint)
		{
			for (var dy = -1; dy <= 1; dy++)
				for (var dx = -1; dx <= 1; dx++)
					if (dx != 0 && dy != 0) continue;
					else
					{
						var nextPoint = new Point { X = linkPoint.Value.X + dx, Y = linkPoint.Value.Y + dy };
						if (track.Contains(nextPoint)) continue;
						track.Add(nextPoint);
						queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, linkPoint));
					}
		}
	}
}