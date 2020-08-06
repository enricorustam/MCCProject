CREATE PROCEDURE SP_InsertForm
	@employee int,
	@StartDate DateTime,
	@EndDate DateTime,
	@Duration int,
	@supervisor int,
	@department int
AS
	insert into TB_M_Form(employeeId, StartDate, EndDate, Duration, supervisorId, departmentId) values(@employee,@StartDate,@EndDate,@Duration,@supervisor,@department);
return 0