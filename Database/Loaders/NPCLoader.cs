﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Navislamia.Notification;
using Navislamia.Database;
using Navislamia.Database.Entities;
using Navislamia.Database.Repositories;
using Navislamia.Database.Interfaces;

namespace Navislamia.Database.Loaders
{
    public class NPCLoader : RepositoryLoader, IRepositoryLoader
    {
        IDatabaseService dbSVC;

        public NPCLoader(INotificationService notificationService, IDatabaseService dbService) : base(notificationService)
        {
            dbSVC = dbService;
        }

        public List<IRepository> Init()
        {
            Tasks.Add(new NPCRepository(dbSVC.WorldConnection).Load());

            if (!Execute())
                return null;

            return this.Repositories;
        }
    }
}
