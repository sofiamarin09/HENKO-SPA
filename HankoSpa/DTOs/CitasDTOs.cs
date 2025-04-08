public class CitasDTO
{
    public int CitasID { get; set; }

    public DateTime FechaCita { get; set; }

    public TimeSpan HoraCita { get; set; }

    public string EstadoCita { get; set; }

    public int UsuarioID { get; set; }

    // Relación muchos a muchos con servicios
    public List<int> ServiciosSeleccionados { get; set; }
}
