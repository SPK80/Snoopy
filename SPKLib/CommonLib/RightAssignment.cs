using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.RightAssignment
{
	public static class RightAssignment
	{
		public static object ChangeType(this object o, Type type) =>
			o == null || type.IsValueType || o is IConvertible ?
				Convert.ChangeType(o, type, null) : o;

		public static T To<T>(this T o) => o;
		public static T To<T>(this T o, out T x) => x = o;
		public static T To<T>(this object o) => (T)ChangeType(o, typeof(T));
		public static T To<T>(this object o, out T x) => x = (T)ChangeType(o, typeof(T));
		
		public static T With<T>(this T o, params object[] pattern) => o;
		public static T With<T>(this T o) => o;
		//public static T With<T, A>(this T o, A a) => o;
		public static T With<T, A, B>(this T o, A a, B b) => o;
		public static T With<T, A, B, C>(this T o, A a, B b, C c) => o;

		public static TX Put<T, TX>(this T o, TX x) => x;
		public static TX Put<T, TX>(this T o, ref TX x) => x;

		public static T WithRef<T, A>(this ref T o, A a) where T : struct => o;

		public static T With<T, A>(this ref T o, A a) where T : struct => o;
		public static T With<T, A>(this T o, A a) where T : class => o;

		public static bool[] Check<T>(this T o, params bool[] pattern) => pattern;

	}
}
