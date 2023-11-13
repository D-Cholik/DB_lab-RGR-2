using Npgsql;
using RGR.Controllers;
using RGR.Models;
using RGR.Models.Entities;
using RGR.Views;
using Spectre.Console;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

var connection = new NpgsqlConnection();

connection.ConnectionString = "host=localhost;port=5433;database=Lab1;user id=postgres;password=pass12345";

while (true)
{
    switch (MenuPrompt())
    {
        case Selection.Clients:
            {
                ClientController controller = new ClientController(new ClientModel(connection));
                switch (TableMenuPrompt())
                {
                    case "Create":
                        {
                            controller.CreateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter first name")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter last name")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter email")
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        }
                        break;
                    case "Read":
                        {
                            controller.ReadItems();
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        }
                        break;
                    case "Update":
                        {
                            controller.UpdateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter item id")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter first name")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter last name")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter email")
                                    .AllowEmpty()
                                )
                            );
                        }
                        AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        break;
                    case "Delete":
                        {
                            controller.DeleteItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter item id")
                                    .AllowEmpty()
                                )
                            );
                        }
                        AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        ;
                        break;
                    case "Back":
                        {
                            
                        }
                        break;
                }
            }
            break;
        case Selection.Flights:
            {
                FlightController controller = new FlightController(new FlightModel(connection));
                switch (TableMenuPrompt("Read flight client count"))
                {
                    case "Create":
                        {
                            controller.CreateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<int>("Enter flight number")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter departure")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter destination")
                                )
                            );
                        }
                        AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        ;
                        break;
                    case "Read":
                        {
                            controller.ReadItems();
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Update":
                        {
                            controller.UpdateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter item id")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<int>("Enter flight number")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter departure")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter destination")
                                    .AllowEmpty()
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Delete":
                        {
                            controller.DeleteItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter item id")
                                    .AllowEmpty()
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Read flight client count":
                        {
                            controller.ReadFlightClientCount(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter min flight number")
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Back":
                        {
                            ;
                        }
                        break;
                }
            }
            break;
        case Selection.Hotels:
            {
                HotelController controller = new HotelController(new HotelModel(connection));
                switch (TableMenuPrompt("Read hotel by flight count"))
                {
                    case "Create":
                        {
                            controller.CreateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter hotel name")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter address")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<int>("Enter star rating")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter journey id")
                                )
                            );
                            AnsiConsole.Prompt(
                                 new TextPrompt<string>("Press to continue...").AllowEmpty()
                             );
                            ;
                        }
                        break;
                    case "Read":
                        {
                            controller.ReadItems();
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Update":
                        {
                            controller.UpdateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter hotel id")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter hotel name")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter address")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<int>("Enter star rating")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter journey id")
                                    .AllowEmpty()
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Delete":
                        {
                            controller.DeleteItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter item id")
                                    .AllowEmpty()
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Read hotel by flight count":
                        {
                            long first = AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter first flight number bound")
                                );
                            long second = AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter second flight number bound")
                                    .Validate(res =>
                                    {
                                        if (res < first)
                                            return ValidationResult.Error("Second is less than first");

                                        if (res <= 0)
                                            return ValidationResult.Error("Number can't be less than or equal 0");

                                        return ValidationResult.Success();
                                    })
                                );
                            controller.ReadHotelFlightCount(first, second);
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        }
                        break;
                    case "Back":
                        {
                        }
                        break;
                }
            }
            break;
        case Selection.Journeys:
            {
                JourneyController controller = new JourneyController(new JourneyModel(connection));
                switch (TableMenuPrompt("Read journey clients"))
                {
                    case "Create":
                        {
                            controller.CreateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter journey name")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<DateTime>("Enter departure time")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<decimal>("Enter cost")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter client id")
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Read":
                        {
                            controller.ReadItems();
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Update":
                        {
                            controller.UpdateItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter journey id")
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter journey name")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<DateTime>("Enter departure time")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<decimal>("Enter cost")
                                    .AllowEmpty()
                                ),
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter client id")
                                    .AllowEmpty()
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Delete":
                        {
                            controller.DeleteItem(
                                AnsiConsole.Prompt(
                                    new TextPrompt<long>("Enter item id")
                                    .AllowEmpty()
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Read journey clients":
                        {
                            controller.ReadJourneyClients(AnsiConsole.Prompt(
                                new TextPrompt<long>("Enter journey min id")
                            ));
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                            ;
                        }
                        break;
                    case "Back":
                        {
                            ;
                        }
                        break;
                }
            }
            break;
        case Selection.Generate:
            {
                switch (GenerateMenuPrompt())
                {
                    case Selection.Clients:
                        {
                            ClientController controller = new(new ClientModel(connection));
                            controller.Generate(
                                (long)AnsiConsole.Prompt(
                                    new TextPrompt<ulong>("Enter count")
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        }
                        break;
                    case Selection.Flights:
                        {
                            FlightController controller = new(new FlightModel(connection));
                            controller.Generate(
                                (long)AnsiConsole.Prompt(
                                    new TextPrompt<ulong>("Enter count")
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        }
                        break;
                    case Selection.Hotels:
                        {
                            HotelController controller = new(new HotelModel(connection));
                            controller.Generate(
                                (long)AnsiConsole.Prompt(
                                    new TextPrompt<ulong>("Enter count")
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        }
                        break;
                    case Selection.Journeys:
                        {
                            JourneyController controller = new(new JourneyModel(connection));
                            controller.Generate(
                                (long)AnsiConsole.Prompt(
                                    new TextPrompt<ulong>("Enter count")
                                )
                            );
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("Press to continue...").AllowEmpty()
                            );
                        }
                        break;
                }
            }
            break;
        case Selection.Exit:
            return;
    }
}

Selection MenuPrompt()
{
    AnsiConsole.Reset();
    return AnsiConsole.Prompt(
        new SelectionPrompt<Selection>()
        {
            Converter = (Selection s) => s.ToString()
        }
        .Title("Select menu")
        .AddChoices(
            new[]
            {
                Selection.Clients,
                Selection.Flights,
                Selection.Hotels,
                Selection.Journeys,
                Selection.Generate,
                Selection.Exit
            }
        )
    );
}

Selection GenerateMenuPrompt()
{
    return AnsiConsole.Prompt(
        new SelectionPrompt<Selection>()
        {
            Converter = (Selection s) => s.ToString()
        }
        .Title("Select table")
        .AddChoices(
            new[]
            {
                Selection.Clients,
                Selection.Flights,
                Selection.Hotels,
                Selection.Journeys
            }
        )
    );
}
string TableMenuPrompt(string? additionalChoise = null)
{
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Table menu")
        .AddChoices( additionalChoise == null ? 
            new[]
            {
                "Create",
                "Read",
                "Update",
                "Delete",
                "Back"
            }
            :
            new[]
            {
                "Create",
                "Read",
                "Update",
                "Delete",
                additionalChoise,
                "Back"
            }
        )
    );
}

enum Selection
{
    Menu,
    Clients,
    Flights,
    Hotels,
    Journeys,
    Generate,
    Exit
}

