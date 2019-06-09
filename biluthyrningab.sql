use mercury

create table Customers
(
CustomerId int primary key identity,
FirstName nvarchar(32) not null,
LastName nvarchar(32) not null,
SSN varchar(12),
)
go

create table Cars
(
Id int primary key identity not null,
CarType nvarchar(32),
RegNr nvarchar(6),
Mileage int,
TimesRented int,
IsAvailable bit,
ToBeCleaned bit,
ToBeRemoved bit,
NeedService bit,
)
go

create table Orders
(
BookingNR int Primary key identity not null,
CustomerId int references Customers(CustomerID),
CarId int references Cars(Id) null,
BookingDate datetime default GETDATE(),
PickUpDate datetime,
ReturnDate datetime,
CurrentMileage int,
MileageOnReturn int,
DrivenMiles int,
IsReturned bit 
)
go

create table HistoryLog
(
Id int Primary key identity not null,
OrderId int references Orders(BookingNr) null,
CustomerId int references Customers(CustomerID) null,
CarId int references Cars(Id) null,
Activity nvarchar(max)
)


select * from Orders
select * from cars
select * from Customers
select * from HistoryLog
go










