﻿using DatabaseManagement.Interfaces;
using ESX.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public class CardRepository : IRepository<Card>
{
    private readonly DatabaseManagementContext _dbContext;
    
    public CardRepository(DatabaseManagementContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Card> GetAll()
        => _dbContext.Cards
            .Include(s => s.User)
            .Include(s => s.Transactions)
            .AsEnumerable();

    public Card? GetById(Guid id)
    {
        var cards = _dbContext.Cards
            .Include(s => s.User)
            .Include(s => s.Transactions);
        return cards.FirstOrDefault(s => s.Id.Equals(id));
    }

    public void Add(Card entity)
    {
        if (_dbContext.Cards.Any(s => s.Id.Equals(entity))) throw new InvalidOperationException("Element already exists");
        if (_dbContext.Cards.Any(s => string.Equals(s.CardNumber.Trim(), entity.CardNumber.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            throw new InvalidOperationException("CardNumber already exists");
        if (entity.CardNumber.Length != 16) throw new InvalidOperationException("CardNumber should be equal 16");
        if (entity.Balance < 0m) throw new InvalidOperationException("Balance can't less 0");
        
        _dbContext.Cards.Add(entity);
    }

    public void Update(Card entity)
    {
        var ent = _dbContext.Cards
            .Include(s => s.User)
            .Include(s => s.Transactions)
            .FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element not found");
        if (entity.CardNumber.Length != 16) throw new InvalidOperationException("CardNumber should be equal 16");
        if (entity.Balance < 0m) throw new InvalidOperationException("Balance can't less 0");
        
        ent.CardNumber = entity.CardNumber;
        ent.Balance = entity.Balance;
        ent.Cardless = entity.Cardless;
        ent.User = entity.User;
        ent.UserId = entity.UserId;
        ent.Transactions = entity.Transactions;
    }

    public void Delete(Card entity)
    {
        var ent = _dbContext.Cards.FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element already deleted");

        _dbContext.Remove(ent);
    }

    public async void Save() 
        => await _dbContext.SaveChangesAsync();
}