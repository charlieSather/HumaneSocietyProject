using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
            catch (InvalidOperationException e)
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
            if (updatedAddress == null)
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

            if (employeeFromDb is null)
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
            catch (InvalidOperationException e)
            {
                UserInterface.DisplayUserOptions(new List<string> { "No employees have an EmployeeNumber that matches the employee passed in.", "No updates have been made." });
                return;
            }
            try
            {
                employeeFromDb.FirstName = employee.FirstName;
                employeeFromDb.LastName = employee.LastName;
                employeeFromDb.EmployeeNumber = employee.EmployeeNumber;
                employeeFromDb.Email = employee.Email;
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                UserInterface.DisplayUserOptions(ex.Message);
            }
        }

        internal static void DeleteEmployee(Employee employee)
        {
            Employee employeeFromDb = db.Employees.FirstOrDefault(e => e.EmployeeNumber == employee.EmployeeNumber && e.LastName == employee.LastName);

            if (employeeFromDb is null)
            {
                UserInterface.DisplayUserOptions("Employee not found.");
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
            Room availableRoom = null;
            try
            {
                //available room will be set to whatever room has the first case of an animal id being null
                //otherwise, if there are no rooms with no animal id, the available room will remain null
                availableRoom = db.Rooms.FirstOrDefault(a => a.AnimalId == null);
                if (availableRoom == null)
                {
                    UserInterface.DisplayUserOptions("Sorry, no more available rooms, can't add animal.");
                }
                else
                {
                    db.Animals.InsertOnSubmit(animal);
                    db.SubmitChanges();
                    availableRoom.AnimalId = animal.AnimalId;
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                UserInterface.DisplayUserOptions("There are no available rooms");
            }
        }

        internal static Animal GetAnimalByID(int id)
        {
            return db.Animals.FirstOrDefault(a => a.AnimalId == id);
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            foreach (var update in updates)
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
                        int age;
                        if( Int32.TryParse(update.Value, out age)) { db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Age = age; };                        
                        break;
                    case 4:
                        db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Demeanor = update.Value;
                        break;
                    case 5:
                        try { db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().KidFriendly = 
                                Convert.ToBoolean(update.Value); }catch(Exception e) { UserInterface.DisplayUserOptions(e.Message); }
                        break;
                    case 6:
                        try { db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().PetFriendly = 
                                Convert.ToBoolean(update.Value); }catch(Exception e) { UserInterface.DisplayUserOptions(e.Message); }
                        break;
                    case 7:
                        int weight;
                        if (Int32.TryParse(update.Value, out weight)) { db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault().Weight = weight; }
                        break;
                    default:
                        break;
                }
            }
            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            Room roomFromDb = null;
            AnimalShot animalShotFromDb = null;
            Adoption adoptionFromDb = null;
            if(animal is null)
            {
                return;
            }

            try
            {
                roomFromDb = db.Rooms.FirstOrDefault(a => a.AnimalId == animal.AnimalId);
                if(roomFromDb != null)
                {
                    roomFromDb.AnimalId = (int?)null;
                    db.SubmitChanges();
                }

                animalShotFromDb = db.AnimalShots.FirstOrDefault(a => a.AnimalId == animal.AnimalId);
                if (animalShotFromDb != null)
                {
                    db.AnimalShots.DeleteOnSubmit(animalShotFromDb);
                    db.SubmitChanges();
                }

                adoptionFromDb = db.Adoptions.FirstOrDefault(a => a.AnimalId == animal.AnimalId);
                if (adoptionFromDb != null)
                {
                    db.Adoptions.DeleteOnSubmit(adoptionFromDb);
                    db.SubmitChanges();
                }

                db.Animals.DeleteOnSubmit(animal);
                db.SubmitChanges();
            }
            catch(Exception ex)
            {
                //UserInterface.DisplayUserOptions("Error deleting animal from database");
                UserInterface.DisplayUserOptions(ex.Message);
            }
        }

        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> traits) // parameter(s)?
        {
            var animals = db.Animals.AsQueryable();
            foreach (var trait in traits)
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
                        animals = animals.Where(x => x.Age == (Parse.IsNumber(trait.Value) ? Int32.Parse(trait.Value) : (int?) null));
                        break;
                    case 4:
                        animals = animals.Where(x => x.Demeanor == trait.Value);
                        break;
                    case 5:
                        animals = animals.Where(x => x.KidFriendly == Parse.StringIntToBoolean(trait.Value));
                        break;
                    case 6:
                        animals = animals.Where(x => x.PetFriendly == Parse.StringIntToBoolean(trait.Value));
                        break;
                    case 7:
                        animals = animals.Where(x => x.Weight == (Parse.IsNumber(trait.Value)? Int32.Parse(trait.Value) : (int?) null));
                        break;
                    case 8:
                        animals = animals.Where(x => x.AnimalId == (Parse.IsNumber(trait.Value)? Int32.Parse(trait.Value) : (int?) null));
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
            return db.Rooms.FirstOrDefault(a => a.AnimalId == animalId);
        }

        internal static int GetDietPlanId(string dietPlanName)
        {
            return db.DietPlans.Where(a => a.Name == dietPlanName).Select(a => a.DietPlanId).FirstOrDefault();
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            if (animal.AdoptionStatus == "Available")
            {
                try
                {
                    Adoption adoption = new Adoption();
                    adoption.ClientId = client.ClientId;
                    adoption.AnimalId = animal.AnimalId;
                    adoption.ApprovalStatus = "Pending";
                    animal.AdoptionStatus = "Pending";
                    adoption.AdoptionFee = 75;
                    adoption.PaymentCollected = false;

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
            return db.Adoptions.Where(a => a.ApprovalStatus == "pending" || a.ApprovalStatus == "Pending");
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            try
            {
                if (isAdopted)
                {
                    db.Adoptions.Where(a => a.AnimalId == adoption.AnimalId && a.ClientId == adoption.ClientId).FirstOrDefault().ApprovalStatus = "Approved";
                    db.Animals.Where(a => a.AnimalId == adoption.AnimalId).FirstOrDefault().AdoptionStatus = "Approved";
                }
                else
                {
                    db.Animals.Where(a => a.AnimalId == adoption.AnimalId).FirstOrDefault().AdoptionStatus = "Available";
                    RemoveAdoption(adoption.AnimalId, adoption.ClientId);
                }
                db.SubmitChanges();
            }
            catch (Exception)
            {
                UserInterface.DisplayUserOptions("That adoption isn't in the database.");
            }
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            try
            {
                db.Adoptions.DeleteOnSubmit(db.Adoptions.FirstOrDefault(a => a.AnimalId == animalId && a.ClientId == clientId));
            }
            catch (Exception)
            {
                UserInterface.DisplayUserOptions("Error on trying to delete adoption.");
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

            if (shotFromDb is null)
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
                animalShotFromDb = db.AnimalShots.Where(x => x.AnimalId == animal.AnimalId && x.ShotId == shotFromDb.ShotId).FirstOrDefault();
            }
            catch
            {
                UserInterface.DisplayUserOptions("Error querying database looking for animalShot");
                return;
            }
            if (animalShotFromDb is null)
            {
                db.AnimalShots.InsertOnSubmit(new AnimalShot { AnimalId = animal.AnimalId, ShotId = shotFromDb.ShotId, DateReceived = DateTime.Now });
                db.SubmitChanges();
            }
            else
            {
                animalShotFromDb.ShotId = shotFromDb.ShotId;
                animalShotFromDb.DateReceived = DateTime.Now;
                db.SubmitChanges();
            }
        }
      
    }
}