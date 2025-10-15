using Npgsql;
using System.Text.Json; 

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string[] allowedOrigins = new[]
{
    "https://5173-cs-5fc35ef0-8e76-44a3-a5a9-6ca19b688c36.cs-asia-southeast1-bool.cloudshell.dev",
    "https://assetpulse.netlify.app"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMultipleOrigins", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader());
});


var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

app.UseCors("AllowMultipleOrigins");

app.MapGet("/", () => {
    using var connection = new NpgsqlConnection(connectionString); 
    connection.Open(); 
    var command = connection.CreateCommand(); 
    command.CommandText = @"select * from board"; 

    using var reader = command.ExecuteReader(); 
    return Results.Ok("hello world");    
});

app.MapGet("/board", () =>
{
    var list = new List<Dictionary<string, object>>();

    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT id, port_number, power_socket, port_status, other FROM board";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                row["id"] = reader.GetInt32(0);
                row["port_number"] = reader.GetInt32(1);
                row["power_socket"] = reader.GetInt32(2);
                row["port_status"] = reader.GetBoolean(3);
                row["other"] = reader.GetString(4);
                row["ip_address"] = reader.GetString(5); 

                list.Add(row);
            }
        }
    }

    var json = JsonSerializer.Serialize(list);
    return Results.Content(json, "application/json");
});

app.MapPost("/board",async (HttpRequest request) => {
   var body = await request.ReadFromJsonAsync<JsonElement>(); 
   var id = body.GetProperty("id").GetInt32(); 
   var port_number = body.GetProperty("port_number").GetInt32();
   var power_socket = body.GetProperty("power_socket").GetInt32();
   var port_status = body.GetProperty("port_status").GetBoolean();
   var other = body.GetProperty("other").GetString(); 
   var ip_address = body.GetProperty("ip_address").GetString(); 
   
   using var connection = new NpgsqlConnection(connectionString);
   connection.Open();  
   var command = connection.CreateCommand(); 
   command.CommandText = @"insert into board values($id, $port_number, $power_socket, $port_status, $other, $ip_address)";
   
   command.Parameters.AddWithValue("$id", id);
   command.Parameters.AddWithValue("$port_number", port_number);
   command.Parameters.AddWithValue("$power_socket", power_socket);
   command.Parameters.AddWithValue("$port_status", port_status); 
   command.Parameters.AddWithValue("$other",other);  
   command.Parameters.AddWithValue("$ip_address",ip_address); 
   command.ExecuteNonQuery();
   return Results.Ok("got in database");   
}); 

app.MapGet("/delete/{id}", async (HttpRequest request) => {
 
	if (!request.RouteValues.ContainsKey("id"))
	{
	    return Results.BadRequest("Missing id");
	}

	object idObj = request.RouteValues["id"];
	if (idObj == null)
	{
	    return Results.BadRequest("Missing id");
	}

	string idStr = idObj.ToString();
	int id;

	try
	{
	   id = int.Parse(idStr);
	}
	catch (System.Exception)
	{
     return Results.BadRequest("Invalid id");
	}
  
  using var connection = new NpgsqlConnection(connectionString); 
  connection.Open(); 
  var command = connection.CreateCommand(); 
  command.CommandText = @"delete from board where id = $id"; 
  command.Parameters.AddWithValue("$id",id); 
  command.ExecuteNonQuery(); 

  return Results.Ok(@"deleted row where id = $id"); 

}); 

app.MapGet("/hi", () => "hi world");

app.MapGet("/greet", greetUser); 

String greetUser(){
  return "hi user"; 
}

app.MapGet("/sayhello", Handlers.SayHello);
app.Run();
