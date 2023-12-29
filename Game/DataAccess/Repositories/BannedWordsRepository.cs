﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Navislamia.Game.DataAccess.Contexts;
using Navislamia.Game.DataAccess.Repositories.Interfaces;

namespace Navislamia.Game.DataAccess.Repositories;

public class BannedWordsRepository : IBannedWordsRepository
{
    private readonly ArcadiaContext _context;
    
    public BannedWordsRepository(DbContextOptions<ArcadiaContext> options)
    {
        _context = new ArcadiaContext(options);
    }

    public bool IsBannedWord(string value)
    {
        return _context.BannedWordsResources.Any(b =>
            string.Equals(b.Word, value, StringComparison.CurrentCultureIgnoreCase));
    }
    
    public bool ContainsBannedWord(string value)
    {
        return _context.BannedWordsResources.Any(b =>
            b.Word.Contains(value, StringComparison.CurrentCultureIgnoreCase));
    }
}