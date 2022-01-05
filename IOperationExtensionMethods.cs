
using System;
using System.Collections.Generic;
using System.Text;

namespace kdyf.operations
{
    public static class IOperationExtensionMethods
    {
        public static SucceedResult<TInOut> Succeed<TInOut>(this IOperation<TInOut> op, TInOut data)
        {
            return new SucceedResult<TInOut>() {Result = data};
        }

        public static RepeatResult<TInOut> Repeat<TInOut>(this IOperation<TInOut> op, TInOut data)
        {
            return new RepeatResult<TInOut>() { Result = data };
        }
    }
}
