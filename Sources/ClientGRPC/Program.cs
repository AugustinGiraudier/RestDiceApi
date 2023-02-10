using ClientGRPC;
using Grpc.Net.Client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:5001");


// test greeters
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);

// get sides :
var client2 = new Sides.SidesClient(channel);
var reply2 = await client2.getSidesAsync(new SideRequest { Id = 2 });
Console.WriteLine("Side: " + reply2.Id + " img : " + reply2.Image);



Console.WriteLine("Press any key to exit...");
Console.ReadKey();