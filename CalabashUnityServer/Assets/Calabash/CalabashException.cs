using UnityEngine;
using System.Collections;

namespace Calabash {
	
	public class CalabashException : System.Exception {
		public CalabashException(string reason) : base(reason) {
			//...
		}
	}
}
