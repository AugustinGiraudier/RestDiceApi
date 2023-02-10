using ClientGRPC;
using Grpc.Net.Client;
using System.Diagnostics;

// The port number must match the port of the gRPC server.

// lauching with .exe :
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
// debug with visual :
//using var channel = GrpcChannel.ForAddress("https://localhost:7118");

// get sides :
var client = new Sides.SidesClient(channel);
var reply = await client.getSidesAsync(new SideRequest { Id = 2 });
Console.WriteLine("Side n2 : ");
Console.WriteLine(reply);


// get sides :
var client2 = new Dices.DicesClient(channel);
var reply2 = await client2.getDicesAsync(new DiceRequest { Id = 1 });
Console.WriteLine("--------------------");
Console.WriteLine("Dice n1 : ");
Console.WriteLine(reply2);

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();