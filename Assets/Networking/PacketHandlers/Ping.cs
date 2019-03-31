using System;
using System.Collections;
using UnityEngine;

public class PingStruct
{
  public long time;
  public PingStruct(long time)
  {
    this.time = time;
  }
}

[NetworkHandler("ping")]
public class Ping : PacketHandler
{
  public override void Start()
  {
    base.Start();
    StartCoroutine(PingCheck());
  }

  private long getTime()
  {
    return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
  }
  private IEnumerator PingCheck()
  {
    while (true)
    {
      yield return new WaitForSeconds(1f);
      long time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
      this.emit("ping", new PingStruct(time));
    }
  }

  [SocketEvent]
  public void PingEvent(PingStruct data)
  {
    long currentTime = this.getTime();
    long currentPing = currentTime - data.time;
    Debug.Log("Ping " + currentPing);
  }
}
