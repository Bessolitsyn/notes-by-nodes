
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
	public partial record NodeDataset(
		
		DateTime CreationDate,
		
		string Description,
		
		string Name,
		
		string Text,
		
		string Type,
		
		int Uid,
		
		string[] HasChildNodes,
		
		string HasOwner,
		
		string HasParentNode
		);
}
