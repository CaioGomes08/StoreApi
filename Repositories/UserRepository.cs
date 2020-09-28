using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Repositories
{
    public class UserRepository
    {
        private readonly StoreDataContext _context;

        public UserRepository(StoreDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<ListUserViewModel> Get()
        {
            return _context.Users
                    .Select(u => new ListUserViewModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Nome = u.Nome,
                        Perfil = u.Perfil,
                        Sobrenome = u.Sobrenome
                    })
                    .AsNoTracking()
                    .ToList();
        }

        public ListUserViewModel GetUserByEmailAndPassword(string email, string senha)
        {
            return _context.Users
                        .Where(u => u.Email == email && u.Senha == senha)
                        .Select(u => new ListUserViewModel
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Nome = u.Nome,
                            Perfil = u.Perfil,
                            Sobrenome = u.Sobrenome
                        })
                        .AsNoTracking()
                        .FirstOrDefault();
        }

        public void Save(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
