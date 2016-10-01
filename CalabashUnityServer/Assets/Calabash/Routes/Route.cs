using System;
using System.Net;
using System.IO;
using UnityEngine;

namespace Calabash
{
	public abstract class Route {
		
		protected T DeserializeRequestBody<T>(HttpListenerRequest httpRequest) {
			string text;
			using (var reader = new StreamReader(httpRequest.InputStream, httpRequest.ContentEncoding)) {
				text = reader.ReadToEnd();
			}

			FileLog.Log("~~~~~~~~~~~~~~");
			FileLog.Log(text);
			FileLog.Log("~~~~~~~~~~~~~~");

			return JsonUtility.FromJson<T>(text);
		}

		public abstract string HandleRequest(HttpListenerRequest httpRequest);
	}
}

