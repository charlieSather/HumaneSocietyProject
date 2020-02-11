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
            Employee greg = new Employee { FirstName = "Greg", LastName = "Smithy", UserName = "gSmith", Password = "pw", EmployeeNumber = 10, Email = "GSmith@gmail.com" };
            Animal dog = new Animal
            {
                Name = "benny",
                Weight = 20,
                Age = 5,
                Demeanor = "passive",
                KidFriendly = true,
                PetFriendly = true,
                Gender = "male",
                AdoptionStatus = "available",
                CategoryId = 1,
                DietPlanId = 1,
                EmployeeId = 1
            };
            Dictionary<int, string> traits = new Dictionary<int, string> { { 1, "cat" }, { 2, "Frisky" } };
            Animal frisky = Query.GetAnimalByID(Query.SearchForAnimalsByMultipleTraits(traits).FirstOrDefault().AnimalId);
            Animal nemo = Query.GetAnimalByID(2);
            Animal wrex = Query.GetAnimalByID(3);

            Client stjohn = Query.GetClient("stjohn", "iliketurtles");

            Adoption adoption = new Adoption
            {
                ClientId = stjohn.ClientId,
                AnimalId = frisky.AnimalId,
                ApprovalStatus = "Pending",
                AdoptionFee = 75,
                PaymentCollected = false,
            };

            Query.Adopt(frisky, stjohn);
            Query.Adopt(nemo, stjohn);
            Query.Adopt(wrex, stjohn);

            Query.UpdateAdoption(true, Query.GetPendingAdoptions().FirstOrDefault());
            Query.UpdateAdoption(false, Query.GetPendingAdoptions().FirstOrDefault());
            Query.UpdateAdoption(false, Query.GetPendingAdoptions().FirstOrDefault());

            Query.RunEmployeeQueries(greg, "update");

            
            //Query.RunEmployeeQueries(greg,"delete");
            //Query.RunEmployeeQueries(greg, "read");
            //Query.AddAnimal(dog);

            var result = Query.SearchForAnimalsByMultipleTraits(traits).ToList();

            result.ForEach(x => Console.WriteLine(x.Name));

            Console.ReadLine();

          //PointOfEntry.Run();
        }
    }
}
