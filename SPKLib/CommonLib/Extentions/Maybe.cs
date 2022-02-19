using System;

namespace CommonLib.Maybe
{
    public static class Maybe
	{
        /// <summary>
        /// Выводит результат evaluator если объект не null, иначе выводит failureValue
        /// </summary> 
        public static TResult With<TInput, TResult>
			(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue=default(TResult))
			where TInput:class		
		{
			if (o == null) return failureValue;
			return evaluator(o);
		}
        
    }
}
