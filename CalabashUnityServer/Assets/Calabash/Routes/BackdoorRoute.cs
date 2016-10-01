using System;
using System.Reflection;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Calabash
{
	public class BackdoorRoute : Route {
		
		public class RequestBody {
			public string selector;
			public string[] arguments;
		}

		public override string HandleRequest(System.Net.HttpListenerRequest httpRequest) {
			var body = DeserializeRequestBody<RequestBody>(httpRequest);

			var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
			var assembly = assemblies.Where(x => x.GetName().Name == "Assembly-CSharp").First();

			var method = assembly.GetTypes()
				.SelectMany(t => t.GetMethods())
				.Where(m => m.GetCustomAttributes(typeof(Backdoor), false).Length > 0)
				.Where(m => m.Name == body.selector)
				.FirstOrDefault();

			if (method == null) {
				throw new CalabashException("Failed to find backdoor method \"" + body.selector + "\"");
			}

			var task = CalabashServer.Instance.ExecuteOnMainThread(() => {
				try {
					method.Invoke(null, null);
				} catch {
					throw new CalabashException("Exception during \"" + body.selector + "\" backdoor method call");
				}
			});

			task.Wait();

			// For now we do nothing
			return "{ \"outcome\": \"SUCCESS\", \"results\": [] }";
		}
	}
}

