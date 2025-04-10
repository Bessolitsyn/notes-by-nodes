
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
	public partial record ContentDataset(
		
		DateTime CreationDate,
		
		string Description,
		
		string Name,
		
		string Type,
		
		string Uid
		);
}
