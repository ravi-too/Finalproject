using System.Text.Json;

public class UserService
{
    private const string UserFile = "users.json";
    public List<User> Users { get; private set; }

    public UserService()
    {
        if (File.Exists(UserFile))
        {
            string json = File.ReadAllText(UserFile);
            Users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        else
        {
            Users = new List<User>();
        }
    }

    public void SaveUsers()
    {
        string json = JsonSerializer.Serialize(Users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(UserFile, json);
    }

    public User? Login(string personalId, string password)
    {
        return Users.FirstOrDefault(u => u.PersonalId == personalId && u.Password == password);
    }

    public User Register(string name, string surname, string personalId)
    {
        if (Users.Any(u => u.PersonalId == personalId))
            throw new Exception("am piradi nomrit aris uvke daregistrirebulli pirovneba!");

        Random rnd = new Random();
        string password = rnd.Next(1000, 9999).ToString();

        User newUser = new User
        {
            Id = Users.Count + 1,
            Name = name,
            Surname = surname,
            PersonalId = personalId,
            Password = password,
            Balance = 0
        };

        Users.Add(newUser);
        SaveUsers();

        return newUser;
    }
}
