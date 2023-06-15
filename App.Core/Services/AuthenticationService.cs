﻿using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;

namespace App.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private bool _isAuthenticated = false;
        private User _currentUser;

        private BoschContext _boschContext;
        public AuthenticationService(BoschContext boschContext)
        {
            _boschContext = boschContext;
        }

        private void Authenticate()
        {
            var adUsername = Environment.UserName;
            var result = _boschContext
                .Users
                .Where(u => u.AdUsername.Equals(adUsername) && u.DeletedDate < u.CreatedDate)
                .ToList();
            if (result.Count != 0 && result[0] != null)
            {
                _isAuthenticated = true;
                _currentUser = result[0];
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (_currentUser == null)
                {
                    Authenticate();
                }
                return _isAuthenticated;
            }
        }

        public bool IsDbActive => _boschContext.Database.CanConnect();

        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    Authenticate();
                }
                return _currentUser;
            }
        }

        public bool IsAdmin 
        {
            get 
            {
                return hasRole(Role.Admin);
            }
        }
        public bool IsSchichtleitung
        {
            get
            {
                return hasRole(Role.Schichtleitung);
            }
        }

        public bool IsLesezugriff
        {
            get
            {
                return hasRole(Role.Lesezugriff);
            }
        }

        private bool hasRole(Role role) {
           return CurrentUser.Role.Equals(role);
        }
    }
}
