using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ServiceStack.DataAnnotations;
using FluentValidation;
using System.Text;
using BestProfanityDetector;
using Axe_Server.Validation;

var builder = WebApplication.CreateBuilder(args);
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(PlayerMapper));

builder.Services.AddScoped<IValidator<ScoreTable>, PlayerValidator>();
builder.Services.AddScoped<IValidator<LoginTable>, LoginValidator>();

//get and use database source string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseMySql(builder.Configuration.GetConnectionString("MyDb"), serverVersion));

//validate token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Asymmetric", o =>
{
    RSA rsa = RSA.Create();
    rsa.FromXmlString(builder.Configuration["Jwt:PublicKey"]);

    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireSignedTokens = true,
        RequireExpirationTime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new RsaSecurityKey(rsa)
    };
});

//require authorization by default
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes("Asymmetric")
    .RequireAuthenticatedUser()
    .Build();
});


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

//fun ascii art
app.MapGet("/", () =>
{
    return @"
                        ______    __
                ___,---'......`-./  \--.
               /_/,.::::::::::::.`--/.:.\_
               '.'|:::::::::::::::::::::..\
             .':| ;|::::::;::;::::::|.::::.\
             ':|| `|;|::|; __...:::::|.::::.\
            /.'::\_______.::::::::::::|.::::.|
           ..|.|:::::::::::::::::::::::|.:::.|
           |.|:.|:|.:|:|\:|.|:|.:|:::::|.::::.|
          .'.::.||:|\:.|.:|: ::\\.:|::::|.:::.|
          |.::::|:..||_||`-|\--;`\--:|:::.:::.|
          |.:::::,--''___      ___  |-.:::.::.|
          |.:|:::.\  ` YD     'lYD` |\|:::||:.'
          |.::::::.|   `P      `P', |/|:::'|:|
          `.|.::::.|  --- _    ---  | /::|:'.'
           \|\.:::.`      \'        |';;;||;|
            `|;_|::_\               ':.::.::'
                \;/.=.     o      _/::::::||
                 | -.'\_   --    / .\|:|:|'
              _._|   `'-`-.__,--'  |'\---'-.-.
             /.   `-._    \ \     /  |. |  |\ `.
            /  \  |. .`.,  ,|   _/   /..| :|    \
           /    \ |.. ./   .'.-'    / ..| ', '   :
          :       | ../    |-.     /|.. | :      |
          |       |..|    |/Dn\  .' |...| |      |
          |       |./\    ; lH \'   |. .| '       |
          /       |/  `- '' lH      |..../        |
         |       _/      /  lH     |. ../         |
         |      / \     /   `H     |... |         |
        |     .'  /`- ' |    U     | ...          |
        |     '  ,'   ; |__________|.../          |
        |  ,'|          | . ... ... ...          .'
        |./ .'          | ... ... ... |          |
        `|  |           |  .... ... .|          .'
         |  |          .' .. ........;          |
         | .'          |,---.__,--._|           |
        /  |           |            \-._        :
        |  '          .'             |  `       `
        /             |               \.       /
        \            .'                `      /";
}).AllowAnonymous();

app.MapPost("/register", async (LoginDTO loginDTO, ApplicationDbContext db, IMapper mapper, IValidator<LoginTable> validator) =>
{
    LoginTable login = mapper.Map<LoginTable>(loginDTO);
    bool isNameTaken = await db.loginTable.AnyAsync(x => x.playerName.ToLower() == login.playerName.ToLower());
    ProfanityFilter filter = new ProfanityFilter();

    if (!isNameTaken)
    {
        //run validator
        var validationResult = validator.Validate(login);

        if (validationResult.IsValid)
        {
            //check if name has profanity
            if (!filter.HasProfanity(login.playerName))
            {
                //re-hash password
                string reHashedPassword = HashPassword(login.pwrdHash);
                login.pwrdHash = reHashedPassword;

                //add and save to database
                await db.loginTable.AddAsync(login);

                //add to leaderboard
                ScoreTable playerScore = new ScoreTable()
                {
                    playerName = login.playerName
                };

                await db.scoreTable.AddAsync(playerScore);
                await db.SaveChangesAsync();

                return Results.Created("/register", "Player Registered");
            }
            else
            {
                return Results.BadRequest("name was unavailable");
            }

        }
        else
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return Results.BadRequest(errors);
        }

    }
    else
    {
        return Results.BadRequest("name was taken");
    }
}).AllowAnonymous();

app.MapPost("/login", async (LoginDTO loginDTO, ApplicationDbContext db, IMapper mapper, IValidator<LoginTable> validator) =>
{
    LoginTable login = mapper.Map<LoginTable>(loginDTO);

    //re-hash password
    string reHashedPassword = HashPassword(login.pwrdHash);
    login.pwrdHash = reHashedPassword;

    //validate, to check for empty/null username
    var validationResult = validator.Validate(login);

    if (validationResult.IsValid)
    {
        //match login credentials
        LoginTable correctCredentials = await db.loginTable.FirstAsync(x => x.playerName.ToLower() == login.playerName.ToLower() && x.pwrdHash == login.pwrdHash);

        //if login credentials valid, give them a token for it
        if (correctCredentials != null)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(builder.Configuration["Jwt:PrivateKey"]);

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );

            var jwt = new JwtSecurityToken(
                issuer: builder.Configuration["Jwt:Issuer"],
                audience: correctCredentials.playerName,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Results.Created("/login", token);
        }
        else
        {
            return Results.BadRequest("wrong username or password");
        }
    }
    else
    {
        var errors = validationResult.Errors.Select(x => x.ErrorMessage);
        return Results.BadRequest(errors);
    }


}).AllowAnonymous();

//return all players in leaderboard might need to re-design later
app.MapGet("/allPlayers", async (HttpRequest request, ApplicationDbContext db, IMapper mapper) =>
{
    //retreive jwt
    string token = request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);
    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
    JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
    string playerName = jwtSecurityToken.Audiences.First();

    //read all players on database
    List<ScoreTable> table = await db.scoreTable.ToListAsync();

    //create instance
    List<ScoreDTO> scoreDTOs = new List<ScoreDTO>();
    LeaderBoardData leaderBoardData = new LeaderBoardData();

    //add conversion to instance
    table.ForEach(x =>
    {
        scoreDTOs.Add(mapper.Map<ScoreDTO>(x));
    });

    if (scoreDTOs != null && scoreDTOs.Any())
    {
        scoreDTOs = scoreDTOs.OrderByDescending(x => x.highScore).ToList();

        //if less than 100 players then use whatever it has
        int tableCount = 100;
        if (table.Count < 100)
        {
            tableCount = scoreDTOs.Count;
        }

        //add that range to leaderBoardData
        leaderBoardData.scoreDTOs = scoreDTOs.GetRange(0, tableCount);

        //find player in table
        ScoreDTO? playerTable = scoreDTOs.Find(x => x.playerName == playerName);

        //add score/rank to leaderBoardData
        leaderBoardData.playerScore = playerTable.highScore;
        leaderBoardData.playerRank = scoreDTOs.FindIndex(x => x == playerTable) + 1;
    }

    return Results.Accepted(null, leaderBoardData);
});

app.MapPut("/updateScore", async (ScoreDTO scoreDTO, HttpRequest request, ApplicationDbContext db, IMapper mapper, IValidator<ScoreTable> validator) =>
{
    //retreive jwt
    string token = request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);
    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
    JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
    string playerName = jwtSecurityToken.Audiences.First();

    ScoreTable scoreTable = mapper.Map<ScoreTable>(scoreDTO);

    scoreTable.playerName = playerName;
    var validationResult = validator.Validate(scoreTable);

    //validate check for empty/null strings
    if (validationResult.IsValid)
    {
        ScoreTable playerTable = await db.scoreTable.SingleAsync(x => x.playerName == playerName);

        //check if score is greater
        if (scoreTable.highScore > playerTable.highScore)
        {
            playerTable.highScore = scoreTable.highScore;
            db.scoreTable.Update(playerTable);
            await db.SaveChangesAsync();
        }

        return Results.Created("/updateScore", "Score Updated");
    }
    else
    {
        var errors = validationResult.Errors.Select(x => x.ErrorMessage);
        return Results.BadRequest(errors);
    }
});

app.Run();

string HashPassword(string password)
{
    string salt = "&ljl07";
    //salt password
    string saltedPwrd = password + salt;

    //get hash
    byte[] data = Encoding.ASCII.GetBytes(saltedPwrd);
    data = SHA256.Create().ComputeHash(data);
    string hash = Encoding.ASCII.GetString(data);
    return hash;
}

public class PlayerMapper : Profile
{
    public PlayerMapper()
    {
        CreateMap<ScoreTable, ScoreDTO>();
        CreateMap<ScoreDTO, ScoreTable>();

        CreateMap<LoginTable, LoginDTO>();
        CreateMap<LoginDTO, LoginTable>();
    }
}

public class LeaderBoardData
{
    public int playerScore { get; set; }
    public int playerRank { get; set; }

    public List<ScoreDTO>? scoreDTOs { get; set; }
}

public class ScoreDTO
{
    public int highScore { get; set; } = 0;
    public string playerName { get; set; } = string.Empty;
}

public class LoginDTO
{
    public string playerName { get; set; } = string.Empty;
    public string pwrdHash { get; set; } = string.Empty;
}

public class LoginTable
{
    [Key, AutoIncrement]
    public int id { get; set; } = 0;
    public string playerName { get; set; } = string.Empty;
    public string pwrdHash { get; set; } = string.Empty;
}

public class ScoreTable
{
    [Key, AutoIncrement]
    public int id { get; set; } = 0;
    public int highScore { get; set; } = 0;
    public string playerName { get; set; } = string.Empty;
}

//creating dbcontext
public class ApplicationDbContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationDbContext(DbContextOptions options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public DbSet<LoginTable> loginTable { get; set; }
    public DbSet<ScoreTable> scoreTable { get; set; }
}