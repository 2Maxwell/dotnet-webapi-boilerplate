using FSH.WebApi.Application.Accounting.Bookings;
using FSH.WebApi.Domain.Accounting;
using Mapster;

namespace FSH.WebApi.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure()
    {
        // here we will define the type conversion / Custom-mapping
        // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

        // This one is actually not necessary as it's mapped by convention
        // TypeAdapterConfig<Product, ProductDto>.NewConfig().Map(dest => dest.BrandName, src => src.Brand.Name);
        TypeAdapterConfig<Booking, BookingDto>.NewConfig()
                    .Map(dest => dest.Id, src => src.Id)
                    .Map(dest => dest.MandantId, src => src.MandantId)
                    .Map(dest => dest.HotelDate, src => src.HotelDate)
                    .Map(dest => dest.ReservationId, src => src.ReservationId)
                    .Map(dest => dest.Name, src => src.Name)
                    .Map(dest => dest.Amount, src => src.Amount)
                    .Map(dest => dest.Price, src => src.Price)
                    .Map(dest => dest.Debit, src => src.Debit)
                    .Map(dest => dest.ItemId, src => src.ItemId)
                    .Map(dest => dest.ItemNumber, src => src.ItemNumber)
                    .Map(dest => dest.Source, src => src.Source)
                    .Map(dest => dest.BookingLineNumberId, src => src.BookingLineNumberId)
                    .Map(dest => dest.TaxId, src => src.TaxId)
                    .Map(dest => dest.TaxRate, src => src.TaxRate)
                    .Map(dest => dest.InvoicePos, src => src.InvoicePos)
                    .Map(dest => dest.State, src => src.State)
                    .Map(dest => dest.InvoiceId, src => src.InvoiceId)
                    .Map(dest => dest.ReferenceId, src => src.ReferenceId);
    }
}