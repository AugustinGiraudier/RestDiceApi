using ClientGRPC;
using Grpc.Net.Client;

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
// get id=8 :
{
    var reply = await sidesClient.getSideAsync(new SideRequest { Id = 8 });
    Console.WriteLine("--------------------");
    Console.WriteLine("get Side n8 : ");
    Console.WriteLine(reply);
}
// update id=8 :
{
    var reply = await sidesClient.updateSideAsync(new UpdateSideRequest { Id = 8, Image = "EncoreUneImage.png"});
    Console.WriteLine("--------------------");
    Console.WriteLine("update Side n8 : ");
    Console.WriteLine(reply);
}
// delete id=8 :
{
    var reply = await sidesClient.deleteSideAsync(new SideRequest { Id = 8 });
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
// create dice :
{
    var request = new InputDiceRequest();
    request.Types_.Add(new InputSideType { NbSides = 1, ProtoId = 1});
    var reply = await DiceClient.addDiceAsync(request);
    Console.WriteLine("--------------------");
    Console.WriteLine("Add Dice : ");
    Console.WriteLine(reply);
}
// get dice 5:
{
    var reply = await DiceClient.getDiceAsync(new DiceRequest { Id = 5 });
    Console.WriteLine("--------------------");
    Console.WriteLine("Dice n5 : ");
    Console.WriteLine(reply);
}
// update dice 5:
{
    var request = new InputDiceRequest();
    request.Types_.Add(new InputSideType { NbSides = 4, ProtoId = 1 });
    var reply = await DiceClient.updateDiceAsync(new UpdateDiceRequest { Id=5, Update=request });
    Console.WriteLine("--------------------");
    Console.WriteLine("update Dice n5 : ");
    Console.WriteLine(reply);
}
// get all dices :
{
    var reply = await DiceClient.getDicesAsync(new Empty { });
    Console.WriteLine("--------------------");
    Console.WriteLine("Dices : ");
    Console.WriteLine(reply);
}
// delete
{
    var reply = await DiceClient.deleteDiceAsync(new DiceRequest { Id=5});
    Console.WriteLine("--------------------");
    Console.WriteLine("deleted Dice : ");
    Console.WriteLine(reply);
}






Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();