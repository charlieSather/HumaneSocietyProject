using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
            //Employee greg = new Employee { FirstName = "Greg", LastName = "Smithy", UserName = "gSmith", Password = "pw", EmployeeNumber = 10, Email = "GSmith@gmail.com" };
            //Animal dog = new Animal
            //{
            //    Name = "benny",
            //    Weight = 20,
            //    Age = 5,
            //    Demeanor = "passive",
            //    KidFriendly = true,
            //    PetFriendly = true,
            //    Gender = "male",
            //    AdoptionStatus = "available",
            //    CategoryId = 1,
            //    DietPlanId = 1,
            //    EmployeeId = 1
            //};
            //Dictionary<int, string> traits = new Dictionary<int, string> { { 1, "cat" }, { 2, "Frisky" }, { 5, "1" } };
            //Dictionary<int, string> updates = new Dictionary<int, string> { { 1, "dog" }, { 2, "wrex" }, { 3, "50" },{ 4, "2000" }, { 5, "False" } };
            ////Animal frisky = Query.GetAnimalByID(Query.SearchForAnimalsByMultipleTraits(traits).FirstOrDefault().AnimalId);
            //Animal nemo = Query.GetAnimalByID(2);
            //Animal wrex = Query.GetAnimalByID(3);
            //Query.UpdateAnimal(wrex.AnimalId,updates);
            //Client stjohn = Query.GetClient("stjohn", "iliketurtles");



            //Query.Adopt(frisky, stjohn);
            //Query.Adopt(nemo, stjohn);
            //Query.Adopt(wrex, stjohn);

            //Query.UpdateAdoption(true, Query.GetPendingAdoptions().FirstOrDefault());
            //Query.UpdateAdoption(false, Query.GetPendingAdoptions().FirstOrDefault());
            //Query.UpdateAdoption(false, Query.GetPendingAdoptions().FirstOrDefault());

            //Query.RunEmployeeQueries(greg, "create");
            //Query.RunEmployeeQueries(new Employee { FirstName = "Gregory", LastName = "Smithy", Email = "Gsmithy@yahoo.com", EmployeeNumber = 10 }, "update");
            //Query.RunEmployeeQueries(new Employee { FirstName = "", LastName = "", Email = "" }, "update");
            //Query.RunEmployeeQueries(greg, "delete");


            //Query.UpdateShot("rabies", wrex);
            //Query.UpdateShot("malaria", wrex);

            //Query.RunEmployeeQueries(greg, "read");
            //Query.AddAnimal(dog);

            // var result = Query.SearchForAnimalsByMultipleTraits(traits).ToList();

            //result.ForEach(x => Console.WriteLine(x.Name));

            //Parse.CSVFile("animals.csv");


            Query.RemoveAnimal(Query.GetAnimalByID(1));
            Query.RemoveAnimal(Query.GetAnimalByID(2));
            Query.RemoveAnimal(Query.GetAnimalByID(3));
            Console.ReadLine();

            //PointOfEntry.Run();
        }
    }
}
