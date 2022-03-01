using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<PermilleCalculator>();

var app = builder.Build();

app.MapGet("/calculatepermille", ([FromServices] PermilleCalculator calculator, [FromBody] CalculationData data) 
    => calculator.CalculatePermille(data));

app.Run();

class CalculationData
{
    public int WeightInKilos { get; set; }
    public int Drinks { get; set; }
    public Gender Gender { get; set; }
}

class PermilleCalculator
{
    private const double mensLiquidPercentage = 0.68;
    private const double womensLiquidPercentage = 0.55;
    private const int alcoholPrDrinkGram = 12;

    public double CalculatePermille(CalculationData data)
    {
        var weightInGrams = data.WeightInKilos* 1000;
        var liquidWeightInGrams = data.Gender switch
        {
            Gender.Man => weightInGrams * mensLiquidPercentage,
            Gender.Woman => weightInGrams * womensLiquidPercentage,
            _ => throw new InvalidOperationException()
        };
        var totalAlcoholIngestedGrams = alcoholPrDrinkGram * data.Drinks;
        var permille = totalAlcoholIngestedGrams / liquidWeightInGrams * 1000;
        var roundedPermille = Math.Round(permille, 2);
        return roundedPermille;
    }
};

enum Gender
{
    Man,
    Woman
};