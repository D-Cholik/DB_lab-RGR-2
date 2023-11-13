using RGR.Models;
using RGR.Models.Entities;
using RGR.Views;

namespace RGR.Controllers;

public class ClientController : Controller<Client>
{
    public ClientController(Model<Client> model) : base(model) { }

    public void CreateItem(string firstName, string lastName, string email)
    {
        Client client = new Client() { First_Name = firstName, Last_Name = lastName, Email =  email };

        CreateItem(client);
    }

    public void UpdateItem(long id, string? firstName = default, string? lastName = default, string? email = default)
    {
        Client client = Model.ReadItems().Where(e => e.Client_ID == id).FirstOrDefault();

        try
        {
            client.First_Name = firstName ?? client.First_Name;
            client.Last_Name = lastName ?? client.Last_Name;
            client.Email = email ?? client.Email;
        }
        catch (Exception ex) 
        {
            View.ShowError(ex);
        }
        

        UpdateItem(id, client);
    }
}
