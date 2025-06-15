
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
	public abstract partial class User : Node, IUser
	{	
		
		public abstract IEnumerable<Node> IsOwnerOf { get; }
		
		protected List<Node> isOwnerOf = [];
		}
    //REVIEW: Почему бы не добавить в поля IUserDto
    public interface IUser
	{
		
		void AddIntoOwnerOf(Node item);
		
		void RemoveFromOwnedNodes(Node item);
	
	}
}
