using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.UseCases
{
    internal class NoteCoreException(string message) : Exception(message)
    {
    }
    internal class NullRefernceUseCaseException(string message) : NullReferenceException(message)
    {
    }
    public class NoUsersNoteCoreException() : NullReferenceException()
    {
    }
}
