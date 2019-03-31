using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Attribute decorator
[AttributeUsage(AttributeTargets.Class |
                       AttributeTargets.Struct)
]
public class NetworkHandler : Attribute
{
  public string prefix;
  public NetworkHandler(string prefix = "")
  {
    this.prefix = prefix;
  }
}

[AttributeUsage(AttributeTargets.Method |
                       AttributeTargets.Struct)
]
public class SocketEvent : Attribute
{
  public string socketEvent = "";
  public SocketEvent()
  {
  }
  public SocketEvent(string socketEvent)
  {
    this.socketEvent = socketEvent;
  }
}

public class NullType { }

public class PacketHandler : MonoBehaviour
{
  private Networking net;
  public virtual void Start()
  {
    this.net = this.GetComponentInParent<Networking>();
    this.register();
  }
  protected void register()
  {
    MethodInfo registerMethod = typeof(Networking).GetMethod("handlerEvent", BindingFlags.Static | BindingFlags.Public);
    Type me = this.GetType();
    IEnumerable<NetworkHandler> networkAttributes = me.GetCustomAttributes(true).OfType<NetworkHandler>();
    foreach (var network in networkAttributes)
    {
      foreach (MethodInfo method in me.GetMethods())
      {
        IEnumerable<SocketEvent> socketAttributes = method.GetCustomAttributes(true).OfType<SocketEvent>();
        foreach (SocketEvent values in socketAttributes)
        {
          string socketEvent = network.prefix + values.socketEvent;
          Type type = typeof(NullType);
          ParameterInfo[] methodParams = method.GetParameters();
          if (methodParams.Length > 0)
          {
            type = methodParams[0].ParameterType;
          }
          if (methodParams.Length > 1)
          {
            Debug.LogError("Handler: '" + method.Name + "' has more of 1 param");
            continue;
          }
          MethodInfo registerMethodType = registerMethod.MakeGenericMethod(new Type[] { type });
          object[] attributes = new object[] { socketEvent, method, this };
          registerMethodType.Invoke(null, attributes);
        }
      }
    }
  }
  protected void emit(string key)
  {
    this.net.emit(key);
  }
  protected void emit<T>(string key, T data) where T : class
  {
    string json = JsonConvert.SerializeObject(data);
    JSONObject send = new JSONObject(json);
    this.net.emit(key, send);
  }
}