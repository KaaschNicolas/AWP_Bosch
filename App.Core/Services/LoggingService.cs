﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Helpers;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace App.Core.Services;
public class LoggingService
{
    private BoschContext _boschContext;
    public ILogger<LoggingService> _logger;

    public LoggingService(BoschContext boschContext)
    {
        _boschContext = boschContext;
    }

    private void AuditBase(LogLevel logLevel, string message, AuditEntry auditEntry)
    {
        _logger.Log(logLevel: logLevel, message: message);
        try
        {
            _boschContext.AuditEntries.Add(auditEntry);
            _boschContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            _logger.Log(logLevel: LogLevel.Error, ex.Message);
        }
    }

    public void Log(LogLevel logLevel, string message) => _logger.Log(logLevel, message);

    public void Log(LogLevel logLevel, string message, object obj)
    {
        _logger.Log(
            logLevel,
            message += $" {obj.PropertiesToString()}"
            );
    }

    public void Audit(LogLevel logLevel, string message, User user)
    {
        var auditEntry = new AuditEntry()
        {
            Message = message,
            User = user,
            Level = logLevel.ToString()
        };
        AuditBase(logLevel, message, auditEntry);     
    }

    public void Audit(LogLevel logLevel, string message, User user, Exception exception)
    {
        var auditEntry = new AuditEntry()
        {
            Message = message,
            User = user,
            Level = logLevel.ToString(),
            Exception = exception.Message
        };
        AuditBase(logLevel, message, auditEntry);
    }

    public void Audit(LogLevel logLevel, string message, User user, Exception? exception, object obj)
    {
        var auditEntry = new AuditEntry()
        {
            Message = $"{message}: {obj.PropertiesToString()}",
            User = user,
            Level = logLevel.ToString(),
            Exception = exception.Message
        };
        AuditBase(logLevel, message, auditEntry);
    }
}
