using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.TimeSpanExtensions
{
	//public static class TimeSpanExt
	//{
	//	public static string ToString(this TimeSpan timeSpan)
	//	{
	//		if (timeSpan.Milliseconds != 0 || timeSpan.Seconds != 0)
	//			return timeSpan.ToString();

	//		var result = new StringBuilder();

	//		if (timeSpan.Days != 0) result.Append(timeSpan.Days + " д.");
	//		if (timeSpan.Hours != 0) result.Append(timeSpan.Hours + " ч.");
	//		if (timeSpan.Minutes != 0) result.Append(timeSpan.Minutes + " мин.");
	//		return result.ToString();
	//	}

	//public static TimeSpan FromMyString(this TimeSpan timeSpan, string str)
	//{
	//	if (str == "") return TimeSpan.Zero;

	//	TimeSpan
	//}



	public class TimeSpanExt
	{
		public TimeSpanExt(TimeSpan timeSpan, string Format="", bool Trim0 = true)
		{
			this.timeSpan = timeSpan;
			this.Format = Format;
			this.Trim0 = Trim0;
		}

		public string Format { get;}
		public TimeSpan timeSpan { get; }
		public bool Trim0 { get; }

		private string getEnding(string determinant)
		{
			int b = Format.IndexOf(determinant+"{");
			if (b < 0)
				return "";
			//throw new Exception("Wrong Format");

			int e = Format.IndexOf("}", b);
			if (e < 0)
				throw new Exception("Wrong Format"); 
			return Format.Substring(b + 2, e - b - 2);
		}

		
		private string getResult(string determinant)
		{

			string ending = "";
			
			try
			{
				ending = getEnding(determinant);
			}
			catch (Exception e)
			{
				throw e;
			}
			
			//if (ending == "") return "";
			string fmt = "{0:%" + determinant+"}";
			var result = String.Format(fmt, timeSpan);
			if (Trim0)
				result=result.TrimStart('0');
			if (result == "")
				return "";
			else
				return result + ending;			
		}


		public override string ToString()
		{
			if (Format == "")
				return timeSpan.ToString();

			try
			{
				return getResult("d") + getResult("h") + getResult("m");
			}
			catch (Exception)
			{
				return timeSpan.ToString(Format);
			}


		}


	}


}
