create table IF NOT EXISTS Customers
(
    Id    char(36)  not null
        primary key,
    Cpf   varchar(11) not null,
    Name  varchar(100) null,
    Email varchar(100) null
);


create table IF NOT EXISTS Products
(
    Id          char(36)      not null comment 'product id'
        primary key,
    Name        varchar(100)  not null,
    Description varchar(200)  not null,
    Category    int           not null,
    Price       decimal(10,2)       not null,
    Images      varchar(1000) null
);


create table IF NOT EXISTS Orders
(
    Id         char(36)   not null,
    CustomerId char(36)   null,
    Status     int        not null,
    Created    datetime   null,
    Updated    datetime   null,
    TrackingCode       varchar(7) null
    --  constraint Order_Customer_Id_fk
    --      foreign key (CustomerId) references Customer (Id) null
);


create table IF NOT EXISTS OrderItems
(
    OrderId     char(36)     not null,
    ProductId   char(36)     not null,
    ProductName varchar(200) not null,
    UnitPrice   decimal      not null,
    Quantity    int          null
);

create table IF NOT EXISTS Payments
(
    Id         char(36)   not null,
    OrderId    char(36)   not null,
    Status     int        not null,
    Created    datetime   null,
    Updated    datetime   null,
    PaymentType int       not null,
    Amount     decimal(10,2) not null,
    PRIMARY KEY (Id, OrderId)
    );
