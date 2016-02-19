using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PipeGame
{
	public class PriorityQueue
	{
		int size_;
		List<AStarNode> nodes_;
		
		public PriorityQueue()
		{
			size_ = 0;
			nodes_ = new List<AStarNode>();
		}
		
		public void Add(AStarNode node)
		{
			nodes_.Add(node);
			size_++;
			ShiftUp(node, size_ - 1);
		}
		
		public AStarNode GetNext()
		{
			if (IsEmpty()) return null;
			
			AStarNode node = nodes_[0];
			nodes_[0] = nodes_[size_ - 1];
			nodes_[size_ - 1] = node;
			
			Pop();
			
			if(!IsEmpty()) ShiftDown(nodes_[0], 0);
			
			return node;
		}
		
		private void Pop()
		{
			if (IsEmpty()) return;
			else
			{
				nodes_.RemoveAt(size_ - 1);
				size_--;
			}
		}
		
		public int Size() { return size_; }
		
		public bool IsEmpty() { return size_ == 0; }
		
		public void UpdateKey(AStarNode node, float priority)
		{
			
		}
		
		private void ShiftUp(AStarNode node, int idx)
		{
			if (IsEmpty() || nodes_[(idx - 1) / 2].f <= node.f) return;
			
			nodes_[idx] = nodes_[(idx - 1) / 2];
			nodes_[(idx - 1) / 2] = node;
			ShiftUp(node, (idx - 1) / 2);
		}
		
		private void ShiftDown(AStarNode node, int idx)
		{
			if (IsEmpty() || idx >= size_ / 2) return;
			
			if(size_ % 2 == 0 && idx == (size_ - 1) / 2) //last parent has 1 child
			{
				if(node.f < nodes_[2 * idx + 1].f)
				{
					nodes_[idx] = nodes_[2 * idx + 1];
					nodes_[2 * idx + 1] = node;
				}
				return;
			}
			
			if((node.f < nodes_[2 * idx + 1].f && node.f < nodes_[2 * idx + 2].f)) return;
			
			if(nodes_[2 * idx + 1].f <= nodes_[2 * idx + 2].f && node.f > nodes_[2 * idx + 1].f)
			{
				nodes_[idx] = nodes_[2 * idx + 1];
				nodes_[2 * idx + 1] = node;
				ShiftDown(node, 2 * idx + 1);
			}
			else if(nodes_[2 * idx + 2].f < nodes_[2 * idx + 1].f && node.f > nodes_[2 * idx + 2].f)
			{
				nodes_[idx] = nodes_[2 * idx + 2];
				nodes_[2 * idx + 2] = node;
				ShiftDown(node, 2 * idx + 2);
			}
		}
		
		internal void Clear() { nodes_.Clear(); size_ = 0; }
	}
}