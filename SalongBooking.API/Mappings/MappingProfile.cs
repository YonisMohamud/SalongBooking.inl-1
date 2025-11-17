using AutoMapper;
using SalongBooking.DTOs;
using SalongBooking.Domain.Entities;

namespace SalongBooking.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // Hairdresser mappings
        CreateMap<Hairdresser, HairdresserDto>();
        CreateMap<CreateHairdresserDto, Hairdresser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        CreateMap<UpdateHairdresserDto, Hairdresser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // Service mappings
        CreateMap<Service, ServiceDto>();
        CreateMap<CreateServiceDto, Service>()
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());
        CreateMap<UpdateServiceDto, Service>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // Booking mappings
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<CreateBookingDto, Booking>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Confirmed))
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Hairdresser, opt => opt.Ignore())
            .ForMember(dest => dest.Service, opt => opt.Ignore());
        CreateMap<UpdateBookingDto, Booking>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Hairdresser, opt => opt.Ignore())
            .ForMember(dest => dest.Service, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom((src, dest) => 
                !string.IsNullOrEmpty(src.Status) && Enum.TryParse<BookingStatus>(src.Status, out var status) 
                    ? status 
                    : dest.Status));
    }
}

