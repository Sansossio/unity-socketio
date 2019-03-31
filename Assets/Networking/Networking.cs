#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using Newtonsoft.Json;
using SocketIO;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class Networking : MonoBehaviour
{
  [Header("Eventos handler")]
  public PacketHandler[] handlers;
  private static SocketIOComponent socket;

  public void Awake()
  {
    socket = this.GetComponent<SocketIOComponent>();
  }
  public void Start()
  {
    socket.On("open", TestOpen);
    socket.On("error", TestError);
    socket.On("close", TestClose);

    StartCoroutine("BeepBoop");
  }

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
      if (typeof(T) != typeof(NullType))
      {
        T value = JsonConvert.DeserializeObject<T>(e.data.ToString());
        attributes = new object[] { value };
      }
      callback.Invoke(classInstance, attributes);

    });
  }
  public void TestOpen(SocketIOEvent e)
  {
    Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
  }

  public void TestError(SocketIOEvent e)
  {
    Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
  }

  public void TestClose(SocketIOEvent e)
  {
    Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
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
