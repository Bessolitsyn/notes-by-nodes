
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
    //REVIEW: ��� ���� � INode ����� �������. ������ � ����������� ������ ���.
    public abstract partial class Node : INode
	{	
		
		public DateTime CreationDate { get; set; }
		
		public string Description { get; set; }
		
		public string Name { get; set; }
		
		public string Text { get; set; }
		
		public string Type { get; set; }
		
		public int Uid { get; set; }
		
		public abstract IEnumerable<Node> HasChildNodes { get; }
		
		protected List<Node> hasChildNodes = [];
		//REVIEW: ���������� �������� ����. ����� ������ Owner
		public abstract User HasOwner { get; }
		
		protected User hasOwner;
        //REVIEW: ���������� �������� ����. ����� ����� ParentNode
        public abstract Node HasParentNode { get; }
		
		protected Node hasParentNode;
		}

	public interface INode
	{
		
		void AddIntoChildNodes(Node item);
		
		void RemoveFromChildNodes(Node item);
		
		void SetOwner(User item);
		
		void SetParentNode(Node item);
	
	}
}
