using System;

namespace kdyf.operations
{
    public class RepeatResult<TInOut> : IOperationResult<TInOut>
    {
        public TInOut Result { get; set; }
        public IOperationResult<TNew> Cast<TNew>() where TNew : TInOut
        {
            return new RepeatResult<TNew>() { Result = (TNew)this.Result};
        }
    }
}
