﻿using Navislamia.Game.DataAccess.Entities.Enums;

namespace Navislamia.Game.DataAccess.Entities.Arcadia;

public class EnhanceResourceEntity : Entity
{
    public EnhanceType EnhanceType { get; set; }
    public FailResultType FailResult { get; set; }
    public LocalFlag LocalFlag { get; set; }
    public short MaxEnhance { get; set; }
    public decimal[] Percentage { get; set; } // = new decimal[20]; 
    
    public long? RequiredItemId { get; set; }
    public ItemResourceEntity RequiredItem { get; set; }
}
