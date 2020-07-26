using P01_StudentSystem.Data;

namespace P01_StudentSystem
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new StudentSystemContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
//using System;
//using System.Linq;
//using System.Reflection;
//using System.Collections.Generic;

//using NUnit.Framework;

//using P03_FootballBetting.Data.Models;

//[TestFixture]
//public class Test_003
//{
//    [Test]
//    public void ValidateModels()
//    {
//        //Get assembly from the most important model
//        var assembly = typeof(Team).Assembly;

//        var modelNames = new string[]
//        {
//            "Bet", "Color", "Country", "Game", "Player", "PlayerStatistic", "Position", "Team", "Town", "User"
//        };

//        var types = new Dictionary<string, Type>();
//        foreach (string name in modelNames)
//        {
//            types[name] = GetModelType(assembly, name);
//        }

//        //Check Bet
//        AssertPropertyIsOfType(types["Bet"], "BetId", typeof(int));
//        AssertPropertyIsOfType(types["Bet"], "Amount", typeof(decimal));
//        AssertPropertyIsOfType(types["Bet"], "DateTime", typeof(DateTime));

//        var contentType = GetPropertyByName(types["Bet"], "Prediction");
//        Assert.IsNotNull(contentType, "Bet.Preddiction property not found!");

//        AssertPropertyIsOfType(types["Bet"], "UserId", typeof(int));
//        AssertPropertyIsOfType(types["Bet"], "User", types["User"]);

//        AssertPropertyIsOfType(types["Bet"], "GameId", typeof(int));
//        AssertPropertyIsOfType(types["Bet"], "Game", types["Game"]);

//        //Check Color
//        AssertPropertyIsOfType(types["Color"], "ColorId", typeof(int));
//        AssertPropertyIsOfType(types["Color"], "Name", typeof(string));

//        AssertCollectionIsOfType(types["Color"], "PrimaryKitTeams", GetCollectionType(types["Team"]));
//        AssertCollectionIsOfType(types["Color"], "SecondaryKitTeams", GetCollectionType(types["Team"]));

//        //Check Country
//        AssertPropertyIsOfType(types["Country"], "CountryId", typeof(int));
//        AssertPropertyIsOfType(types["Country"], "Name", typeof(string));

//        AssertCollectionIsOfType(types["Country"], "Towns", GetCollectionType(types["Town"]));

//        //Check User
//        AssertPropertyIsOfType(types["User"], "UserId", typeof(int));
//        AssertPropertyIsOfType(types["User"], "Username", typeof(string));
//        AssertPropertyIsOfType(types["User"], "Email", typeof(string));
//        AssertPropertyIsOfType(types["User"], "Name", typeof(string));
//        AssertPropertyIsOfType(types["User"], "Balance", typeof(decimal));

//        AssertCollectionIsOfType(types["User"], "Bets", GetCollectionType(types["Bet"]));

//        //Check Town
//        AssertPropertyIsOfType(types["Town"], "TownId", typeof(int));
//        AssertPropertyIsOfType(types["Town"], "Name", typeof(string));

//        AssertPropertyIsOfType(types["Town"], "CountryId", typeof(int));
//        AssertPropertyIsOfType(types["Town"], "Country", types["Country"]);

//        AssertCollectionIsOfType(types["Town"], "Teams", GetCollectionType(types["Team"]));

//        //Check Position
//        AssertPropertyIsOfType(types["Position"], "PositionId", typeof(int));
//        AssertPropertyIsOfType(types["Position"], "Name", typeof(string));

//        AssertCollectionIsOfType(types["Position"], "Players", GetCollectionType(types["Player"]));

//        //Check PlayerStatistic
//        AssertPropertyIsOfType(types["PlayerStatistic"], "PlayerId", typeof(int));
//        AssertPropertyIsOfType(types["PlayerStatistic"], "Player", types["Player"]);

//        AssertPropertyIsOfType(types["PlayerStatistic"], "GameId", typeof(int));
//        AssertPropertyIsOfType(types["PlayerStatistic"], "Game", types["Game"]);

//        AssertInteger(types["PlayerStatistic"], "ScoredGoals");
//        AssertInteger(types["PlayerStatistic"], "Assists");
//        AssertInteger(types["PlayerStatistic"], "MinutesPlayed");
//    }

//    public static void AssertRealNumber(Type type, string propertyName)
//    {
//        var property = GetPropertyByName(type, propertyName);
//        Assert.IsNotNull(property, $"{type.Name}.{propertyName} property not found.");

//        var errorMessage = string.Format($"{type.Name}.{property.Name} property is not a real number!");
//        Assert.IsTrue(new[]
//            {
//                typeof(decimal),
//                typeof(float),
//                typeof(double)
//            }
//            .Any(t => t == property.PropertyType), errorMessage);
//    }

//    public static void AssertInteger(Type type, string propertyName)
//    {
//        var property = GetPropertyByName(type, propertyName);
//        Assert.IsNotNull(property, $"{type.Name}.{propertyName} property not found.");

//        var errorMessage = string.Format($"{type.Name}{property.Name} property is not an integer!");
//        Assert.IsTrue(new[]
//            {
//                typeof(byte),
//                typeof(int),
//                typeof(long)
//            }
//            .Any(t => t == property.PropertyType), errorMessage);
//    }

//    public static PropertyInfo GetPropertyByName(Type type, string propName)
//    {
//        var properties = type.GetProperties();

//        var firstOrDefault = properties.FirstOrDefault(p => p.Name == propName);
//        return firstOrDefault;
//    }

//    public static void AssertPropertyIsOfType(Type type, string propertyName, Type expectedType)
//    {
//        var property = GetPropertyByName(type, propertyName);
//        Assert.IsNotNull(property, $"{type.Name}.{propertyName} property not found.");

//        var errorMessage = string.Format($"{type.Name}.{property.Name} property is not {expectedType}!");
//        Assert.That(property.PropertyType, Is.EqualTo(expectedType), errorMessage);
//    }

//    public static void AssertCollectionIsOfType(Type type, string propertyName, Type collectionType)
//    {
//        var ordersProperty = GetPropertyByName(type, propertyName);

//        var errorMessage = string.Format($"{type.Name}.{propertyName} property not found!");

//        Assert.IsNotNull(ordersProperty, errorMessage);

//        Assert.That(collectionType.IsAssignableFrom(ordersProperty.PropertyType));
//    }

//    public static Type GetModelType(Assembly assembly, string modelName)
//    {
//        var modelType = assembly.GetTypes()
//            .Where(t => t.Name == modelName)
//            .FirstOrDefault();

//        Assert.IsNotNull(modelType, $"{modelName} model not found!");

//        return modelType;
//    }

//    public static Type GetCollectionType(Type modelType)
//    {
//        var collectionType = typeof(ICollection<>).MakeGenericType(modelType);
//        return collectionType;
//    }
//}