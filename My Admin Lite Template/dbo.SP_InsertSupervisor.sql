CREATE PROCEDURE SP_InsertSupervisor
	@Name varchar(50) ,
	@Pass varchar(50) 
AS
	insert into TB_M_Supervisor(Name,Password) values(@Name,@Pass);
return 0