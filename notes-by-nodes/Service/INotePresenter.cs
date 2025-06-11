using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Service
{
    [Obsolete]
    public interface INotePresenter
    {
        /// <summary>
        /// The argument is a tuple collection of users. First int value is Uid of user, Second value is name of user, Third - email.
        /// </summary>
        /// <param name="users"></param>
        void SetUsers(IEnumerable<(int, string, string)> users);

        
        void SetBoxes(IEnumerable<(int, string, string)> boxes);

        void SetChildNotes(int parentNodeUid, (int, string, string)[] childNodes);

        void Attach(INotePresenterObserver observer);
        void Detach(INotePresenterObserver observer);

        

    }
    [Obsolete]
    public interface INotePresenterObserver
    {
        void GetViewModel<T>(int uid);

    }
}
