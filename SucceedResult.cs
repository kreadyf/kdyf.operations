using System;

namespace kdyf.operations
{
    public class SucceedResult<TInOut> : IOperationResult<TInOut>
    {
        public SucceedResult() { }
        public SucceedResult(TInOut result) { this.Result = result; }

        public TInOut Result { get; set; }
        public IOperationResult<TNew> Cast<TNew>() where TNew : TInOut
        {
            return new SucceedResult<TNew>() { Result = (TNew)this.Result};
        }
    }
}
