using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public interface Listenable<T> where T : Listener {
	void AddListener (T listener);
	void RemoveListener (T listener);
	void ForEachListener (Action<T> action);
}
