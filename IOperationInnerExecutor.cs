using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kdyf.operations
{
    public interface IOperationInnerExecutor<TInOut>
    {
        Task<IOperationResult<TInOut>> Execute(CancellationToken cancellationToken = default);

        Task<IOperationResult<TDerivateInOut>> Execute<TDerivateInOut>(CancellationToken cancellationToken = default)
            where TDerivateInOut : TInOut;
    }

    public interface IOperationInnerExecutor<TOp, TInOut, TInOutPrev> : IOperationInnerExecutor<TInOut>
        where TOp : IOperation<TInOut>

    {
    IOperationInnerExecutor<TOpNew, TInOutNew, TInOut> Add<TOpNew, TInOutNew>(Func<TInOut, TInOutNew> input)
        where TOpNew : IOperation<TInOutNew>;

    IOperationInnerExecutor<TOpNew, TInOutNew, TInOut> Add<TOpNew, TInOutNew>()
        where TOpNew : IOperation<TInOutNew>;
    }
}
