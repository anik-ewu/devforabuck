using DevForABuck.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace DevForABuck.Application.Commands;

public class CreateBookingCommand: IRequest<Booking>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Stack { get; set; }
    public int ExperienceYears { get; set; }
    public DateTime SlotTime { get; set; }
    
    public string SessionType { get; set; }
    public IFormFile Resume { get; set; }
}