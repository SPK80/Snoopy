using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Redefinitions
{
	class Object
	{
		public static List<Object> operator +(List<Object> o1, Object o2)
		{
			o1.Add(o2);
			return o1;
		}
	}
}
