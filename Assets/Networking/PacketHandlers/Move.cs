using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NetworkHandler]
[System.Serializable]
public class Move : PacketHandler
{
  [SocketEvent("boop")]
  public void move(Vector3 data)
  {
    GameObject cube = GameObject.Find("Cube");
    cube.transform.position = data;
  }
}
