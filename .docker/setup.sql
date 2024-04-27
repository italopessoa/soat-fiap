create table Customer
(
    Id    varchar(34)  not null comment 'customer''s cpf of guid for anonymous customer'
        primary key,
    Name  varchar(100) not null,
    Email varchar(100) not null
);


create table Product
(
    Id    varchar(36)  not null comment 'product id'
        primary key,
    Name  varchar(100) not null,
    Description varchar(200) not null,
    Category int not null,
    Price decimal not null,
    Images varchar(1000) null
);


