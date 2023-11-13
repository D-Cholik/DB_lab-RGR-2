using RGR.Models;
using RGR.Models.Entities;
using RGR.Views;
using System.Security.Principal;

namespace RGR.Controllers;

public class HotelController : Controller<Hotel>
{
    public HotelController(Model<Hotel> model) : base(model) { }

    public void CreateItem(string hotelName, string address, int starRating, long journeyId)
    {
        Hotel entity = new Hotel() { Hotel_Name = hotelName, Address = address, Star_Rating = starRating, Journey_ID = journeyId };

        CreateItem(entity);
    }

    public void UpdateItem(long id, string? hotelName = default, string? address = default, int? starRating = default, long? journeyId = default)
    {
        Hotel entity = Model.ReadItems().Where(e => e.Hotel_ID == id).FirstOrDefault();

        try
        {
            entity.Hotel_Name = hotelName ?? entity.Hotel_Name;
            entity.Address = address ?? entity.Address;
            entity.Star_Rating = starRating ?? entity.Star_Rating;
            entity.Journey_ID = journeyId ?? entity.Journey_ID;
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }

        UpdateItem(id, entity);
    }

    public void ReadHotelFlightCount(long filterF, long filterS)
    {
        try
        {
            View.ShowItems(((HotelModel)Model).ReadHotelFlightCount(filterF, filterS), Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }
}