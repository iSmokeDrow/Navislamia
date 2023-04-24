﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Network.Entities
{
    public class Tag
    {
        public CString AccountName { get; set; } = new CString(61);

        public List<string> CharacterList { get; set; } = new List<string>();

        // TODO: StructPlayer Player;

        public int AccountID { get; set; }

        public int Version { get; set; }

        public float LastReadTime { get; set; }

        public bool AuthVerified { get; set; }

        public byte PCBangMode { get; set; }

        public int EventCode { get; set; }

        public int Age { get; set; }

        public int AgeLimitFlags { get; set; }

        public float ContinuousPlayTime { get; set; }

        public float ContinuousLogoutTime { get; set; }

        public float LastContinuousPlayTimeProcTime;

        public int ConnectionID { get; set; }

        public string NameToDelete { get; set; }

        public bool StorageSecurityCheck { get; set; } = false;


    }
}
