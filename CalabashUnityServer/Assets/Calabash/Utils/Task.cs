using System;
using System.Threading;

// Very-very simple System.Threading.Tasks.Task implementation
namespace Calabash
{
	public class Task {
		protected ManualResetEvent endedEvent = new ManualResetEvent(false);
		protected bool hasEnded = false;
		protected Action action = null;

		public Task() {
		}

		public Task(Action action) {
			this.action = action;
		}

		public virtual void Do() {
			if (action != null) {
				action = null;
			}
			hasEnded = true;
			endedEvent.Set();
		}

		public void Wait() {
			if (hasEnded)
				return;
			endedEvent.WaitOne();
		}
	}

	public class Task<T> : Task {

		protected Func<T> func = null;

		public Task(Func<T> func) {
			this.func = func;
		}

		protected T result = default(T);

		public T Result {
			get {
				return result;
			}
		}

		public override void Do() {
			if (func != null) {
				result = func();
			}
			base.Do();
		}
	}
}