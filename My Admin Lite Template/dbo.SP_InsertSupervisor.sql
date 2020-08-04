CREATE PROCEDURE SP_InsertSupervisor
	@Name varchar(50) 
AS
	insert into TB_M_Supervisor(Name) values(@Name);
return 0