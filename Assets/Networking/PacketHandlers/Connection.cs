using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NetworkHandler]
public class Connection : PacketHandler
{
  [SocketEvent("open")]
  public void openEven(SocketIOEvent e)
  {
    Debug.Log("Open socket");
  }
  [SocketEvent("close")]
  public void closeEvent()
  {
    Debug.Log("Close socket");
  }
  [SocketEvent("error")]
  public void errorEvent()
  {
    Debug.Log("Error socket");
  }

}
