using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HumaneSociety
{
    static class Parse
    {
        static public void CSVFile(string fileName)
        {
            try
            {
                var result = File.ReadAllLines($"{fileName}").Select(x => x.Split(',')).Select(x => x.Select(y => string.Join("", y.Where(z => z != '"'))).ToList());

                foreach (var animal in result)
                {
                    Query.AddAnimal(new Animal
                    {
                        Name = animal[0],
                        Weight = Int32.Parse(animal[1]),
                        Age = Int32.Parse(animal[2]),
                        Demeanor = animal[3],
                        KidFriendly = StringIntToBoolean(animal[4]),
                        PetFriendly = StringIntToBoolean(animal[5]),
                        Gender = animal[6],
                        AdoptionStatus = CorrectAdoptionStatus(animal[7]),
                        CategoryId = IsNumber(animal[8]) ? Int32.Parse(animal[8]) : (int?) null,
                        DietPlanId = IsNumber(animal[8]) ? Int32.Parse(animal[9]) : (int?) null,
                        EmployeeId = IsNumber(animal[8]) ? Int32.Parse(animal[10]) : (int?) null
                    });
                }
            }
            catch (Exception)
            {
                UserInterface.DisplayUserOptions("Error reading file");
            }
        }
        public static bool StringIntToBoolean(string str) => str == "1" ? true : str == "0" ? false : false;

        public static bool IsNumber(string input) => new Regex(@"^\d+$").IsMatch(input) ? true : false;

        public static string CorrectAdoptionStatus(string input) => input == "not adopted" ? "Available" : input;


    }
}
