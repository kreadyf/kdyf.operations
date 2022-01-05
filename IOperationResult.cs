using System;
using System.Collections.Generic;
using System.Text;

namespace kdyf.operations
{
    public interface IOperationResult<TInOut>

    {
        TInOut Result { get; set; }
        IOperationResult<TNew> Cast<TNew>() where TNew : TInOut;
    }
}
