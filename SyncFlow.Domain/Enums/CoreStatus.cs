namespace SyncFlow.Domain.Enums;

public enum CoreStatus : byte
{
    Pending = 0,   // aún no inicia – recibe recordatorios
    Active = 1,   // en progreso
    Completed = 2,   // listo / hecho – sin recordatorios
    Canceled = 3    // anulado
}
