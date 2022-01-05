using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;


namespace kdyf.operations
{
    public class OperationExecutor : IOperationExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public OperationExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IOperationInnerExecutor<TOp, TInOut, TInOut> Add<TOp, TInOut>(Func<TInOut> input) where TOp : IOperation<TInOut>
        {
            return new OperationInnerExecutor<TOp, TInOut, TInOut>(null, prev => input(), _serviceProvider);
        }
    }

    // todo remove new() and use DI
    public class OperationInnerExecutor<TOp, TInOut, TInOutPrev> : IOperationInnerExecutor<TOp, TInOut, TInOutPrev> where TOp : IOperation<TInOut>
    {
        private readonly IOperationInnerExecutor<TInOutPrev> _previous;
        private readonly Func<TInOutPrev, TInOut> _inputFunc;
        private readonly IServiceProvider _serviceProvider;

        internal OperationInnerExecutor(IOperationInnerExecutor<TInOutPrev> previous, Func<TInOutPrev, TInOut> inputFunc, IServiceProvider serviceProvider)
        {
            _previous = previous;
            _inputFunc = inputFunc;
            _serviceProvider = serviceProvider;
        }

        public IOperationInnerExecutor<TOpNew, TInOutNew, TInOut> Add<TOpNew, TInOutNew>(Func<TInOut, TInOutNew> input) where TOpNew : IOperation<TInOutNew>
        {
            return new OperationInnerExecutor<TOpNew, TInOutNew, TInOut>(this, input, _serviceProvider);
        }

        public IOperationInnerExecutor<TOpNew, TInOutNew, TInOut> Add<TOpNew, TInOutNew>() where TOpNew : IOperation<TInOutNew>
        {
            return new OperationInnerExecutor<TOpNew, TInOutNew, TInOut>(this, prev => (TInOutNew)(dynamic)prev, _serviceProvider);
        }

        public async Task<IOperationResult<TInOut>> Execute(CancellationToken cancellationToken = default)
        {
            /*Method 1: elegant and quite fast, but recursive stack overflow if to many operations*/
            IOperationResult<TInOutPrev> prevResult = null;
            if (_previous != null)
                prevResult = await _previous.Execute(cancellationToken);

            TOp op = _serviceProvider.GetService<TOp>();

            var result = await op.Execute(_inputFunc(prevResult == null ? default(TInOutPrev) : prevResult.Result), cancellationToken);

            while (result is RepeatResult<TInOut>)
                result = await op.Execute(result.Result, cancellationToken);

            return result;
        }

        public async Task<IOperationResult<TDerivateInOut>> Execute<TDerivateInOut>(CancellationToken cancellationToken = default) where TDerivateInOut : TInOut
        {
            return  (await this.Execute(cancellationToken)).Cast<TDerivateInOut>();
        }

        
    }

}
