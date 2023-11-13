using RGR.Models;
using RGR.Views;

namespace RGR.Controllers;

public abstract class Controller<T> where T : class, new()
{
    public Model<T> Model { get; set; }

    public Controller(Model<T> model)
    {
        Model = model;
    }

    protected void CreateItem(T item)
    {
        try
        {
            Model.CreateItem(item);
            View.ShowCreated(item, Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }

    public void ReadItems()
    {
        try
        {
            View.ShowItems(Model.ReadItems(), Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }

    protected void UpdateItem(long id, T item)
    {
        try
        {
            Model.UpdateItem(id, item);
            View.ShowUpdated(item, Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }

    public void DeleteItem(long id)
    {
        try
        {
            Model.DeleteItem(id);
            View.ShowDeleted(id, Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }

    public void Generate(long count)
    {
        try
        {
            Model.GenerateItems(count);
            View.ShowItems(Model.ReadItems(), Model.LastQueryExecutionTime);
        }
        catch (Exception ex)
        {
            View.ShowError(ex);
        }
    }
}
