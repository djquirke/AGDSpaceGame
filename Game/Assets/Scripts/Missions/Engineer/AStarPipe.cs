using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PipeGame
{
	public class AStarNode
	{
		public AStarNode parent;
		public int x, y;
		public float f, g, h;
		
		public override bool Equals(object obj)
		{
			AStarNode node = obj as AStarNode;
			return (node.x == this.x && node.y == this.y);
		}
		
		public override int GetHashCode()
		{
			int hash = 23;
			return hash + 31 * x.GetHashCode() * y.GetHashCode();
		}
	}
	
	public class AStarPipe {
		
		BoardManager3D bm_;
		
		public void Run(GameObject start_tile, GameObject end_tile, int board_width, BoardManager3D bm)
		{
			bm_ = bm;
			
			PriorityQueue open_q = new PriorityQueue();
			Dictionary<AStarNode, AStarNode> open_d = new Dictionary<AStarNode, AStarNode>();
			Dictionary<AStarNode, AStarNode> closed_nodes = new Dictionary<AStarNode, AStarNode>();
			
			AStarNode goal = new AStarNode();
			goal.x = board_width - 1;
			goal.y = end_tile.GetComponent<Tile3D>().Y();
			
			AStarNode start = new AStarNode();
			start.x = 0;
			start.y = start_tile.GetComponent<Tile3D>().Y();
			start.h = GetNodeDistance(start, goal);
			start.g = 0;
			start.f = start.g + start.h;
			
			open_q.Add(start);
			open_d.Add(start, start);
			bool atGoal = false;
			
			while (open_q.Size() > 0)
			{
				AStarNode mvp = open_q.GetNext();
				open_d.Remove(mvp);
				
				if(AtSamePos(mvp, goal))
				{
					goal.parent = mvp;
					atGoal = true;
					break;
				}
				
				List<AStarNode> successors = getNodeSuccessors(mvp);
				
				foreach(AStarNode node in successors)
				{
					if(AtSamePos(node, start)) continue;
					
					node.g = mvp.g + 1;
					
					if(BetterNodeAlreadyInList(node, open_d))
						continue;
					if(BetterNodeAlreadyInList(node, closed_nodes))
						continue;
					
					node.h = GetNodeDistance(node, goal);
					node.f = node.g + node.h;
					
					open_q.Add(node);
					open_d.Add(node, node);
				}
				
				closed_nodes.Add(mvp, mvp);
			}
			
			if(atGoal)
			{
				TraversePath(goal);
			}
		}
		
		void TraversePath(AStarNode node)
		{
			Tile3D this_node = bm_.getTile(node.x, node.y);
			this_node.setFlow(true);
			
			if(node.parent == null)
			{
				return;
			}
			TraversePath(node.parent);
		}
		
		float GetNodeDistance(AStarNode start, AStarNode end)
		{
			return (Mathf.Pow((end.x - start.x), 2) + Mathf.Pow((end.y - start.y), 2));
		}
		
		List<AStarNode> getNodeSuccessors(AStarNode node)
		{
			List<AStarNode> successors = new List<AStarNode>();
			Tile3D cur_tile_exits = bm_.getTile(node.x, node.y);
			if(cur_tile_exits.north)
			{
				int[] nextcoords = bm_.getNextCoords(node.x, node.y, Direction.NORTH);
				if(!bm_.BlankTile(nextcoords[0], nextcoords[1]))
				{
					Tile3D next_tile = bm_.getTile(nextcoords[0], nextcoords[1]);
					if(next_tile.hasExitOppositeTo(Direction.NORTH))
					{
						AStarNode successor = new AStarNode();
						successor.x = nextcoords[0];
						successor.y = nextcoords[1];
						successor.parent = node;
						successors.Add(successor);
					}
				}
			}
			if(cur_tile_exits.east)
			{
				int[] nextcoords = bm_.getNextCoords(node.x, node.y, Direction.EAST);
				if(!bm_.BlankTile(nextcoords[0], nextcoords[1]))
				{
					Tile3D next_tile = bm_.getTile(nextcoords[0], nextcoords[1]);
					if(next_tile.hasExitOppositeTo(Direction.EAST))
					{
						AStarNode successor = new AStarNode();
						successor.x = nextcoords[0];
						successor.y = nextcoords[1];
						successor.parent = node;
						successors.Add(successor);
					}
				}
			}
			if(cur_tile_exits.south)
			{
				int[] nextcoords = bm_.getNextCoords(node.x, node.y, Direction.SOUTH);
				if(!bm_.BlankTile(nextcoords[0], nextcoords[1]))
				{
					Tile3D next_tile = bm_.getTile(nextcoords[0], nextcoords[1]);
					if(next_tile.hasExitOppositeTo(Direction.SOUTH))
					{
						AStarNode successor = new AStarNode();
						successor.x = nextcoords[0];
						successor.y = nextcoords[1];
						successor.parent = node;
						successors.Add(successor);
					}
				}
			}
			if(cur_tile_exits.west)
			{
				int[] nextcoords = bm_.getNextCoords(node.x, node.y, Direction.WEST);
				if(!bm_.BlankTile(nextcoords[0], nextcoords[1]))
				{
					Tile3D next_tile = bm_.getTile(nextcoords[0], nextcoords[1]);
					if(next_tile.hasExitOppositeTo(Direction.WEST))
					{
						AStarNode successor = new AStarNode();
						successor.x = nextcoords[0];
						successor.y = nextcoords[1];
						successor.parent = node;
						successors.Add(successor);
					}
				}
			}
			
			return successors;
		}
		
		bool AtSamePos(AStarNode node1, AStarNode node2)
		{
			return (node1.x == node2.x && node1.y == node2.y);
		}
		
		bool BetterNodeAlreadyInList(AStarNode node, Dictionary<AStarNode, AStarNode> dict)
		{
			return dict.ContainsKey(node);
		}
	}
}
