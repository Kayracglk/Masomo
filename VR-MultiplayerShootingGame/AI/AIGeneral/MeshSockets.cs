using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SocketId
{
    Spine,
    RightHand,
    PistolRightHand,
    ThighTwist,
}

public class MeshSockets : MonoBehaviour
{
    Dictionary<SocketId, MeshSocket> socketMap = new Dictionary<SocketId, MeshSocket>();

    private void Awake()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (MeshSocket socket in sockets) 
        {
            socketMap[socket.socketID] = socket;
        }
    }

    public void Attach(Transform objectTransform, SocketId socketID)
    {
        socketMap[socketID].Attach(objectTransform);
    }
}
