using SocketIO;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class Networking : MonoBehaviour
{
  #region Private Attributes
  private static SocketIOComponent socket;
  private static Networking instance;
  #endregion
  #region UnityBasics
  public void Awake()
  {
    instance = this;
    socket = this.GetComponent<SocketIOComponent>();
    DontDestroyOnLoad(this.gameObject);
  }
  public void Start()
  {
    StartCoroutine("BeepBoop");
  }
  #endregion
  private IEnumerator BeepBoop()
  {
    // wait 1 seconds and continue
    yield return new WaitForSeconds(1);

    socket.Emit("beep");

    // wait 3 seconds and continue
    yield return new WaitForSeconds(3);

    socket.Emit("beep");

    // wait 2 seconds and continue
    yield return new WaitForSeconds(2);

    socket.Emit("beep");

    // wait ONE FRAME and continue
    yield return null;

    socket.Emit("beep");
    socket.Emit("beep");
  }

  public static void handlerEvent<T>(string key, MethodInfo callback, object classInstance)
  {
    socket.On(key, (SocketIOEvent e) =>
    {
      object[] attributes = null;
      Type argType = typeof(T);
      if (argType == typeof(NullType))
      {
        attributes = null;
      }
      else if (argType == typeof(SocketIOEvent))
      {
        attributes = new object[] { e };
      }
      else
      {
        T value = JSONHandler.parseJson<T>(e.data);
        attributes = new object[] { value };
      }
      callback.Invoke(classInstance, attributes);
    });
  }

  public void emit(string key)
  {
    socket.Emit(key);
  }

  public void emit(string key, JSONObject data)
  {
    socket.Emit(key, data);
  }
}
