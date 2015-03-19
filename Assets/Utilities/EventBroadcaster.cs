using System.Collections.Generic;

public class EventBroadcaster {

	// Public Variables

	// Private Variables
	private Dictionary<string, ObjectListener> objListener;

	// Static Variables

	private EventBroadcaster() {
		objListener = new Dictionary<string, ObjectListener>();
	}

	public void AddObserver(string name, System.Action action) {
		ObjectListener listener = null;
		if (objListener.ContainsKey(name)) {
			listener = objListener[name];
			listener.AddObserver(action);
		}
		else {
			listener = new ObjectListener();
			listener.AddObserver(action);
			objListener.Add(name, listener);
		}
	}

	public void AddObserver(string name, System.Action<Parameters> action) {
		ObjectListener listener = null;
		if (objListener.ContainsKey(name)) {
			listener = objListener[name];
			listener.AddObserver(action);
		}
		else {
			listener = new ObjectListener();
			listener.AddObserver(action);
			objListener.Add(name, listener);
		}
	}

	public void RemoveObserverAction(string name, System.Action action) {
		if (objListener.ContainsKey(name)) {
			ObjectListener listener = objListener[name];
			listener.RemoveObserver(action);
		}
	}

	public void RemoveObserverAction(string name, System.Action<Parameters> action) {
		if (objListener.ContainsKey(name)) {
			ObjectListener listener = objListener[name];
			listener.RemoveObserver(action);
		}
	}

	public void CallObserver(string name) {
		if (objListener.ContainsKey(name)) {
			ObjectListener listener = objListener[name];
			listener.NotifyObservers();
		}
	}

	public void CallObserver(string name, Parameters param) {
		if (objListener.ContainsKey(name)) {
			ObjectListener listener = objListener[name];
			listener.NotifyObservers(param);
		}
	}

	public void RemoveObserver(string name) {
		if (objListener.ContainsKey(name)) {
			ObjectListener listener = objListener[name];
			listener.RemoveAllObservers();
			objListener.Remove(name);
		}
	}

	public void ClearObservers() {
		foreach (ObjectListener listener in objListener.Values) {
			listener.RemoveAllObservers();
		}

		objListener.Clear();
	}
}