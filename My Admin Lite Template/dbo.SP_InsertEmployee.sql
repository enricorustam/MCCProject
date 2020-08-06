CREATE PROCEDURE SP_InsertEmployee
	@NIP int,
	@Name varchar(50) 
AS
	insert into TB_M_Employee(Name, NIP) values(@Name,@NIP);
return 0