  



INSERT INTO Categories VALUES('');
INSERT INTO Categories VALUES('');
INSERT INTO Categories VALUES('');
INSERT INTO Categories VALUES('');
INSERT INTO Categories VALUES('');


INSERT INTO DietPlans VALUES('','',);
INSERT INTO DietPlans VALUES('','',);
INSERT INTO DietPlans VALUES('','',);
INSERT INTO DietPlans VALUES('','',);
INSERT INTO DietPlans VALUES('','',);

/*
CREATE TABLE Animals (AnimalId INTEGER IDENTITY (1,1) PRIMARY KEY, 
Name VARCHAR(50), Weight INTEGER, Age INTEGER
, Demeanor VARCHAR(50), 
KidFriendly BIT, PetFriendly BIT, 
Gender VARCHAR(50), AdoptionStatus VARCHAR(50),
CategoryId INTEGER FOREIGN KEY REFERENCES Categories(CategoryId), 
DietPlanId INTEGER FOREIGN KEY REFERENCES DietPlans(DietPlanId), 
EmployeeId INTEGER FOREIGN KEY REFERENCES Employees(EmployeeId));
*/


INSERT INTO Animals VALUES('','','','','','','','','','','');
INSERT INTO Animals VALUES('','','','','','','','','','','');
INSERT INTO Animals VALUES('','','','','','','','','','','');
INSERT INTO Animals VALUES('','','','','','','','','','','');
INSERT INTO Animals VALUES('','','','','','','','','','','');



INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');
INSERT INTO Rooms VALUES ('','');

/*
CREATE TABLE Clients (ClientId INTEGER IDENTITY (1,1) PRIMARY KEY, 
FirstName VARCHAR(50), LastName VARCHAR(50), UserName VARCHAR(50), Password VARCHAR(50), 
AddressId INTEGER FOREIGN KEY REFERENCES Addresses(AddressId), Email VARCHAR(50));
*/

INSERT INTO Clients VALUES('','','','','','');
INSERT INTO Clients VALUES('','','','','','');
INSERT INTO Clients VALUES('','','','','','');
INSERT INTO Clients VALUES('','','','','','');
INSERT INTO Clients VALUES('','','','','','');

/*
CREATE TABLE Addresses (AddressId INTEGER IDENTITY (1,1) PRIMARY KEY, 
AddressLine1 VARCHAR(50), City VARCHAR(50), USStateId INTEGER FOREIGN KEY REFERENCES USStates(USStateId),  Zipcode INTEGER);
*/ 

INSERT INTO Addresses VALUES('','','','');
INSERT INTO Addresses VALUES('','','','');
INSERT INTO Addresses VALUES('','','','');
INSERT INTO Addresses VALUES('','','','');
INSERT INTO Addresses VALUES('','','','');


/*
CREATE TABLE Employees (EmployeeId INTEGER IDENTITY (1,1) PRIMARY KEY, 
FirstName VARCHAR(50), LastName VARCHAR(50), UserName VARCHAR(50), Password VARCHAR(50), EmployeeNumber INTEGER, Email VARCHAR(50));
*/

INSERT INTO Employees VALUES('','','','','','');
INSERT INTO Employees VALUES('','','','','','');
INSERT INTO Employees VALUES('','','','','','');
INSERT INTO Employees VALUES('','','','','','');
INSERT INTO Employees VALUES('','','','','','');












