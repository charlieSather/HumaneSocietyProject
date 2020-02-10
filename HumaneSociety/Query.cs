using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }
        
        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            switch (crudOperation)
            {
                case ("create"):
                    AddEmployee(employee);
                    break;
                case ("read"):
                    GetEmployeeByNumber(employee);
                    break;
                case ("update"):
                    UpdateEmployee(employee);
                    break;
                case ("delete"):
                    DeleteEmployee(employee);
                    break;
            }
        }

        internal static void AddEmployee(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();

            if(employeeFromDb is null)
            {
                db.Employees.InsertOnSubmit(employee);
                db.SubmitChanges();
            }
            else
            {
               UserInterface.DisplayUserOptions("Employee already exists. Can't add into database.");
            }
        }

        internal static void GetEmployeeByNumber(Employee employee)
        {
            Employee employeeFromDb = db.Employees.FirstOrDefault(e => e.EmployeeNumber == employee.EmployeeNumber);

            if (employeeFromDb is null)
            {
                UserInterface.DisplayUserOptions("Employee not found.");
            }
            else
            {
                UserInterface.DisplayUserOptions($"Hello {employeeFromDb.FirstName} {employeeFromDb.LastName}!");
            }
        }

        internal static void UpdateEmployee(Employee employee)
        {
            Employee employeeFromDb = null;

            try
            {
                employeeFromDb = db.Employees.FirstOrDefault(e => e.EmployeeNumber == employee.EmployeeNumber);
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No employees have an EmployeeNumber that matches the employee passed in.");
                Console.WriteLine("No updates have been made.");
                return;
            }

            employeeFromDb.FirstName = employee.FirstName;
            employeeFromDb.LastName = employee.LastName;
            employeeFromDb.EmployeeNumber = employee.EmployeeNumber;
            employeeFromDb.Email = employee.Email;

            db.SubmitChanges();
        }

        internal static void DeleteEmployee(Employee employee)
        {
            Employee employeeFromDb = db.Employees.FirstOrDefault(e => e.EmployeeNumber == employee.EmployeeNumber);

            if (employeeFromDb is null)
            {
                Console.WriteLine("Employee not found.");
            }
            else
            {
                db.Employees.DeleteOnSubmit(employeeFromDb);
                db.SubmitChanges();
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)
        {
            return db.Animals.FirstOrDefault(a => a.AnimalId == id);
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            foreach(var update in updates)
            {
                switch (update.Key)
                {
                    case 1:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Category = db.Categories.Where(b => b.Name == update.Value).FirstOrDefault();
                        break;
                    case 2:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Name = update.Value;
                        break;
                    case 3:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Age = Int32.Parse(update.Value);
                        break;
                    case 4:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Demeanor = update.Value;
                        break;
                    case 5:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().KidFriendly = Convert.ToBoolean(update.Value);
                        break;
                    case 6:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().PetFriendly = Convert.ToBoolean(update.Value);
                        break;
                    case 7:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Weight = Int32.Parse(update.Value);
                        break;
                    default:
                        break;
                }
                //db.SubmitChanges();
            }
            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> traits) // parameter(s)?
        {
            var animals = db.Animals.AsQueryable();

            foreach(var trait in traits)
            {
                switch (trait.Key)
                {
                    case 1:
                        animals = animals.Where(x => x.Category.Name == trait.Value);
                        break;
                    case 2:
                        animals = animals.Where(x => x.Name == trait.Value);
                        break;
                    case 3:
                        animals = animals.Where(x => x.Age == Int32.Parse(trait.Value));
                        break;
                    case 4:
                        animals = animals.Where(x => x.Demeanor == trait.Value);
                        break;
                    case 5:
                        animals = animals.Where(x => x.KidFriendly == Convert.ToBoolean(trait.Value));
                        break;
                    case 6:
                        animals = animals.Where(x => x.PetFriendly == Convert.ToBoolean(trait.Value));
                        break;
                    case 7:
                        animals = animals.Where(x => x.Weight == Int32.Parse(trait.Value));
                        break;
                    case 8:
                        animals = animals.Where(x => x.AnimalId == Int32.Parse(trait.Value));
                        break;
                }
            }
            return animals;
        }
         
        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            return db.Categories.Where(a => a.Name == categoryName).Select(a => a.CategoryId).FirstOrDefault();
        }
        
        internal static Room GetRoom(int animalId)
        {
            return db.Rooms.Where(a => a.AnimalId == animalId).FirstOrDefault();
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            return db.DietPlans.Where(a => a.Name == dietPlanName).Select(a => a.DietPlanId).FirstOrDefault();
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            if(animal.AdoptionStatus == "available")
            {
                try
                {
                    Adoption adoption = new Adoption();
                    adoption.ClientId = client.ClientId;
                    adoption.AnimalId = animal.AnimalId;
                    adoption.ApprovalStatus = "pending";
                    animal.AdoptionStatus = "pending";
                    adoption.AdoptionFee = 75;
                    adoption.PaymentCollected = false;

                    client.Adoptions.Add(adoption);
                    animal.Adoptions.Add(adoption);

                    db.Adoptions.InsertOnSubmit(adoption);
                    db.SubmitChanges();

                }
                catch (Exception)
                {
                    UserInterface.DisplayUserOptions("Error.");
                }
            }
            else
            {
                UserInterface.DisplayUserOptions("That animal is not available.");
            }
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            return db.Adoptions.Where(x => x.ApprovalStatus == "pending");
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            Adoption adoptionFromDb = null;

            try
            {
                adoptionFromDb = db.Adoptions.Where(x => x.ClientId == adoption.ClientId && x.AnimalId == adoption.AnimalId).FirstOrDefault();
            }
            catch
            {
                UserInterface.DisplayUserOptions("Could not find adoption case in database.");
                return;
            }

            if (isAdopted)
            {
                adoptionFromDb.ApprovalStatus = "accepted";
            }
            else
            {
                adoptionFromDb.ApprovalStatus = "declined";
            }
            db.SubmitChanges();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            Adoption adoption = db.Adoptions.Where(x => x.AnimalId == animalId && x.ClientId == clientId).FirstOrDefault();
            db.Adoptions.DeleteOnSubmit(adoption);

            try
            {
                db.SubmitChanges();
            }
            catch
            {
                UserInterface.DisplayUserOptions("Error on trying to delete adoption");
            }
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            return db.AnimalShots.Where(x => x.AnimalId == animal.AnimalId);
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            AnimalShot animalShotFromDb = null;


            Shot shotFromDb = db.Shots.FirstOrDefault(x => x.Name == shotName);

            if(shotFromDb is null)
            {
                Shot shot = new Shot
                {
                    Name = shotName
                };
                db.Shots.InsertOnSubmit(shot);
                db.SubmitChanges();

                shotFromDb = shot;
            }

            try
            {
                animalShotFromDb = db.AnimalShots.Where(x => x.AnimalId == animal.AnimalId && x.ShotId == animalShotFromDb.ShotId).FirstOrDefault();
            }
            catch
            {
                UserInterface.DisplayUserOptions("Couldn't find animal shot to update");
                return;
            }  
            
            if(animalShotFromDb is null)
            {
                db.AnimalShots.InsertOnSubmit(new AnimalShot { AnimalId = animal.AnimalId, ShotId = shotFromDb.ShotId, DateReceived = DateTime.Now });                
            }
            else
            {
                animalShotFromDb.ShotId = shotFromDb.ShotId;
            }

            try
            {
                db.SubmitChanges();
            }
            catch
            {
                UserInterface.DisplayUserOptions("Error Updating AnimalShot");
            }

            


        }
    }
}