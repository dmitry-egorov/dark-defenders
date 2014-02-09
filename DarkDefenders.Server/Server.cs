using System;
using System.IO;
using DarkDefenders.Console;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Files;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.Util;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Server
{
    public class Server
    {
        

        public void Start()
        {
            
        }

        public void Stop()
        {
            _stop = true;
        }

        
    }
}
