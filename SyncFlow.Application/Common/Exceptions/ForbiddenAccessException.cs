using System;

namespace SyncFlow.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando el usuario actual carece de permisos para realizar la operación.
/// </summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string? message = null)
        : base(message ?? "Acceso prohibido para el usuario actual.") { }
}
