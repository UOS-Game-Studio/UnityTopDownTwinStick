using System;
using UnityEngine;

namespace Rooms
{
    /// <summary>
    /// Door Controller is used by RoomManager to get the exit door used by the player
    /// and to set the entry door when a room is instantiated.
    /// </summary>
    public class DoorController : MonoBehaviour
    {
        public RoomDoor[] doors;

        public DoorDirection GetExitDirection()
        {
            foreach (RoomDoor door in doors)
            {
                if (door.IsExitDoor) return door.direction;
            }

            return DoorDirection.South;
        }

        public DoorDirection entryDir;
        
        // REVIEW: is this function doing too much?
        // Should it be separated into SetEntryDirection and GetRoomPlayerSpawn?
        public Transform SetEntryPoint(DoorDirection exitDir)
        {
            entryDir = exitDir switch
            {
                DoorDirection.North => DoorDirection.South,
                DoorDirection.South => DoorDirection.North,
                DoorDirection.East => DoorDirection.West,
                DoorDirection.West => DoorDirection.East,
                _ => throw new ArgumentOutOfRangeException(nameof(exitDir), exitDir, null)
            };
            
            foreach (RoomDoor door in doors)
            {
                if (door.direction != entryDir) continue;
                
                door.MakeSpawnDoor();
                return door.GetSpawnTransform();
            }

            // if we somehow haven't found a valid door, we just return the transform of the rooms parent object.
            return transform;
        }
    }
}
