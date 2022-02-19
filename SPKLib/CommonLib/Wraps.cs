using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Wraps
{
	public class WrapToString<fillingType>
	{
		public readonly fillingType filling;
		private Func<fillingType, string> toString;

		public WrapToString(fillingType filling, Func<fillingType, string> toString)
		{
			this.filling = filling;
			this.toString = toString ?? throw new ArgumentNullException(nameof(toString));
		}

		public override string ToString()
		{
			return toString?.Invoke(filling);
		}
	}
}
