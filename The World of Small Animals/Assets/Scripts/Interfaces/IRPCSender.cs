using Photon.Pun;
using System;

public  interface IRPCSender
    {
    void SendRPC(Action action, RpcTarget target = RpcTarget.All, params object[] parameters);
    void SendSecureRPC(Action action, RpcTarget target = RpcTarget.All, bool encrypt = true, params object[] parameters);
}
