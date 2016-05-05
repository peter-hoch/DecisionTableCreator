using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestSupport
{
    public interface IInvokeDiffAction
    {
        void ExecuteDiffAction(string filePath1, string filePath2);

    }
}
