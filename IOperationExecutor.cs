using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace kdyf.operations
{
    public interface IOperationExecutor
    {
        IOperationInnerExecutor<TOp, TInOut, TInOut> Add<TOp, TInOut>(Func<TInOut> input)
            where TOp : IOperation<TInOut>;

        IOperationInnerExecutor<TOp, TInOut, TInOut> Add<TOp, TInOut>()
            where TOp : IOperation<TInOut>;
    }
}
