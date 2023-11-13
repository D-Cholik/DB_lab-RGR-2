using RGR.Models;
using RGR.Models.Entities;
using RGR.Views;
using System.Security.Principal;

namespace RGR.Controllers;

public class FlightController : Controller<Flight>
{
    public FlightController(Model<Flight> model) : base(model) { }

    public void CreateItem(int flightNumber, string departure, string destination)
    {
        Flight entity = new Flight() { Flight_Number = flightNumber, Departure = departure, Destination = destination };

        CreateItem(entity);
    }

    public void UpdateItem(long id, int? flightNumber = default, string? departure = default, string? destination = default)
    {

        Flight entity = Model.ReadItems().Where(e => e.Flight_ID == id).FirstOrDefault();

        try
        {
            entity.Flight_Number = flightNumber ?? entity.Flight_Number;
            entity.Departure = departure ?? entity.Departure;
            entity.Destination = destination ?? entity.Destination;
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }

        UpdateItem(id, entity);
    }

    public void ReadFlightClientCount(long filter)
    {
        try
        {
            View.ShowItems(((FlightModel)Model).ReadFlightClientCount(filter), Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }
}

