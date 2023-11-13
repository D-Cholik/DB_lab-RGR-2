using RGR.Models;
using RGR.Models.Entities;
using RGR.Views;
using System.Security.Principal;

namespace RGR.Controllers;

public class JourneyController : Controller<Journey>
{
    public JourneyController(Model<Journey> model) : base(model) { }

    public void CreateItem(string journeyName, DateTime departureTime, decimal cost, long clientId)
    {
        Journey entity = new Journey() { Journey_Name = journeyName, Departure_Date = departureTime, Cost = cost, Client_ID = clientId };

        CreateItem(entity);
    }

    public void UpdateItem(long id, string? journeyName = default, DateTime? departureTime = default, decimal? cost = default, long? clientId = default)
    {
        Journey entity = Model.ReadItems().Where(e => e.Journey_ID == id).FirstOrDefault();

        try
        {
            entity.Journey_Name = journeyName ?? entity.Journey_Name;
            entity.Departure_Date = departureTime ?? entity.Departure_Date;
            entity.Cost = cost ?? entity.Cost;
            entity.Client_ID = clientId ?? entity.Client_ID;
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }

        UpdateItem(id, entity);
    }

    public void ReadJourneyClients(long filter)
    {
        try
        {
            View.ShowItems(((JourneyModel)Model).ReadJourneyClients(filter), Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }
}
