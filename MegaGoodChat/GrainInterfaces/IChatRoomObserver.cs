using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IChatRoomObserver : IGrainObserver
    {
        // TODO: Add an 'OnMessage' method.
        // The interface should inherit from IGrainObserver so that Orleans knows about it.
        // The method must return void because observers are one-way

        // Note: Observers can also return Task, but we wont cover that today.

        // TODO: What other methods would be interesting here? Join/Leave notifications, maybe?
    }
}
