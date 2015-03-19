using System.Collections;
using System.Collections.Generic;

public class Parameters {

	// Public Variables

	// Private Variables
	private Dictionary<string, char> charParam;
	private Dictionary<string, int> intParam;
	private Dictionary<string, bool> boolParam;
	private Dictionary<string, string> stringParam;
	private Dictionary<string, double> doubleParam;
	private Dictionary<string, float> floatParam;

	private Dictionary<string, ArrayList> arrayListParam;
	private Dictionary<string, object> objectParam;

	// Static Variables

	public Parameters() {
		charParam = new Dictionary<string, char>();
		intParam = new Dictionary<string, int>();
		boolParam = new Dictionary<string, bool>();
		stringParam = new Dictionary<string, string>();
		doubleParam = new Dictionary<string, double>();
		floatParam = new Dictionary<string, float>();

		arrayListParam = new Dictionary<string,ArrayList>();
		objectParam = new Dictionary<string,object>();
	}

	public void PutExtra(string name, char value) {
		charParam.Add(name, value);
	}

	public void PutExtra(string name, int value) {
		intParam.Add(name, value);
	}

	public void PutExtra(string name, bool value) {
		boolParam.Add(name, value);
	}

	public void PutExtra(string name, string value) {
		stringParam.Add(name, value);
	}

	public void PutExtra(string name, double value) {
		doubleParam.Add(name, value);
	}

	public void PutExtra(string name, float value) {
		floatParam.Add(name, value);
	}

	public void PutExtra(string name, ArrayList value) {
		arrayListParam.Add(name, value);
	}

	public void PutExtra(string name, object value) {
		objectParam.Add(name, value);
	}

	public char GetExtra(string name, char defaultValue) {
		return (charParam.ContainsKey(name)) ? charParam[name] : defaultValue;
	}

	public int GetExtra(string name, int defaultValue) {
		return (intParam.ContainsKey(name)) ? intParam[name] : defaultValue;
	}

	public bool GetExtra(string name, bool defaultValue) {
		return (boolParam.ContainsKey(name)) ? boolParam[name] : defaultValue;
	}

	public string GetExtra(string name, string defaultValue) {
		return (stringParam.ContainsKey(name)) ? stringParam[name] : defaultValue;
	}

	public double GetExtra(string name, double defaultValue) {
		return (doubleParam.ContainsKey(name)) ? doubleParam[name] : defaultValue;
	}

	public float GetExtra(string name, float defaultValue) {
		return (floatParam.ContainsKey(name)) ? floatParam[name] : defaultValue;
	}

	public ArrayList GetExtra(string name, ArrayList defaultValue) {
		return (arrayListParam.ContainsKey(name)) ? arrayListParam[name] : defaultValue;
	}

	public object GetExtra(string name, object defaultValue) {
		return (objectParam.ContainsKey(name)) ? objectParam[name] : defaultValue;
	}
}