drop database if exists for_the_life_db;
create database for_the_life_db;
use for_the_life_db;

create table roles(
	id int primary key auto_increment,
    title varchar(32) NOT NULL UNIQUE
);

create table users(
	id int primary key auto_increment,
    role_id int NOT NULL,
    lastname varchar(128) NOT NULL,
    firstname varchar(128) NOT NULL,
    middlename varchar(128),
    username varchar(32) NOT NULL UNIQUE,
    pass varchar(256) NOT NULL,
    foreign key (role_id) references roles(id)
);

create table categories(
	id int primary key auto_increment,
    title varchar(32) NOT NULL UNIQUE
);

create table suppliers(
	id int primary key auto_increment,
    title varchar(32) NOT NULL UNIQUE
);

create table producers(
	id int primary key auto_increment,
    title varchar(32) NOT NULL UNIQUE
);

create table product_names(
	id int primary key auto_increment,
    title varchar(32) NOT NULL UNIQUE
);

create table units(
	id int primary key auto_increment,
    title varchar(32) NOT NULL UNIQUE
);

create table products(
	id int primary key auto_increment,
    category_id int NOT NULL,
	supplier_id int NOT NULL,
    producer_id int NOT NULL,
    product_name_id int NOT NULL references categories(id),
    unit_id int NOT NULL,
	article varchar(6) NOT NULL UNIQUE,
    price decimal(10, 2) NOT NULL,
    current_sale int not null,
    max_sale int not null,
	count int not null,
	description varchar(512),
    image_url varchar(128), 
    foreign key (category_id) references categories(id),
	foreign key (supplier_id) references suppliers(id),
    foreign key (producer_id) references producers(id),
    foreign key (product_name_id) references product_names(id),
    foreign key (unit_id) references units(id)
);

create table orders_statuses(
	id int primary key auto_increment,
    title varchar(32)
);

insert into orders_statuses(id, title)
values (1, 'Создан'), (2, 'Доставляется'), (3, 'Выполнен');

create table orders(
	id int primary key auto_increment,
    user_id int NOT NULL,
    orders_status_id int NOT NULL,
    foreign key (user_id) references users(id),
    foreign key (orders_status_id) references orders_statuses(id)
);

create table orders_products(
	id int primary key auto_increment,
    order_id int NOT NULL,
    product_id int NOT NULL,
    quantity int NOT NULL default 0,
    foreign key (order_id) references orders(id),
    foreign key (product_id) references products(id)
);


