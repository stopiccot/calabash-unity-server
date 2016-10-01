using System;

namespace Calabash
{
	public class FileLog
	{
		public static object lockObject = new object();

		public static void Log(string text) {
			var logFile = CalabashServer.Instance.logFile;
			if (String.IsNullOrEmpty(logFile)) {
				return;
			}
				
			lock (lockObject) {
				using (var stream = System.IO.File.AppendText(logFile)) {
					stream.WriteLine(text);
					stream.Flush();
				}
			}
		}
	}
}

