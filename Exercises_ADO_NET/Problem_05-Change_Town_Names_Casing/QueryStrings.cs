namespace Problem_05_Change_Town_Names_Casing
{
    internal class QueryStrings
    {
        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=MinionsDB;Integrated Security=true;";
        internal const string selectTownsByCountryNameQueryString = @" SELECT t.[Name] 
                                                               FROM Towns as t
                                                               JOIN Countries AS c ON c.Id = t.CountryCode
                                                              WHERE c.Name = @countryName";
        internal const string updateTownNamesToUpperByCountryNameQueryString = 
            @"UPDATE Towns
                   SET [Name] = UPPER([Name])
                 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.[Name] = @countryName)";
    }
}
