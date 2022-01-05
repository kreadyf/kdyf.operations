using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kdyf.operations
{
    public interface IOperation
    {

    }

    public interface IOperation<TInOut> : IOperation
    {
        Task<IOperationResult<TInOut>> Execute(TInOut input, CancellationToken cancellationToken = default);

    }
}
