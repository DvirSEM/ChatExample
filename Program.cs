using System.Net;
using System.Text;
using System.Text.Json;

class Program
{
  public static void Main()
  {
    Message[] messages = [];

    HttpListener listener = new();
    listener.Prefixes.Add("http://*:5000/");
    listener.Start();

    Console.WriteLine("Server started. Listening for requests...");
    Console.WriteLine("Main page on http://localhost:5000/website/index.html");

    while (true)
    {
      HttpListenerContext context = listener.GetContext();
      HttpListenerRequest request = context.Request;
      HttpListenerResponse response = context.Response;

      string rawPath = request.RawUrl!;
      string absPath = request.Url!.AbsolutePath;

      Console.WriteLine($"Received a request with path: " + rawPath);

      string filePath = "." + absPath;
      bool isHtml = request.AcceptTypes!.Contains("text/html");

      if (File.Exists(filePath))
      {
        byte[] fileBytes = File.ReadAllBytes(filePath);
        if (isHtml) { response.ContentType = "text/html; charset=utf-8"; }
        response.OutputStream.Write(fileBytes);
      }
      else if (isHtml)
      {
        response.StatusCode = (int)HttpStatusCode.Redirect;
        response.RedirectLocation = "/website/404.html";
      }
      else if (absPath == "/getMessages")
      {
        string messagesJson = JsonSerializer.Serialize(messages);
        byte[] messagesBytes = Encoding.UTF8.GetBytes(messagesJson);
        response.OutputStream.Write(messagesBytes);
      }
      else if (absPath == "/sendMessage")
      {
        string messageJson = GetBody(request);
        Message newMessage = JsonSerializer.Deserialize<Message>(messageJson)!;
        messages = [.. messages, newMessage];
      }
      else if (absPath == "/getFriends")
      {
        Friend[] friends = [];

        for (int i = 0; i < messages.Length; i++)
        {
          Message message = messages[i];

          if (!ContainsFriend(friends, message))
          {
            friends = [.. friends, new Friend(message.Name, message.Color)];
          }
        }

        string friendsJson = JsonSerializer.Serialize(friends);
        byte[] friendsBytes = Encoding.UTF8.GetBytes(friendsJson);
        response.OutputStream.Write(friendsBytes);
      }

      response.Close();
    }
  }

  public static string GetBody(HttpListenerRequest request)
  {
    return new StreamReader(request.InputStream).ReadToEnd();
  }

  public static bool ContainsFriend(Friend[] friends, Message message)
  {
    for (int i = 0; i < friends.Length; i++)
    {
      if (friends[i].Name == message.Name && friends[i].Color == message.Color)
      {
        return true;
      }
    }

    return false;
  }
}

class Message(string name, string color, string content)
{
  public string Name { get; set; } = name;
  public string Color { get; set; } = color;
  public string Content { get; set; } = content;
}

class Friend(string name, string color)
{
  public string Name { get; set; } = name;
  public string Color { get; set; } = color;
}