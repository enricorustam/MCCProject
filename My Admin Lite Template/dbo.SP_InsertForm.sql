CREATE PROCEDURE SP_InsertForm
	@Name varchar(50), 
	@StartDate DateTime,
	@EndDate DateTime,
	@Duration int,
	@supervisor int,
	@department int
AS
	insert into TB_M_Form(Name, StartDate, EndDate, Duration, supervisorId, departmentId) values(@Name,@StartDate,@EndDate,@Duration,@supervisor,@department);
return 0