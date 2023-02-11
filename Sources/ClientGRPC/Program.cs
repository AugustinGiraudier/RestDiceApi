using ClientGRPC;
using Grpc.Net.Client;
using System.Diagnostics;

// The port number must match the port of the gRPC server.

// lauching with .exe :
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
// debug with visual :
//using var channel = GrpcChannel.ForAddress("https://localhost:7118");


/// ------- SIDES
var sidesClient = new Sides.SidesClient(channel);
Console.WriteLine("\n============== SIDES ==============\n");
// create new :
{
    var reply = await sidesClient.addSideAsync(new InputSideRequest { Image = "nouvelleImage.png" });
    Console.WriteLine("--------------------");
    Console.WriteLine("added new side : ");
    Console.WriteLine(reply);
}
// get id=2 :
{
    var reply = await sidesClient.getSideAsync(new SideRequest { Id = 2 });
    Console.WriteLine("--------------------");
    Console.WriteLine("get Side n2 : ");
    Console.WriteLine(reply);
}
// delete id=2 :
{
    var reply = await sidesClient.deleteSideAsync(new SideRequest { Id = 2 });
    Console.WriteLine("--------------------");
    Console.WriteLine("deleted side n2 : ");
    Console.WriteLine(reply);
}
// get All :
{
    var reply = await sidesClient.getSidesAsync(new Empty { });
    Console.WriteLine("--------------------");
    Console.WriteLine("Sides : ");
    Console.WriteLine(reply);
}


/// ------- DICES
var DiceClient = new Dices.DicesClient(channel);
Console.WriteLine("\n============== DICES ==============\n");

// get  dice :
{
    var reply2 = await DiceClient.getDicesAsync(new DiceRequest { Id = 1 });
    Console.WriteLine("--------------------");
    Console.WriteLine("Dice n1 : ");
    Console.WriteLine(reply2);
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();