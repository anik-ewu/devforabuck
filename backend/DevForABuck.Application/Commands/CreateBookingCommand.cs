using System.ComponentModel.DataAnnotations;
using DevForABuck.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevForABuck.Application.Commands;

// TODO: Consider using FluentValidation for more complex validation rules.
public class CreateBookingCommand : IRequest<Booking>
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Stack { get; set; }

    [Range(0, 50)]
    public int ExperienceYears { get; set; }

    [Required]
    public DateTime SlotTime { get; set; }

    [Required]
    public string SessionType { get; set; }

    [Required]
    public IFormFile Resume { get; set; }
}