using System;

namespace SeasonsOfServerless
{
    public class KebabRecipe
    {
        private double _lambMeat;
        private int _roundingPrecision;

        public KebabRecipe(double kilogramMeat)
        {
            _lambMeat = kilogramMeat;
            // The bigger the number in kg, the less precise the rounding needs to be:
            _roundingPrecision = (Math.Truncate(_lambMeat).ToString().Length - 3) * -1;
            // Rounding can never be lower than 0:
            _roundingPrecision = _roundingPrecision < 0 ? 0 : _roundingPrecision;
        }

        // Recipe used: https://www.thespruceeats.com/adana-kebab-4164647
        // Calculations are based on 1 pound of lamb meat = 0.45 kg = 4 persons.
        public string RecipeSource => "https://www.thespruceeats.com/adana-kebab-4164647";
        public string NrOfServings => $"Number of servings: {Math.Round(_lambMeat*4/0.45, _roundingPrecision)}.";
        public string Lamb => $"Lamb meat: {Math.Round(_lambMeat, _roundingPrecision)} kg.";
        public string Onion => $"Onions: {Math.Round(_lambMeat/0.45, _roundingPrecision)} pieces.";
        public string Garlic => $"Garlic cloves: {Math.Round(_lambMeat*4/0.45, _roundingPrecision)} pieces.";
        public string Cumin => $"Cumin: {Math.Round(_lambMeat*1.5/0.45, _roundingPrecision)} teaspoons.";
        public string Sumac => $"Sumac: {Math.Round(_lambMeat*1.5/0.45, _roundingPrecision)} teaspoons.";
        public string Salt => $"Salt: {Math.Round(_lambMeat*0.5/0.45, _roundingPrecision)} teaspoons.";
        public string BlackPepper => $"Black pepper: {Math.Round(_lambMeat*0.25/0.45, _roundingPrecision)} teaspoons.";
        public string PepperFlakes => $"Red pepper flakes: {Math.Round(_lambMeat*0.25/0.45, _roundingPrecision)} teaspoons.";
        public string Water => $"Ice cold water: {Math.Round(_lambMeat*2/0.45, _roundingPrecision)} tablespoons.";
    }
}