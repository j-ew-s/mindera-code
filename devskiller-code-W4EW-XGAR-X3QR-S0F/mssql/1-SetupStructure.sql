CREATE DATABASE blog
GO 

USE blog;
GO 


CREATE TABLE blog.dbo.Post (
	Id uniqueidentifier PRIMARY KEY default NEWID(),
	Title varchar(30) NOT NULL,
	Content varchar(1200) NOT NULL,
	CreationDate datetime2 NOT NULL
);
CREATE INDEX Post_Id_IDX ON blog.dbo.Post (Id);

CREATE TABLE blog.dbo.Comment (
	Id uniqueidentifier PRIMARY KEY default NEWID(),
	Author varchar(30) NOT NULL,
	Content varchar(120) NOT NULL,
	CreationDate datetime2 NOT NULL,
	PostId uniqueidentifier FOREIGN KEY REFERENCES blog.dbo.Post(Id)
);

