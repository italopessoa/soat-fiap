create table Customer
(
    Id    varchar(34)  not null comment 'customer''s cpf of guid for anonymous customer'
        primary key,
    Name  varchar(100) not null,
    Email varchar(100) not null
);


create table Product
(
    Id          char(36)      not null comment 'product id'
        primary key,
    Name        varchar(100)  not null,
    Description varchar(200)  not null,
    Category    int           not null,
    Price       decimal       not null,
    Images      varchar(1000) null
);


create table Orders
(
    Id         char(36)   not null,
    CustomerId char(36)   null,
    Status     int        not null,
    Created    datetime   null,
    Updated    datetime   null,
    Code       varchar(4) null
    --  constraint Order_Customer_Id_fk
    --      foreign key (CustomerId) references Customer (Id) null
);


create table OrderItems
(
    OrderId     char(36)     not null,
    ProductId   char(36)     not null,
    ProductName varchar(200) not null,
    UnitPrice   decimal      not null,
    Quantity    int          null
);